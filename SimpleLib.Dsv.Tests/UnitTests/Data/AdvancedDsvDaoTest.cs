using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SimpleLib.Data;
using SimpleLibTest;

namespace SimpleLibTest.UnitTests.Data
{
    [TestClass]
    public class AdvancedDsvDaoTest
    {
        //public static string DataFolder = FileHelper.GetProgramFolder() + @"..\..\..\TestData\";
        public static string DataFolder = FileHelper.GetProgramFolder() 
            + (".." + Path.DirectorySeparatorChar).Multiply(3)
            + "TestData" + Path.DirectorySeparatorChar;
        
        [TestMethod]
        public void TestReadOperations()
        {
            var dao = new AdvancedDsvDao(DataFolder + "AdvancedReadOnlyTsv.txt", '\t', true);
            Assert.AreEqual("Name", dao.GetHeaders()[0]);
            
            var dataEntries = dao.GetDataEntries().ToList();
            Assert.AreEqual(2, dataEntries.Count());
            Assert.AreEqual("simple text", dataEntries[0][2]);
            Assert.AreEqual("Asturian", dataEntries[1][3]);
        }

        [TestMethod]
        public void TestAddRemoveHeaders()
        {
            AdvancedDsvDao dao = new AdvancedDsvDao(DataFolder + "AdvancedReadWriteTsv.txt", '\t', true);
            var headers = dao.GetHeaders();
            int dataEntriesCount = dao.GetDataEntries().Count();

            dao.RemoveHeaders();
            Assert.AreEqual(null, dao.GetHeaders());
            Assert.AreEqual(dataEntriesCount, dao.GetDataEntries().Count());

            dao.UpdateHeaders(headers);
            Assert.AreEqual("Name", dao.GetHeaders()[0]);
            Assert.AreEqual(dataEntriesCount, dao.GetDataEntries().Count());
        }

        [TestMethod]
        public void TestAddRemoveDataEntries()
        {
            AdvancedDsvDao dao = new AdvancedDsvDao(DataFolder + "AdvancedReadWriteTsv.txt", '\t', true);
            var entries = dao.GetDataEntries().ToList();


            ////my own tests to see how the enumerator works
            //var entries1 = dao.GetDataEntries();
            //int count1 = entries1.Count();
            //int count2 = entries1.Count();

            dao.RemoveDataEntries();
            Assert.AreEqual(0, dao.GetDataEntries().Count());
            
            dao.AddDataEntries(entries);
            Assert.AreEqual(entries.Count(), dao.GetDataEntries().Count());
            Assert.AreEqual("Johan", dao.GetDataEntries().ToList()[0][0]);

            dao.AddDataEntries(entries);
            Assert.AreEqual(entries.Count() * 2, dao.GetDataEntries().Count());
        }
    }
}

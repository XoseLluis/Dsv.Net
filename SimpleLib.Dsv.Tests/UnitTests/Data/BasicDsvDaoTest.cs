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
    public class BasicDsvDaoTest
    {
        //the Program folder for tests (that is, the path from which the tests run) is something like this:
        //"SolutionRoot"\TestResults\jsampayo_JSAMPAYO-1 2012-10-04 15_10_07\Out
        //public static string DataFolder = FileHelper.GetProgramFolder() + @"..\..\..\TestData\";
        public static string DataFolder = FileHelper.GetProgramFolder()
            + (".." + Path.DirectorySeparatorChar).Multiply(3)
            + "TestData" + Path.DirectorySeparatorChar;

        [TestMethod]
        public void TestReadOperations()
        {
            BasicDsvDao dao = new BasicDsvDao(DataFolder + "BasicReadOnlyTsv.txt", '\t', true);
            Assert.AreEqual("Name", dao.GetHeaders()[0]);
            Assert.AreEqual(2, dao.GetDataEntries().Count());
        }

        
    }
}

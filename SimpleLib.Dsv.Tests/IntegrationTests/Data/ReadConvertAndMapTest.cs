using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SimpleLib.Data;
using SimpleLib.Data.Mapping;

using SimpleLibTest;

namespace SimpleLibTest.IntegrationTests.Data
{
    [TestClass]
    public class ReadConvertAndMapTest
    {
        [TestMethod]
        public void ReadConvertAndMap()
        {
            var dicToObjMappingInfos = new List<DictionaryItemToObjectMappingInfo>()
            {
                new DictionaryItemToObjectMappingInfo()
                {
                    Key = "person name",
                    PropertyName = "Name",
                    FromStringMapping = st => st,
                    ToStringMapping = ob => (string)ob
                }, 
                new DictionaryItemToObjectMappingInfo()
                {
                    Key = "how old",
                    PropertyName = "Age",
                    FromStringMapping = st => Convert.ToInt32(st),
                    ToStringMapping = num => num.ToString()
                }
            };

            var dirSep = System.IO.Path.DirectorySeparatorChar;
            //var filePath = FileHelper.GetProgramFolder() + @"..\..\..\TestData\ReadAndMapTsv.txt";
            var filePath = FileHelper.GetProgramFolder()
                + (".." + dirSep).Multiply(3)
                + "TestData" + dirSep
                + "ReadAndMapTsv.txt";
            var dao = new AdvancedDsvDao(filePath, '\t', true);
            var headersList = dao.GetHeaders();

            var colToObjMappingInfos = new MappingInfoConverter().Convert(dicToObjMappingInfos, headersList).ToList();
            Assert.AreEqual(0, colToObjMappingInfos[0].Index);
            Assert.AreEqual(3, colToObjMappingInfos[1].Index);

            //the "real life" usage would go on like this:
            var mapper = new CollectionToFromObjectMapper<Person>(colToObjMappingInfos);
            var people = dao.GetDataEntries()
                .Select(personRow => mapper.Map(personRow))
                .ToList();

            Assert.AreEqual(people[0].Name, "Johan");
            Assert.AreEqual(people[0].Age, 34);

            Assert.AreEqual(people[1].Name, "Xose");
            Assert.AreEqual(people[1].Age, 36);
        }
    }
}

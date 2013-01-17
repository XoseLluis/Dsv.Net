using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SimpleLib.Data;
using SimpleLib.Data.Mapping;

namespace SimpleLibTest.UnitTests.Data.Mapping
{
    [TestClass]
    public class MappingInfoConverterTest
    {
        [TestMethod]
        public void Conversion()
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

            var headersList = new List<string>() { "city", "person name", "country", "how old" };
           
            var colToObjMappingInfos = new MappingInfoConverter().Convert(dicToObjMappingInfos, headersList).ToList();
            Assert.AreEqual(1, colToObjMappingInfos[0].Index);
            Assert.AreEqual(3, colToObjMappingInfos[1].Index);
        }
    }
}

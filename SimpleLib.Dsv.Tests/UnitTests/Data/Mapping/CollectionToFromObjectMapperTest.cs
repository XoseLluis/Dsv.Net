using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleLib.Data.Mapping;

namespace SimpleLibTest.UnitTests.Data.Mapping
{
    [TestClass]
    public class CollectionToFromObjectMapperTest
    {
        List<DictionaryItemToObjectMappingInfo> dicToObjMappingInfos = new List<DictionaryItemToObjectMappingInfo>()
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

        List<string> headers = new List<string>() 
            { 
                "city", 
                "person name", 
                "country", 
                "how old" 
            };
        
        [TestMethod]
        public void MapCollectionToObject()
        {
            var colToObjMappingInfos = new MappingInfoConverter().Convert(dicToObjMappingInfos, headers).ToList();
            
            var collection = new List<string>()
            {
                "Xixón", 
                "Xuan", 
                "Asturies", 
                "25"
            };
            var mapper = new CollectionToFromObjectMapper<Person>(colToObjMappingInfos);
            Person p1 = mapper.Map(collection);
            Assert.AreEqual("Xuan", p1.Name);
            Assert.AreEqual(25, p1.Age);
        }

        [TestMethod]
        public void MapObjectToCollection()
        {
            var colToObjMappingInfos = new MappingInfoConverter().Convert(dicToObjMappingInfos, headers).ToList();
            Person p1 = new Person()
            {
                Name = "Xuan",
                Age = 25
            };
           
            var mapper = new CollectionToFromObjectMapper<Person>(colToObjMappingInfos);
            var collection = mapper.Map(p1);
            Assert.AreEqual(p1.Name, collection);
            Assert.AreEqual(25, p1.Age);
        }
    }
}

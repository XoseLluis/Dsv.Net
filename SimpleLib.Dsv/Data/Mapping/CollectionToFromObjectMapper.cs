using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace SimpleLib.Data.Mapping
{
    /// <summary>
    /// we contemplate the case where not all items in the List are mapped to a property in the object, that is, we can have a List with 10 elements
    /// and only fields 0, 3 and 5 be mapped to properties of the object.
    /// This is mainly significant when mapping from Object to List, cause we'll have to create at least elements 1, 2 and 4 as null elements
    /// (creating the nulls from 6 to 9 seems unnecessary)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CollectionToFromObjectMapper<T> where T: new()
    { 
        
        /// <summary>
        /// in the end we have to resort to Reflection instead of using dynamic, as we need access based on a property string name
        /// to try to speed it up a bit, we'll use this internal additional class storing the PropertyInfo
        /// </summary>
        protected class CollectionToObjectMappingInfoAdvanced : CollectionItemToObjectMappingInfo
        {
            public PropertyInfo PropertyInfo { get; set; }
        }
        
        protected List<CollectionToObjectMappingInfoAdvanced> MappingInfos { get; set; }
        private int listSize;

        public CollectionToFromObjectMapper(List<CollectionItemToObjectMappingInfo> mappingInfos)
        {
            //this.MappingInfos = mappingInfos;
            this.MappingInfos = new List<CollectionToObjectMappingInfoAdvanced>();
            foreach (var mapInfo in mappingInfos)
            {
                this.MappingInfos.Add(new CollectionToObjectMappingInfoAdvanced()
                {
                    Index = mapInfo.Index,
                    PropertyName = mapInfo.PropertyName,
                    PropertyInfo = typeof(T).GetProperty(mapInfo.PropertyName),
                    FromStringMapping = mapInfo.FromStringMapping,
                    ToStringMapping = mapInfo.ToStringMapping
                });
            }
            this.listSize = this.MappingInfos.Max(mI => mI.Index) + 1;
        }

        public T Map(List<string> collection)
        {
            /*
            dynamic obj = new T();
            foreach (var mappingInfo in this.MappingInfos)
            {
                ((IDictionary<string, object>)obj)[mappingInfo.PropertyName] = mappingInfo.FromStringMapping(collection[mappingInfo.Index]);
            }
            */
            T obj = new T();
            foreach (var mappingInfo in this.MappingInfos)
            {
                mappingInfo.PropertyInfo.SetValue(obj, mappingInfo.FromStringMapping(collection[mappingInfo.Index]), null);
            }
            return obj;
        }

        //fill with nulls those indexes not present in the mapping list
        public List<string> Map(T obj)
        {
            List<string> result = new List<string>();
            for (var i=0; i<this.listSize; i++)
                result.Add(null);

            foreach (var mappingInfo in this.MappingInfos)
            {
                //result[mappingInfo.Index] = ((dynamic)obj)[mappingInfo.PropertyName];
                result[mappingInfo.Index] = mappingInfo.ToStringMapping(mappingInfo.PropertyInfo.GetValue(obj, null));
            }
            return result;
        }
    }
}

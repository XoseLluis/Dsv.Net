using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLib.Data.Mapping
{
    public class MappingInfoConverter
    {
        public IEnumerable<CollectionItemToObjectMappingInfo> Convert(IEnumerable<DictionaryItemToObjectMappingInfo>dicToObjInfos, List<string> keys)
        {
            var result = new List<CollectionItemToObjectMappingInfo>();
            foreach (var dicToObjInfo in dicToObjInfos)
            {
                int index = keys.IndexOf(dicToObjInfo.Key);
                if (index != -1)
                    result.Add(new CollectionItemToObjectMappingInfo()
                    {
                        Index = index,
                        PropertyName = dicToObjInfo.PropertyName,
                        FromStringMapping = dicToObjInfo.FromStringMapping,
                        ToStringMapping = dicToObjInfo.ToStringMapping
                    });
            }
            return result;
        }
    }
}

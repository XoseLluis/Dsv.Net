using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLib.Data.Mapping
{
    public class CollectionItemToObjectMappingInfo /*<T> where T : new()*/
    {
        public int Index { get; set; }
        public string PropertyName { get; set; }
        public Func<string, Object> FromStringMapping { get; set; }
        public Func<Object, string> ToStringMapping { get; set; }
    }
}

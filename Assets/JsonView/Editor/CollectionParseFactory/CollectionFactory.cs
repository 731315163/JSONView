

using System;
using System.Collections;

namespace CTWCYS.Editor.JsonView
{
    public class CollectionFactory
    {
        public static  CollectionInterface CreateCollectionParse(Type type)
        {
            if (type.IsArray)
            {
                return new ArrayCollection();
            }
            if(typeof(IDictionary).IsAssignableFrom(type))
            {
                return new DictionaryCollection();
            }
            if (typeof(IList).IsAssignableFrom(type))
            {
                return new ListCollection();
            }
            return null;
        }
    }
}
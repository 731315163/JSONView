

using System;
using System.Collections;
using System.Collections.Generic;

namespace CTWCYS.Editor.JsonView
{
    public class DictionaryCollection:CollectionInterface
    {
        public void Add(Element ele)
        {
            Reflection(ele, delegate (IDictionary dic, object key, object value)
            {
                dic.Add(key, value);
            });

        }

        public void SetValue(Element ele)
        {
            Reflection(ele, delegate (IDictionary dic, object key, object value)
            {
                dic[key] = value;
            });

        }

        public void Del(Element ele)
        {
            Reflection(ele, delegate (IDictionary dic, object key, object value)
            {
               dic.Remove(key);
            });
            ele.Father.ChildElements.Remove(ele);
        }

        private void Reflection(Element ele, Action<IDictionary, object, object> action)
        {
            var dic = ele.Father.Value as IDictionary;
            var keyvaluetype = ele.Value.GetType();
            var key = keyvaluetype.GetProperty("Key").GetGetMethod().Invoke(ele.Value, null);
            var value = keyvaluetype.GetProperty("Value").GetGetMethod().Invoke(ele.Value, null);
            action(dic, key, value);

        }
    }
}
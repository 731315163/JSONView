

using System;
using System.Collections;

namespace CTWCYS.Editor.JsonView
{
    public class ListCollection:CollectionInterface
    {
        public void Add(Element ele)
        {
            var list = ele.Father.Value as IList;
            list.Add(ele.Value);
        }

        public void SetValue(Element ele)
        {
            var list = ele.Father.Value as IList;
            list[ele.Father.ChildElements.IndexOf(ele)] = ele.Value;
        }

        public void Del(Element ele)
        {
            var list = ele.Father.Value as IList;
            list.Remove(ele.Value);
        }

    }
}
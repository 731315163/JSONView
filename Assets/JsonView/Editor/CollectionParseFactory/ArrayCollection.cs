

using System;

namespace CTWCYS.Editor.JsonView
{
    public class ArrayCollection : CollectionInterface
    {
        public void Add(Element ele)
        {
            throw new Exception("数组是定长的");
        }

        public void Del(Element ele)
        {
            SetValue(ele);
        }

        public void SetValue(Element ele)
        {
            var childtype = ele.Father.Value.GetType().GetElementType();
            var array = ele.Father.Value as System.Array;                     
            ele.AssignValue(ele.Value,childtype , delegate(object obj, object value)
            {
                 array.SetValue(value,ele.Father.ChildElements.IndexOf(ele));
            });
           
            
        }

        
    }
}
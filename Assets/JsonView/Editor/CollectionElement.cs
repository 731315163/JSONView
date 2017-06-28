

using System;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CTWCYS.Editor.JsonView
{
    public class CollectionElement:Element
    {
        public override void SetChildValue(Element ele)
        {
            var index = this.ChildElements.IndexOf(ele);
            if (index == -1)
            {
                CollectionFactory.CreateCollectionParse(ValueType).Add(ele);             
            }
            else
            {
                CollectionFactory.CreateCollectionParse(ValueType).SetValue(ele);
            }
        }

        public virtual void DelChildValue(Element ele)
        {
            CollectionFactory.CreateCollectionParse(ValueType).Del(ele);
        }

       
    }
}
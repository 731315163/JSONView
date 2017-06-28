using System;
using System.Collections.Generic;
using System.Reflection;

namespace CTWCYS.Editor.JsonView
{
    public class Element
   {
       public string Name;
       public Element Father;
       public List<Element> ChildElements  = new List<Element>();
       public Type ValueType;
       public MemberFlag flag;
       private object value;
       public object Value
       {
           get { return value; }
           set
           {
               AssignValue(value,ValueType,delegate(object target,object arrow) { this.value= arrow;});   
               if(Father != null)Father.SetChildValue(this);
           }
       }

       public virtual void SetChildValue(Element ele)
       {
           switch (ele.flag)
           {
               case MemberFlag.Field:
               {
                   var fieldinfo = ele.Father.ValueType.GetField(ele.Name);
                   AssignValue(ele.Value,ele.ValueType, fieldinfo.SetValue);                   
                   break;
               }
               case MemberFlag.Property:
               {
                   var propertyinfo = ele.Father.ValueType.GetProperty(ele.Name);
                   if (propertyinfo.CanWrite)
                   {
                       AssignValue(ele.Value, ele.ValueType, delegate(object target, object arrow)
                       {
                           propertyinfo.GetSetMethod().Invoke(target,new object[]{ arrow});
                       });
                   }
                   break;
               }
           }
           
       }
        /// <summary>
        /// 通过类型赋值
        /// </summary>
        /// <param name="childvalue">值</param>
        /// <param name="rightValueType">值应该的属性</param>
        /// <param name="setValue"> 如何赋值的函数</param>
       public virtual void AssignValue(object childvalue,Type rightValueType,Action<object,object> setValue)
       {
           if (childvalue == null)
           {
                setValue(this.value, childvalue);
                return;
           }
           var type = childvalue.GetType();            
           if (rightValueType.IsClass)
           {
               if(rightValueType != type && ! type.IsSubclassOf(rightValueType)) throw new Exception("childvalue missing type ");             
                   setValue(this.value,childvalue);                                 
           }
           else
           {
               if (rightValueType.IsEnum)
               {
                   setValue(this.value, Enum.Parse(rightValueType, childvalue as string));
               }
               else
               {
                    setValue(this.value, Convert.ChangeType(childvalue, rightValueType));
               }
           }
        }
    }
}
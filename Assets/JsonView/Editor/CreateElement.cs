using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CTWCYS.Editor.JsonView
{
    public class CreateElement
    {
        public  Element GetRootElement(object obj)
        {
            return RecuriveParseType(obj, obj.GetType(),null, "", MemberFlag.Null);
        }
     
        private Element RecuriveParseType(object obj, Type type,Element father,string name,MemberFlag flag)
        { 
            if(obj == null && father == null)throw new NullReferenceException("element and element father are null");
            if (obj == null)
            {
                if (flag == MemberFlag.Field)
                {
                    type = father.ValueType.GetField(name).FieldType;
                    return getOneElement(obj, type, father, name, flag);
                }
                if (flag == MemberFlag.Property)
                {
                    type = father.ValueType.GetProperty(name).PropertyType;
                    return getOneElement(obj, type, father, name, flag);
                }
            }
            if (obj is bool ||
                obj is byte ||
                obj is char ||
                obj is decimal ||
                obj is double ||
                obj is Enum ||
                obj is float ||
                obj is int ||
                obj is long ||
                obj is sbyte ||
                obj is short ||
                obj is uint ||
                obj is ulong ||
                obj is ushort ||
                obj is string || obj is StringBuilder
            )
            {
                return getOneElement(obj,type, father, name, flag);
            }
            //数组 注意字符串实现了IEnumerable接口
            if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                var array = obj as IEnumerable;
                var ele = getOneElement(obj, type,father, name, flag);
                foreach (var item in array)
                {
                    RecuriveParseType(item, item.GetType(),ele, "", MemberFlag.Content);
                }
                return ele;
            }
            //自定义类或结构体     
            else
            {
                 var ele = getOneElement(obj, type,father, name, flag);
                 setFields(ele);
                return ele;
            }
         }

        /// <summary>
        /// 反射取得字段，并调用RecuriveParseType生成element
        /// </summary>
        /// <param name="father"></param>
        private void setFields(Element father)
        {
            var fields = father.ValueType.GetFields();
            foreach (var fieldInfo in fields)
            {
                RecuriveParseType(fieldInfo.GetValue(father.Value), fieldInfo.FieldType,father, fieldInfo.Name, MemberFlag.Field);
            }
        }

        /// <summary>
        /// 创建集合collectionelement或者是element
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="father"></param>
        /// <param name="name"></param>
        /// <param name="flag"></param>
        /// <returns></returns>


        public Element getOneElement(object obj, Type type, Element father, string name, MemberFlag flag)
        {
            Element ele = new Element();
            if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                ele = new CollectionElement();
            }
            ele.Father = father;
            ele.Name = name;
            ele.flag = flag;
            ele.ValueType = type;
            ele.Value = obj;
            if (father != null) father.ChildElements.Add(ele);
            return ele;
        }
    }
}

using System;
using NUnit.Framework;
namespace CTWCYS.Editor.JsonView
{
   /// <summary>
   /// 未完成的单元测试
   /// </summary>
    [TestFixture]
    public class ElementTest
    {
        /// <summary>
        /// 测试嵌套结构声明的类
        /// </summary>
        class Foo
        {
            public string name;
            public Foo foo;
        }
        public Element ele;
        [SetUp]
        public void SetUP()
        {
            ele = new Element();
            ele.Father = null;
            ele.Name = "";
            ele.flag = MemberFlag.Null;
        }
        [Test]
        public void ValueTest()
        {
            ele.ValueType = typeof(int);
            ele.Value = 10;
            Assert.AreEqual(ele.Value ,10);
        }

        [Test]
        public void ValueExceptionTest()
        {
            try
            {
                ele.ValueType = typeof(int);
                ele.Value = "Ex";
            }
            catch (FormatException e)
            {
                //All good
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void ValueTest1()
        {
            var foo = new Foo();
            Element test = new CreateElement().GetRootElement(foo);
        }
    }
}
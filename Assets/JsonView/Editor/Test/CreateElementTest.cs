using NUnit.Framework;
using UnityEditor.Graphs;

namespace CTWCYS.Editor.JsonView
{
    [TestFixture()]
    public class CreateElementTest
    {
        class FooLoop
        {
            public string name;
            public FooLoop foo;
        }

        class Foo1
        {
            public int i;
        }
        public CreateElement CreateElement;

        [SetUp]
        public void SetUp()
        {
            CreateElement = new CreateElement();
        }
        

        [Test]
        public void GetRootElementTest()
        {
            var foo = new Foo1();
            var ele = CreateElement.GetRootElement(foo);
            Assert.AreEqual(ele.ChildElements[0].ValueType, typeof(int));
        }

        [Test]
        public void GetRootElementLoopTest()
        {
            var foo = new FooLoop();
            var ele = CreateElement.GetRootElement(foo);
            Assert.AreEqual(ele.ChildElements[1].ValueType , typeof(FooLoop));
            Assert.IsNull(ele.ChildElements[1].Value);
        }

    }
}
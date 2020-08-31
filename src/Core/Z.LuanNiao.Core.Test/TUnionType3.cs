using LuanNiao.Core;
using NUnit.Framework;

namespace Z.LuanNiao.Core.Test
{
    [TestFixture]
    public class TUnionType3
    {
        private class TestClass
        {
            public int MyProperty { get; set; }
        }

        [SetUp]
        public void Setup()
        {
            UnionType<int, string, TestClass> _testInstance = 1;
            _testInstance = "";
            _testInstance = new TestClass();
        }

        [Test]
        public void TestSetValue()
        {
            UnionType<int, string, TestClass> _testInstance = 1;
            Assert.AreEqual(_testInstance.Value, 1);
            _testInstance = 2;
            Assert.AreEqual(_testInstance.Value, 2);
        }

        [Test]
        public void TestSetValue2()
        {
            UnionType<int, string, TestClass> _testInstance = "Test";
            Assert.AreEqual(_testInstance.Value, "Test");
            _testInstance = "Ben";
            Assert.AreEqual(_testInstance.Value, "Ben");
        }


        [Test]
        public void TestSetValue3()
        {
            var c1 = new TestClass() { MyProperty = 2 };
            var c2 = new TestClass() { MyProperty = 3 };
            UnionType<int, string, TestClass> _testInstance = c1;
            Assert.AreEqual(_testInstance.Value, c1);
            _testInstance = c2;
            Assert.AreEqual(_testInstance.Value, c2);
        }
        [Test]
        public void TestSetDiffTypeValue()
        {
            UnionType<int, string, TestClass> _testInstance = 1;
            _testInstance = 2;
            _testInstance = "s";
            _testInstance = new TestClass() { MyProperty = 2 };
            Assert.AreNotEqual(_testInstance.Value, 2);
        }


        [Test]
        public void TestSetSameTypeDiffValueAndEqual()
        {
            var c1 = new TestClass() { MyProperty = 2 };
            var c2 = new TestClass() { MyProperty = 2 };
            UnionType<int, string, TestClass> _testInstance = c1;
            UnionType<int, string, TestClass> _testInstance2 = c2;
            Assert.AreNotEqual(_testInstance, _testInstance2);
        }


        [Test]
        public void TestSetSameTypeAndSameValueEqual()
        {
            var c1 = new TestClass() { MyProperty = 2 };
            UnionType<int, string, TestClass> _testInstance = c1;
            UnionType<int, string, TestClass> _testInstance2 = c1;
            Assert.AreEqual(_testInstance, _testInstance2);
        }

        [Test]
        public void TestSetSameTypeAndSameValueEqualUseOperator()
        {
            var c1 = new TestClass() { MyProperty = 2 };
            UnionType<int, string, TestClass> _testInstance = c1;
            UnionType<int, string, TestClass> _testInstance2 = c1;
            Assert.IsTrue(_testInstance == _testInstance2);
        }
    }
}
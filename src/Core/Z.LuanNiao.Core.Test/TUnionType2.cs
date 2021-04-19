using LuanNiao.Core;
using NUnit.Framework;

namespace Z.LuanNiao.Core.Test
{
    [TestFixture]
    public class TUnionType2
    {
        [SetUp]
        public void Setup()
        {
        }
        [Test]
        public void TestNullDecide()
        {
            UnionType<int, string> _testInstance = 1;

            Assert.IsFalse(_testInstance == null);
        }

        [Test]
        public void TestNotNullDecide()
        {
            UnionType<int, string> _testInstance = 1;

            Assert.IsTrue(_testInstance != null);
        }
        [Test]
        public void TestSetValue()
        {
            UnionType<int, string> _testInstance = 1;
            Assert.AreEqual(_testInstance.Value, 1);
            _testInstance = 2;
            Assert.AreEqual(_testInstance.Value, 2);
        }

        [Test]
        public void TestSetValue2()
        {
            UnionType<int, string> _testInstance = "Test";
            Assert.AreEqual(_testInstance.Value, "Test");
            _testInstance = "Ben";
            Assert.AreEqual(_testInstance.Value, "Ben");
        }

        [Test]
        public void TestSetDiffTypeValue()
        { 
            UnionType<int, string> _testInstance = "s";
            Assert.AreNotEqual(_testInstance.Value, 2);
        }


        [Test]
        public void TestSetSameTypeDiffValueAndEqual()
        {
            UnionType<int, string> _testInstance = 1;
            UnionType<int, string> _testInstance2 = 2;
            Assert.AreNotEqual(_testInstance, _testInstance2);
        }


        [Test]
        public void TestSetSameTypeAndSameValueEqual()
        {
            UnionType<int, string> _testInstance = 2;
            UnionType<int, string> _testInstance2 = 2;
            Assert.AreEqual(_testInstance, _testInstance2);
        }

        [Test]
        public void TestSetSameTypeAndSameValueEqualUseOperator()
        {
            UnionType<int, string> _testInstance = 2;
            UnionType<int, string> _testInstance2 = 2;
            Assert.IsTrue(_testInstance == _testInstance2);
        }
    }
}
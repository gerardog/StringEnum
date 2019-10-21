using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace StringEnum.Tests
{
    [TestClass]
    public class DictionaryKeyTest
    {
        [TestMethod]
        public void UsageAsDictionaryKeyTest()
        {
            var myDic = new Dictionary<Color, int>();
            myDic.Add(Color.Blue, 1);
            myDic.Add(Color.Green, 2);
            myDic.Add(Color.Red, 3);

            Assert.AreEqual(myDic[Color.Blue], 1);
            Assert.AreEqual(myDic[Color.Green], 2);
            Assert.AreEqual(myDic[Color.Red], 3);

            Assert.AreEqual(myDic[Color.Parse("Blue")], 1);
            Assert.AreEqual(myDic[Color.Parse("Green")], 2);
            Assert.AreEqual(myDic[Color.Parse("Red")], 3);
        }
    }
}

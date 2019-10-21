using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace StringEnum.Sample.NewtonsoftSerialization
{
    [TestClass]
    public class JsonUsageExample
    {
        const string expected = "{\"MyAnimal\":\"Cat\",\"MyString\":\"HelloWorld\"}";

        [TestMethod]
        public void Serialize()
        {
            var obj = new { MyAnimal = Animal.Cat, MyString = "HelloWorld" };
            var str = JsonConvert.SerializeObject(obj, new StringEnumJsonConverter());
            Assert.AreEqual(expected, str);
        }

        [TestMethod]
        public void Deserialize()
        {
            var obj = new { MyAnimal = (Animal)null, MyString = string.Empty };
            var result = JsonConvert.DeserializeAnonymousType(expected, obj);

            Assert.AreEqual("HelloWorld", result.MyString);
            Assert.AreEqual(Animal.Cat, result.MyAnimal);
        }

    }
}

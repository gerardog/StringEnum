using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using StringEnum;
using System;
using System.Reflection;

namespace StringEnum.Tests
{
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumJsonConverter))]
    class JsonSerializableColor : StringEnum<JsonSerializableColor>
    {
        public static readonly JsonSerializableColor Blue = New("Blue");
        public static readonly JsonSerializableColor Red = New("Red");
        public static readonly JsonSerializableColor Green = New("Green");
    }

    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class JsonTests
    {
        const string expected = "{\"Color\":\"Red\",\"MyString\":\"HelloWorld\"}";

        [TestMethod]
        public void SerializeJsonMarked()
        {
            var obj = new { Color = JsonSerializableColor.Red, MyString = "HelloWorld" };
            var str = JsonConvert.SerializeObject(obj);
            Assert.AreEqual(expected, str);
        }

        [TestMethod]
        public void DeserializeJsonMarked()
        {
            var obj = new { Color = (JsonSerializableColor)null, MyString = string.Empty };

            var str = JsonConvert.DeserializeAnonymousType(expected, obj);

            Assert.AreEqual("HelloWorld", str.MyString);
            Assert.AreEqual(JsonSerializableColor.Red, str.Color);
        }

        [TestMethod]
        //  Serialize a class not marked as serializable
        public void SerializeNotMarked()
        { 
            var obj = new { Color = Color.Red, MyString = "HelloWorld" };
            var str = JsonConvert.SerializeObject(obj, new StringEnumJsonConverter());
            Assert.AreEqual(expected, str);
        }

        [TestMethod]
        //  De-Serialize a class not marked as serializable
        public void DeserializeNotMarked()
        {
            var obj = new { Color = (Color)null, MyString = string.Empty };

            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new StringEnumJsonConverter());
            var result = JsonConvert.DeserializeAnonymousType(expected, obj, settings);

            Assert.AreEqual("HelloWorld", result.MyString);
            Assert.AreEqual(Color.Red, result.Color);
        }
    }
}
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using StringEnum;
using System;
using System.Reflection;

namespace StringEnum.Tests
{
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
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
        public void Serialize()
        { 
            var obj = new { Color = Color.Red, MyString = "HelloWorld" };
            var str = JsonConvert.SerializeObject(obj, new StringEnumConverter());
            Assert.AreEqual(expected, str);
        }

        [TestMethod]
        //  De-Serialize a class not marked as serializable
        public void Deserialize()
        {
            var obj = new { Color = (Color)null, MyString = string.Empty };

            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new StringEnumConverter());
            var str = JsonConvert.DeserializeAnonymousType(expected, obj, settings);

            Assert.AreEqual("HelloWorld", str.MyString);
            Assert.AreEqual(Color.Red, str.Color);
        }
    }

    public class StringEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return IsSubclassOfRawGeneric(typeof(StringEnum<>), objectType);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string s = (string)reader.Value;
            return typeof(StringEnum<>)
                    .MakeGenericType(objectType)
                    .GetMethod("Parse", BindingFlags.Public | BindingFlags.Static)
                    .Invoke(null, new object[] { s, false });
            ;
        }

        static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }
    }
}
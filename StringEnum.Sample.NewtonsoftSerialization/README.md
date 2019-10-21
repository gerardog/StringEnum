# StringEnum Newtonsoft.Json serialization

Json serialization was not built into the NuGet package to avoid a possibly unneeded dependency on `Newtonsoft.Json`.

To add `Json.Net` serialization support you need the `StringEnumJsonConverter` class.

This extended version file has everything in one: [StringEnum.cs](StringEnum.cs).

Or:

- Add [StringEnumJsonConverter](StringEnum.cs#59) class to your code.

    ``` csharp
    using Newtonsoft.Json;
    using System;
    using System.Reflection;

    public class StringEnumJsonConverter : JsonConverter
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
    ```

- Adding the following attribute to the `StringEnum` base class to support all instances, or add it to your specific StringEnums: 

``` csharp
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumJsonConverter))]
```

- You can also avoid the attributes and inject the `StringEnumJsonConverter` when you setup your (de)serialization:

``` csharp
    //serialization
    var obj = new { Color = Color.Red, MyString = "HelloWorld" };
    var jsonString = JsonConvert.SerializeObject(obj, new StringEnumConverter());

    //deserealization
    var settings = new JsonSerializerSettings();
    settings.Converters.Add(new StringEnumConverter()); 

    var result = JsonConvert.DeserializeAnonymousType(jsonString, obj, settings);
    Assert.AreEqual(Color.Red, result.Color);
```

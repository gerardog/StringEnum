# StringEnum Newtonsoft.Json serialization

Json serialization was not built into the NuGet package to avoid a probably unneeded dependency to `Newtonsoft.Json`.

To add `Json.Net` serialization support you need the `StringEnumJsonConverter` class.

- Add [StringEnumJsonConverter](StringEnum.cs#59) class to your code.

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

- Adding the following attribute to the `StringEnum` base class to support all instances, OR to all the StringEnums implementations. 

      [Newtonsoft.Json.JsonConverter(typeof(StringEnumJsonConverter))]

TL;DR; This extended version has everything in one. [StringEnum.cs](StringEnum.cs).

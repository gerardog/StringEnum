using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace StringEnum
{
    [JsonConverter(typeof(StringEnumJsonConverter))]
    public abstract class StringEnum<T> : IEquatable<T> where T : StringEnum<T>, new()
    {
        protected string Value;
        private static IList<T> valueList = new List<T>();
        protected static T New(string value)
        {
            if (value == null)
                return null; // the null-valued instance is null.

            var result = new T() { Value = value };
            valueList.Add(result);
            return result;
        }

        public static implicit operator string(StringEnum<T> enumValue) => enumValue.Value;
        public override string ToString() => Value;

        public static bool operator !=(StringEnum<T> o1, StringEnum<T> o2) => o1?.Value != o2?.Value;
        public static bool operator ==(StringEnum<T> o1, StringEnum<T> o2) => o1?.Value == o2?.Value;

        public override bool Equals(object other) => this.Value.Equals((other as T)?.Value ?? (other as string));
        bool IEquatable<T>.Equals(T other) => this.Value.Equals(other.Value);
        public override int GetHashCode() => Value.GetHashCode();

        public static T Parse(string name, bool caseSensitive = false)
        {
            var result = TryParse(name, caseSensitive);
            if (result == null)
                throw new InvalidOperationException((name == null ? "null" : $"'{name}'") + $" is not a valid {typeof(T).Name}");

            return result;
        }

        public static T TryParse(string name, bool caseSensitive = false)
        {
            if (name == null) return null;
            if (valueList.Count == 0) System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(T).TypeHandle); // force static fields initialization
            var field = valueList.FirstOrDefault(f => f.Value.Equals(name,
                    caseSensitive ? StringComparison.Ordinal 
                                  : StringComparison.OrdinalIgnoreCase));
            // Not using InvariantCulture because it's only supported in NETStandard >= 2.0

            if (field == null)
                return null;

            return field;
        }
    }

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
}

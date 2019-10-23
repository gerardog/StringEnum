using System;
using System.Collections.Generic;
using System.Linq;

namespace StringEnum
{
    // For Newtonsoft.Json support check /StringEnum.Sample.NewtonsoftSerialization/StringEnum.cs .</remarks>
    /// <summary>
    /// Base class for creating string-valued enums in .NET.<br/>
    /// Provides static Parse() and TryParse() methods and implicit cast to string.
    /// </summary>
    /// <example> 
    /// <code>
    /// class Color : StringEnum &lt;Color&gt;
    /// {
    ///     public static readonly Color Blue = Create("Blue");
    ///     public static readonly Color Red = Create("Red");
    ///     public static readonly Color Green = Create("Green");
    /// }
    /// </code>
    /// </example>
    /// <typeparam name="T">The string-valued enum type. (i.e. class Color : StringEnum&lt;Color&gt;)</typeparam>
    public abstract class StringEnum<T> : IEquatable<T> where T : StringEnum<T>, new()
    {
        protected string Value;
        private static Dictionary<string, T> valueDict = new Dictionary<string, T>();
        protected static T Create(string value)
        {
            if (value == null)
                return null; // the null-valued instance is null.

            var result = new T() { Value = value };
            valueDict.Add(value, result);
            return result;
        }

        public static implicit operator string(StringEnum<T> enumValue) => enumValue.Value;
        public override string ToString() => Value;

        public static bool operator !=(StringEnum<T> o1, StringEnum<T> o2) => o1?.Value != o2?.Value;
        public static bool operator ==(StringEnum<T> o1, StringEnum<T> o2) => o1?.Value == o2?.Value;

        public override bool Equals(object other) => this.Value.Equals((other as T)?.Value ?? (other as string));
        bool IEquatable<T>.Equals(T other) => this.Value.Equals(other.Value);
        public override int GetHashCode() => Value.GetHashCode();

        /// <summary>
        /// Parse the <paramref name="value"/> specified and returns a valid <typeparamref name="T"/> or else throws InvalidOperationException.
        /// </summary>
        /// <param name="value">The string value representad by an instance of <typeparamref name="T"/>. Matches by string value, not by the member name.</param>
        /// <param name="caseSensitive">If true, the strings must match case and takes O(log n). False allows different case but is little bit slower (O(n))</param>
        public static T Parse(string value, bool caseSensitive = true)
        {
            var result = TryParse(value, caseSensitive);
            if (result == null)
                throw new InvalidOperationException((value == null ? "null" : $"'{value}'") + $" is not a valid {typeof(T).Name}");

            return result;
        }

        /// <summary>
        /// Parse the <paramref name="value"/> specified and returns a valid <typeparamref name="T"/> or else returns null.
        /// </summary>
        /// <param name="value">The string value representad by an instance of <typeparamref name="T"/>. Matches by string value, not by the member name.</param>
        /// <param name="caseSensitive">If true, the strings must match case. False allows different case but is slower: O(n)</param>
        public static T TryParse(string value, bool caseSensitive = true)
        {
            if (value == null) return null;
            if (valueDict.Count == 0) System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(T).TypeHandle); // force static fields initialization
            if (caseSensitive)
            {
                if (valueDict.TryGetValue(value, out T item))
                    return item;
                else
                    return null;
            }
            else
            {
                // slower O(n) case insensitive search
                return valueDict.FirstOrDefault(f => f.Key.Equals(value, StringComparison.OrdinalIgnoreCase)).Value;
                // Why Ordinal? => https://esmithy.net/2007/10/15/why-stringcomparisonordinal-is-usually-the-right-choice/
            }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace StringEnum
{
    /// <summary>
    /// Base class for string-valued enums.<br/>Provides implicit cast to string and static Parse() and TryParse() methods.
    /// </summary>
    /// <example> 
    /// <code>
    /// class Color : StringEnum &lt;Color&gt;
    /// {
    ///     public static readonly Color Blue = New("Blue");
    ///     public static readonly Color Red = New("Red");
    ///     public static readonly Color Green = New("Green");
    /// }
    /// </code>
    /// </example>
    /// <typeparam name="T">The string-valued enum type. (i.e. class Color : StringEnum&lt;Color&gt;)</typeparam>
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

        /// <summary>
        /// Parse the <paramref name="value"/> specified and returns a valid <typeparamref name="T"/> or else throws InvalidOperationException.
        /// </summary>
        /// <param name="value">The string value representad by an instance of <typeparamref name="T"/>. Matches by string value, not by the member name.</param>
        /// <param name="caseSensitive">If true, the strings must match case sensitivity.</param>
        public static T Parse(string value, bool caseSensitive = false)
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
        /// <param name="caseSensitive">If true, the strings must match case sensitivity.</param>
        public static T TryParse(string value, bool caseSensitive = false)
        {
            if (value == null) return null;
            if (valueList.Count == 0) System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(T).TypeHandle); // force static fields initialization
            var field = valueList.FirstOrDefault(f => f.Value.Equals(value,
                    caseSensitive ? StringComparison.Ordinal 
                                  : StringComparison.OrdinalIgnoreCase));
            // Not using InvariantCulture because it's only supported in NETStandard >= 2.0

            if (field == null)
                return null;

            return field;
        }
    }
}
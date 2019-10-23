namespace StringEnum.Tests
{
    /// <summary>
    /// Color enum but with an implicit cast-from-string that invokes the Create() method, allowing any string to be casted as Color.
    /// </summary>
    /// <completionlist cref="ColorWithImplicitCreate"/>
    class ColorWithImplicitCreate : StringEnum<ColorWithImplicitCreate>
    {
        public static readonly ColorWithImplicitCreate Blue = Create("Blue");
        public static readonly ColorWithImplicitCreate Red = Create("Red");
        public static readonly ColorWithImplicitCreate Green = Create("Green");

        // Added implicit conversion from string
        public static implicit operator ColorWithImplicitCreate(string stringValue) 
            // Create new only if parsing fails, (i.e. it was not created with Create() before).
            => TryParse(stringValue, true) ?? Create(stringValue);
    }
}

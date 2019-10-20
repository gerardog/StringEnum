namespace StringEnum.Tests
{
    /// <summary>
    /// Color enum but with an implicit cast-from-string that invokes the New() method, allowing any string to be casted as Color.
    /// </summary>
    /// <completionlist cref="ColorWithImplicitNew"/>
    class ColorWithImplicitNew : StringEnum<ColorWithImplicitNew>
    {
        public static readonly ColorWithImplicitNew Blue = New("Blue");
        public static readonly ColorWithImplicitNew Red = New("Red");
        public static readonly ColorWithImplicitNew Green = New("Green");

        // Added implicit conversion from string
        public static implicit operator ColorWithImplicitNew(string stringValue) 
            // Create new only if parsing fails, (i.e. it was not created with New() before).
            => TryParse(stringValue, true) ?? New(stringValue);
    }
}

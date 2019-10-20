namespace StringEnum.Tests
{
    /// <completionlist cref="ImplicitParseColor"/>
    class ImplicitParseColor : StringEnum<ImplicitParseColor>
    {
        public static readonly ImplicitParseColor Blue = New("Blue");
        public static readonly ImplicitParseColor Red = New("Red");
        public static readonly ImplicitParseColor Green = New("Green");

        public static implicit operator ImplicitParseColor(string stringValue) => Parse(stringValue, caseSensitive: false);
    }
}

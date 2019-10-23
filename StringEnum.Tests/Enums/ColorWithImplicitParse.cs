namespace StringEnum.Tests
{
    /// <completionlist cref="ColorWithImplicitParse"/>
    class ColorWithImplicitParse : StringEnum<ColorWithImplicitParse>
    {
        public static readonly ColorWithImplicitParse Blue = Create(nameof(Blue));
        public static readonly ColorWithImplicitParse Red = Create(nameof(Red));
        public static readonly ColorWithImplicitParse Green = Create(nameof(Green));

        public static implicit operator ColorWithImplicitParse(string stringValue) => Parse(stringValue, caseSensitive: false);
    }
}

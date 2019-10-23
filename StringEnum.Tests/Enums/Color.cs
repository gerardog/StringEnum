namespace StringEnum.Tests
{
    ///<completionlist cref="Color"/> 
    class Color : StringEnum<Color>
    {
        public static readonly Color Blue = Create("Blue");
        public static readonly Color Red = Create("Red");
        public static readonly Color Green = Create("Green");
    }
}

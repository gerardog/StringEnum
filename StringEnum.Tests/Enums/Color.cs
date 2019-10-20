namespace StringEnum.Tests
{
    ///<completionlist cref="Color"/> 
    class Color : StringEnum<Color>
    {
        public static readonly Color Blue = New("Blue");
        public static readonly Color Red = New("Red");
        public static readonly Color Green = New("Green");
    }

    public class x
    {
        void bbb(Color xx) { }
        void aaaa(ColorWithImplicitNew myColor)
        {
            aaaa(ColorWithImplicitNew.Blue);

            //aaaa(Color.Blue);
            bbb(Color.Blue);
            bbb(Color.Red);
            //aaaa() 
        }
    }
}

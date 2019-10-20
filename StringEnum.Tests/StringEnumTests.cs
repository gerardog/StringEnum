using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace StringEnum.Tests
{
    [TestClass]
    public class StringEnumTests
    {
        [TestMethod]
        public void StringEnumComparisonTests()
        { 
            Color red1 = Color.Red;
            Color red2 = Color.Parse("Red");
            Color blue1 = Color.Blue;
            Color blue2 = Color.Parse("Blue");
            Color nullColor1 = (null as Color);
            Color nullColor2 = Color.TryParse(null as string);

            Assert.IsTrue(red1 == red2);
            Assert.IsFalse(red1 != red2);
            Assert.IsTrue(red1.Equals(red2));

            Assert.IsFalse(red1 == blue1);
            Assert.IsFalse(red1 == blue2);
            Assert.IsFalse(red2 == blue1);
            Assert.IsFalse(red2 == blue2);

            Assert.IsFalse(red1.Equals(blue1));

            Assert.IsFalse(red1 == null);
            Assert.IsFalse(null == red1);

            Assert.IsFalse(red1 == nullColor1);
            Assert.IsFalse(nullColor1 == red1);

            Assert.IsTrue(nullColor1 == nullColor2);
            Assert.IsFalse(nullColor1 != nullColor2);

            Assert.IsTrue(nullColor1 == null);
            Assert.IsTrue(null == nullColor1);
            Assert.IsTrue(nullColor2 == null);
            Assert.IsTrue(null == nullColor2);

            Assert.IsFalse(nullColor1 != null);
            Assert.IsFalse(null != nullColor1);
            Assert.IsFalse(nullColor2 != null);
            Assert.IsFalse(null != nullColor2);

            Assert.IsFalse(red1.Equals(nullColor2));

            Assert.AreEqual(Color.Red.GetHashCode(), "Red".GetHashCode());
            Assert.AreEqual(Color.Red.GetHashCode(), Color.Red.GetHashCode());
        }

        [TestMethod]
        public void StringComparisonTests()
        {
            // Case sensitive same value.
            Assert.IsTrue(ColorWithImplicitNew.Red == "Red");
            Assert.IsTrue("Red" == ColorWithImplicitNew.Red);
            Assert.IsTrue(ColorWithImplicitNew.Red.Equals("Red"));
            Assert.IsTrue(("Red").Equals(ColorWithImplicitNew.Red));
            Assert.IsFalse(ColorWithImplicitNew.Red != "Red");
            Assert.IsFalse("Red" != ColorWithImplicitNew.Red);

            // Case sensitive different case.
            Assert.IsFalse(ColorWithImplicitNew.Red == "red");
            Assert.IsFalse("red" == ColorWithImplicitNew.Red);
            Assert.IsFalse(ColorWithImplicitNew.Red.Equals("red"));
            Assert.IsFalse(("red").Equals(ColorWithImplicitNew.Red));
            Assert.IsTrue(ColorWithImplicitNew.Red != "red");
            Assert.IsTrue("red" != ColorWithImplicitNew.Red);

            // Case Insensitive same case/value.
            Assert.IsTrue(ColorWithImplicitNew.Red == "Red");
            Assert.IsTrue("Red" == ColorWithImplicitNew.Red);
            Assert.IsTrue(ColorWithImplicitNew.Red.Equals("Red"));
            Assert.IsTrue("Red".Equals(ColorWithImplicitNew.Red));
            Assert.IsFalse(ColorWithImplicitNew.Red != "Red");
            Assert.IsFalse("Red" != ColorWithImplicitNew.Red);

            // Case Insensitive different case.
            Assert.IsFalse(ImplicitParseColor.Red == "red");
            Assert.IsFalse("red" == ImplicitParseColor.Red);
            Assert.IsFalse(ImplicitParseColor.Red.Equals("red"));
            Assert.IsFalse("red".Equals(ImplicitParseColor.Red));
            Assert.IsTrue(ImplicitParseColor.Red != "red");
            Assert.IsTrue("red" != ImplicitParseColor.Red);

            // Case sensitive different values
            Assert.IsFalse(ColorWithImplicitNew.Red == "Blue");
            Assert.IsFalse("Blue" == ColorWithImplicitNew.Red);
            Assert.IsFalse(ColorWithImplicitNew.Red.Equals("Blue"));
            Assert.IsFalse("Blue".Equals(ColorWithImplicitNew.Red));
            Assert.IsTrue(ColorWithImplicitNew.Red != "Blue");
            Assert.IsTrue("Blue" != ColorWithImplicitNew.Red);

            // null value comparisons
            Assert.IsFalse(null == ColorWithImplicitNew.Red);
            Assert.IsFalse(ColorWithImplicitNew.Red == null);
            Assert.IsTrue(null != ColorWithImplicitNew.Red);
            Assert.IsTrue(ColorWithImplicitNew.Red != null);
            Assert.IsFalse(ColorWithImplicitNew.Red.Equals(null));
        }

        [TestMethod]
        public void UsageTests()
        {
            var r1 = PaintWithColor(Color.Red);
            var r2 = PaintWithColor(Color.Parse("Red"));

            Assert.AreEqual("The wall is now Red", r1);
            Assert.AreEqual("The wall is now Red", r2);

            Assert.AreEqual("The wall is now Red,Blue,Green,Red,Blue,Green",
                PaintStripe(Color.Red, Color.Blue, Color.Green, Color.Parse("Red"), Color.Parse("Blue"), Color.Parse("Green"))
                );
        }

        private string PaintWithColor(Color wallColor)
        {
            return "The wall is now " + wallColor;
        }

        private string PaintStripe(params Color[] colors)
        {
            return "The wall is now " + string.Join(",", colors.Select(c => c.ToString()).ToArray());
        }

        [TestMethod]
        public void ParseTest_OK()
        {
            Color a = Color.Parse("Red", true);
            Assert.AreEqual(a, Color.Red);

            ImplicitParseColor b = "Red";
            Assert.AreEqual(b, ImplicitParseColor.Red);
        }

        [TestMethod]
        public void ParseTest_CaseInsensitiveOK()
        {
            ImplicitParseColor a = ImplicitParseColor.Parse("red");
        }

        [TestMethod]
        public void ParseTest_ReferenceEquals()
        {
            var a = (ImplicitParseColor)"red";
            var b = ImplicitParseColor.Parse("red",false);
            var c = ImplicitParseColor.Parse("Red", true);
            var d = ImplicitParseColor.Red;

            Assert.IsTrue(object.ReferenceEquals(a, b));
            Assert.IsTrue(object.ReferenceEquals(a, c));
            Assert.IsTrue(object.ReferenceEquals(a, d));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ParseTest_CaseInsensitiveShouldFail()
        {
            ImplicitParseColor a = ImplicitParseColor.Parse("not a color");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ParseTest_CaseSensitiveShouldFail()
        {
            ColorWithImplicitNew  a = ColorWithImplicitNew.Parse("red", true);
        }

        [TestMethod]
        public void AnyString_OK()
        {
            ColorWithImplicitNew myColor;

            myColor = (ColorWithImplicitNew)"Violet";
            Assert.IsTrue("Violet" == myColor);
        }
    }
}

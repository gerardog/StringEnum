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
            Assert.IsTrue(Color.Red == "Red");
            Assert.IsTrue("Red" == Color.Red);
            Assert.IsTrue(Color.Red.Equals("Red"));
            Assert.IsTrue(("Red").Equals(Color.Red));
            Assert.IsFalse(Color.Red != "Red");
            Assert.IsFalse("Red" != Color.Red);

            // Case sensitive different case.
            Assert.IsFalse(Color.Red == "red");
            Assert.IsFalse("red" == Color.Red);
            Assert.IsFalse(Color.Red.Equals("red"));
            Assert.IsFalse(("red").Equals(Color.Red));
            Assert.IsTrue(Color.Red != "red");
            Assert.IsTrue("red" != Color.Red);

            // Case Insensitive same case/value.
            Assert.IsTrue(Color.Red == "Red");
            Assert.IsTrue("Red" == Color.Red);
            Assert.IsTrue(Color.Red.Equals("Red"));
            Assert.IsTrue("Red".Equals(Color.Red));
            Assert.IsFalse(Color.Red != "Red");
            Assert.IsFalse("Red" != Color.Red);

            // Case Insensitive different case.
            Assert.IsFalse(Color.Red == "red");
            Assert.IsFalse("red" == Color.Red);
            Assert.IsFalse(Color.Red.Equals("red"));
            Assert.IsFalse("red".Equals(Color.Red));
            Assert.IsTrue(Color.Red != "red");
            Assert.IsTrue("red" != Color.Red);

            // Case sensitive different values
            Assert.IsFalse(Color.Red == "Blue");
            Assert.IsFalse("Blue" == Color.Red);
            Assert.IsFalse(Color.Red.Equals("Blue"));
            Assert.IsFalse("Blue".Equals(Color.Red));
            Assert.IsTrue(Color.Red != "Blue");
            Assert.IsTrue("Blue" != Color.Red);

            // null value comparisons
            Assert.IsFalse(null == Color.Red);
            Assert.IsFalse(Color.Red == null);
            Assert.IsTrue(null != Color.Red);
            Assert.IsTrue(Color.Red != null);
            Assert.IsFalse(Color.Red.Equals(null));
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

            ColorWithImplicitParse b = "Red";
            Assert.AreEqual(b, ColorWithImplicitParse.Red);
        }

        [TestMethod]
        public void ParseTest_CaseInsensitiveOK()
        {
            Color a = Color.Parse("red", false);
            Assert.AreEqual(a, Color.Red);

            ColorWithImplicitParse b = "red";
            Assert.AreEqual(b, ColorWithImplicitParse.Red);
        }

        [TestMethod]
        public void ParseTest_ReferenceEquals()
        {
            var a = (ColorWithImplicitParse)"red";
            var b = ColorWithImplicitParse.Parse("red", false);
            var c = ColorWithImplicitParse.Parse("Red", true);
            var d = ColorWithImplicitParse.Red;

            Assert.IsTrue(object.ReferenceEquals(a, b));
            Assert.IsTrue(object.ReferenceEquals(a, c));
            Assert.IsTrue(object.ReferenceEquals(a, d));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ParseTest_CaseInsensitiveShouldFail()
        {
            ColorWithImplicitParse a = ColorWithImplicitParse.Parse("not a color");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ParseTest_CaseSensitiveShouldFail()
        {
            ColorWithImplicitCreate a = ColorWithImplicitCreate.Parse("red", true);
        }

        [TestMethod]
        public void AnyString_OK()
        {
            ColorWithImplicitCreate myColor = "Red";
            Assert.IsTrue("Red" == myColor);
            Assert.IsTrue("Red".Equals(myColor));

            myColor = (ColorWithImplicitCreate)"KindaViolet";
            Assert.IsTrue("KindaViolet" == myColor);
        }
    }
}

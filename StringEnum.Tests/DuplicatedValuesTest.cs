using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace StringEnum.Tests
{
    /// <summary>
    /// This class represents the common copy/paste typo when 2 items have the same value.
    /// </summary>
    class InvalidEnum : StringEnum<InvalidEnum>
    {
        public static readonly InvalidEnum ItemOne = Create("ItemOne");
        public static readonly InvalidEnum ItemTwo = Create("ItemOne"); // <-- Intended duplicated value
    }

    /// <summary>
    /// What if it wasn't a typo and you need two enums with the same string. Create() the first one, Parse() the rest.
    /// </summary>
    class InvalidEnumWorkaround : StringEnum<InvalidEnum>
    {
        public static readonly InvalidEnum ItemOne = Create("ItemOne");
        public static readonly InvalidEnum ItemTwo = Parse("ItemOne"); // <-- Allowed duplicated value via workaround.
        // In this case, order matters. Put the Create call before the Parse.
    }

    [TestClass]
    public class DuplicatedValuesTest
    {
        [TestMethod]
        [ExpectedException(typeof(TypeInitializationException))]
        public void InvalidEnum_ShouldFail()
        {
            var item1 = InvalidEnum.ItemOne; // When you try to call the enum, it throws.
        }

        [TestMethod]
        public void InvalidEnumWorkAround_Ok()
        {
            var item1 = InvalidEnumWorkaround.ItemOne;
            var item2 = InvalidEnumWorkaround.ItemTwo;
            var item3 = InvalidEnumWorkaround.Parse("ItemOne");

            Assert.IsTrue(item1 == item2);
            Assert.IsTrue(item1 == item3);
            Assert.IsTrue(item1.Equals(item2));
            Assert.IsTrue(item1.Equals(item3));
            Assert.IsTrue(object.ReferenceEquals(item1, item2));
            Assert.IsTrue(object.ReferenceEquals(item1, item3));
        }

    }
}

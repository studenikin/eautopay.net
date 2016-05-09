using Microsoft.VisualStudio.TestTools.UnitTesting;

using EAutopay.Upsells;

namespace EAutopay.Tests
{
    [TestClass]
    public class UpsellTest
    {
        [TestMethod]
        public void IsNew_Should_Be_True_For_Empty_Upsell()
        {
            var u = new Upsell();
            Assert.IsTrue(u.IsNew);
        }

        [TestMethod]
        public void ID_Should_Be_Zero_For_Empty_Upsell()
        {
            var u = new Upsell();
            Assert.AreEqual(0, u.ID);
        }

        [TestMethod]
        public void Price_Should_Be_Zero_For_Empty_Upsell()
        {
            var u = new Upsell();
            Assert.AreEqual(0, u.Price);
        }

        [TestMethod]
        public void PriceInvariant_Should_Have_Dotted_Delimiter()
        {
            var u = new Upsell();
            u.Price = 199.99;
            Assert.AreEqual("199.99", u.PriceInvariant);
        }

        [TestMethod]
        public void New_Upsell_Should_Have_Zero_OriginID()
        {
            var u = new Upsell();
            Assert.AreEqual(0, u.OriginID);
        }

        [TestMethod]
        public void New_Upsell_Should_Have_Zero_ParentID()
        {
            var u = new Upsell();
            Assert.AreEqual(0, u.ParentID);
        }
    }
}

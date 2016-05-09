using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using EAutopay.Products;

namespace EAutopay.Tests
{
    [TestClass]
    public class ProductTest
    {
        [TestMethod]
        public void IsNew_Should_Be_True_For_Empty_Product()
        {
            var p = new Product();
            Assert.IsTrue(p.IsNew);
        }

        [TestMethod]
        public void ID_Should_Be_Zero_For_Empty_Product()
        {
            var p = new Product();
            Assert.AreEqual(0, p.ID);
        }

        [TestMethod]
        public void Price_Should_Be_Zero_For_Empty_Product()
        {
            var p = new Product();
            Assert.AreEqual(0, p.Price);
        }

        [TestMethod]
        public void IsUpsell_Should_True_If_Has_UPSELL_Word()
        {
            var p = new Product();
            p.Name = "Product1 (Upsell)";
            Assert.IsTrue(p.IsUpsell);
        }

        [TestMethod]
        public void IsUpsell_Should_False_If_No_UPSELL_Word()
        {
            var p = new Product();
            p.Name = "Product1";
            Assert.IsFalse(p.IsUpsell);
        }

        [TestMethod]
        public void PriceFormatted_Should_Have_Currency_Symbol()
        {
            var p = new Product();
            p.Price = 199.99;
            string currency = CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol;
            Assert.IsTrue(p.PriceFormatted.Contains(currency));
        }

        [TestMethod]
        public void PriceInvariant_Should_Have_Dotted_Delimiter()
        {
            var p = new Product();
            p.Price = 199.99;
            Assert.AreEqual("199.99", p.PriceInvariant);
        }
    }
}

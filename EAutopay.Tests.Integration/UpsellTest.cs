using System;
using System.IO;
using System.Web;
using System.Net;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using EAutopay;

using HtmlAgilityPack;


namespace EAutopay.Tests.Integration
{
    
    [TestClass]
    public class UpsellTest
    {
        private Product _product;

        [TestInitialize]
        public void SetUp()
        {
            HttpContext.Current = Common.GetHttpContext();
            Common.Login();

            _product = Common.CreateTestProduct();
        }

        [TestCleanup]
        public void TearDown()
        {
            _product.Delete();

            Common.Logout();
            HttpContext.Current = null;
        }

        [TestMethod]
        public void Upsell_Add()
        {
            var upsell = CreateUpsell(_product);

            Check_UpsellHasBeenCreated(upsell);
            Check_UpsellHasBeenEnabled(_product);
            Check_UpsellReferenceHasBeenCreated(_product, upsell);
        }

        [TestMethod]
        public void Upsell_Remove()
        {
            var upsell = CreateUpsell(_product);
            upsell.Delete();

            Check_UpsellHasBeenRemoved(upsell);
            Check_UpsellHasBeenDisabled(_product);
            Check_UpsellReferenceHasBeenRemoved(_product, upsell);
        }

        [TestMethod]
        public void Upsell_IsRemovedAfterMainProductRemoval()
        {
            var upsell = CreateUpsell(_product);

            _product.Delete();

            Assert.IsNull(ProductFactory.Get(upsell.ID));
        }

        private Product CreateUpsell(Product mainProduct)
        {
            if (_product.ID == 0)
            {
                _product = Common.CreateTestProduct();
            }
            double newPrice = _product.Price / 2;
            return mainProduct.AddUpsell(newPrice);
        }

        private void Check_UpsellHasBeenCreated(Product upsell)
        {
            var p = ProductFactory.Get(upsell.ID);
            Assert.IsNotNull(p);
            Assert.AreEqual(upsell.Name, p.Name);
            Assert.AreEqual(upsell.PriceInvariant, p.PriceInvariant);
        }

        private void Check_UpsellHasBeenRemoved(Product upsell)
        {
            var p = ProductFactory.Get(upsell.ID);
            Assert.IsNull(p);
        }

        private void Check_UpsellHasBeenEnabled(Product product)
        {
            var settings = UpsellSettingsRepository.Get(product.ID);

            Assert.IsTrue(settings.IsUpsellsEnabled);
            Assert.AreEqual(Config.UPSELL_INTERVAL, settings.Interval);
            Assert.AreEqual(Config.GetUpsellPageURI(), settings.RedirectUri);
        }

        private void Check_UpsellHasBeenDisabled(Product product)
        {
            var settings = UpsellSettingsRepository.Get(product.ID);
            Assert.IsFalse(settings.IsUpsellsEnabled);
        }

        private void Check_UpsellReferenceHasBeenCreated(Product product, Product upsell)
        {
            var settings = UpsellSettingsRepository.Get(product.ID);
            Assert.IsTrue(settings.HasProductUpsells);
        }

        private void Check_UpsellReferenceHasBeenRemoved(Product product, Product upsell)
        {
            var settings = UpsellSettingsRepository.Get(product.ID);
            Assert.IsFalse(settings.HasProductUpsells);
        }
    }
}

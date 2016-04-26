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
            var crawler = new Crawler();
            using (var resp = crawler.HttpGet(Config.GetSendSettingsURI(product.ID)))
            {
                var reader = new StreamReader(resp.GetResponseStream());

                var html = new HtmlDocument();
                html.LoadHtml(reader.ReadToEnd());
                var root = html.DocumentNode;

                // Check that the "Allow upsells" checkbox has been enabled.
                var checkbox = root.SelectSingleNode("//input[@name='additional_tovar_offer'][@type='checkbox']");
                Assert.IsNotNull(checkbox);
                Assert.AreEqual(true, checkbox.Attributes["checked"] != null);
                Assert.AreEqual("checked", checkbox.Attributes["checked"].Value);

                // Check that the "Interval" textbox has correct value.
                var interval = root.SelectSingleNode("//input[@name='time_for_add']");
                Assert.IsNotNull(interval);
                Assert.AreEqual(Config.UPSELL_INTERVAL, interval.Attributes["value"].Value);

                // Check that the "Upsell page" textbox has correct value.
                var page = root.SelectSingleNode("//input[@name='additional_tovar_page_offer']");
                Assert.IsNotNull(page);
                Assert.AreEqual(Config.GetUpsellPageURI(), page.Attributes["value"].Value);
            }
        }

        private void Check_UpsellHasBeenDisabled(Product product)
        {
            var crawler = new Crawler();
            using (var resp = crawler.HttpGet(Config.GetSendSettingsURI(product.ID)))
            {
                var reader = new StreamReader(resp.GetResponseStream());

                var html = new HtmlDocument();
                html.LoadHtml(reader.ReadToEnd());
                var root = html.DocumentNode;

                // Check that the "Allow upsells" checkbox has been disabled.
                var checkbox = root.SelectSingleNode("//input[@name='additional_tovar_offer'][@type='checkbox']");
                Assert.IsNotNull(checkbox);
                Assert.IsNull(checkbox.Attributes["checked"]);
            }
        }

        private void Check_UpsellReferenceHasBeenCreated(Product product, Product upsell)
        {
            var crawler = new Crawler();
            using (var resp = crawler.HttpGet(Config.GetSendSettingsURI(product.ID)))
            {
                var reader = new StreamReader(resp.GetResponseStream());
                string html = reader.ReadToEnd();

                // Check if source contains upsell link. That means the upsell is bouned to the product.
                Assert.IsTrue(html.IndexOf("&tovar_id="+ upsell.ID) > -1);
            }
        }

        private void Check_UpsellReferenceHasBeenRemoved(Product product, Product upsell)
        {
            var crawler = new Crawler();
            using (var resp = crawler.HttpGet(Config.GetSendSettingsURI(product.ID)))
            {
                var reader = new StreamReader(resp.GetResponseStream());
                string html = reader.ReadToEnd();

                // Check if source doesn't contain upsell link.
                Assert.IsTrue(html.IndexOf("&tovar_id=" + upsell.ID) == -1);
            }
        }
    }
}

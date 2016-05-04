using System.Web;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using EAutopay.Products;

namespace EAutopay.Tests.Integration
{
    
    [TestClass]
    public class UpsellTest
    {
        Product _product;

        readonly ProductService _prodService = new ProductService();

        readonly IProductRepository _repository = new EAutopayProductRepository();

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

            Assert.IsNull(_repository.Get(upsell.ID));
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
            var p = _repository.Get(upsell.ID);
            Assert.IsNotNull(p);
            Assert.AreEqual(upsell.Name, p.Name);
            Assert.AreEqual(upsell.PriceInvariant, p.PriceInvariant);
        }

        private void Check_UpsellHasBeenRemoved(Product upsell)
        {
            var p = _repository.Get(upsell.ID);
            Assert.IsNull(p);
        }

        private void Check_UpsellHasBeenEnabled(Product product)
        {
            var settings = _prodService.GetUpsellSettings(product.ID);

            Assert.IsTrue(settings.IsUpsellsEnabled);
            Assert.AreEqual(GetInterval(), settings.Interval);
            Assert.AreEqual(GetLanding(), settings.RedirectUri);
        }

        private void Check_UpsellHasBeenDisabled(Product product)
        {
            var settings = _prodService.GetUpsellSettings(product.ID);
            Assert.IsFalse(settings.IsUpsellsEnabled);
        }

        private void Check_UpsellReferenceHasBeenCreated(Product product, Product upsell)
        {
            var settings = _prodService.GetUpsellSettings(product.ID);
            Assert.IsTrue(settings.HasProductUpsells);
        }

        private void Check_UpsellReferenceHasBeenRemoved(Product product, Product upsell)
        {
            var settings = _prodService.GetUpsellSettings(product.ID);
            Assert.IsFalse(settings.HasProductUpsells);
        }

        private int GetInterval()
        {
            int ret;
            int.TryParse(ConfigurationManager.AppSettings["eautopay_upsell_interval"], out ret);
            return ret > 0 ? ret : 20;
        }

        private string GetLanding()
        {
            return ConfigurationManager.AppSettings["eautopay_upsell_landing"] ?? "";
        }
    }
}

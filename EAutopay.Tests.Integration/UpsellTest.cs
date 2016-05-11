using System.Web;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using EAutopay.Products;
using EAutopay.Upsells;

namespace EAutopay.Tests.Integration
{
    
    [TestClass]
    public class UpsellTest
    {
        const string SUCCESS_PAGE_URI = "http://domain.com/success";

        ProductService _prodService;

        IUpsellRepository _upsellRepo;

        [TestInitialize]
        public void SetUp()
        {
            HttpContext.Current = Common.GetHttpContext();
            Common.Login();
            _prodService = new ProductService();
            _upsellRepo = new EAutopayUpsellRepository();
        }

        [TestCleanup]
        public void TearDown()
        {
            Common.RemoveAllTestProducts();
            Common.Logout();
            HttpContext.Current = null;
            _prodService = null;
            _upsellRepo = null;
        }

        [TestMethod]
        public void Upsell_Is_Saved_Correctly()
        {
            var p = Common.CreateTestProduct();
            var u = CreateAndSaveUpsell(p);

            var u2 = _upsellRepo.Get(u.ID, u.ParentID);

            Assert.IsNotNull(u2);
            Assert.IsTrue(u2.ID > 0);
            Assert.IsTrue(u2.OriginID > 0);
            Assert.AreEqual(u2.ParentID, p.ID);
            Assert.AreEqual(u2.SuccessUri, SUCCESS_PAGE_URI);
            Assert.AreEqual(u2.PriceInvariant, u.PriceInvariant);
            Assert.IsTrue(u2.Title.ToUpper().Contains("UPSELL"));
        }

        [TestMethod]
        public void Upsell_Is_Updated_Correctly()
        {
            var p = Common.CreateTestProduct();
            var u = CreateAndSaveUpsell(p);

            var p2 = Common.CreateTestProduct();
            var u2 = _upsellRepo.Get(u.ID, u.ParentID);
            int id = u2.ID;
            string title = u2.Title;

            u2.OriginID = p2.ID;
            u2.Price = 1234.00;
            u2.SuccessUri = "http://some_uri.com";
            _upsellRepo.Save(u2, p.ID);
            
            Assert.IsNotNull(u2);
            Assert.AreEqual(u2.ID, id);
            Assert.IsTrue(u2.ID > 0);
            Assert.AreEqual(u2.OriginID, p2.ID);
            Assert.AreEqual(u2.ParentID, p.ID);
            Assert.AreEqual(u2.SuccessUri, "http://some_uri.com");
            Assert.AreEqual(u2.Price, 1234.00);
            Assert.AreEqual(u2.Title, title);
        }

        [TestMethod]
        public void Upsell_Should_Not_Exist_After_Removal()
        {
            var p = Common.CreateTestProduct();
            var u = CreateAndSaveUpsell(p);

            int id = u.ID;
            _upsellRepo.Delete(u);

            var u2 = _upsellRepo.Get(id, p.ID);
            Assert.IsNull(u2);
        }

        [TestMethod]
        public void Upsell_Empty_Product_Has_No_Upsells()
        {
            var p = Common.CreateTestProduct();
            Assert.IsFalse(_upsellRepo.HasUpsell(p.ID));
        }

        [TestMethod]
        public void Upsell_Should_Product_Have_Upsell_After_Adding()
        {
            var p = Common.CreateTestProduct();
            var upsell = CreateAndSaveUpsell(p);

            Assert.IsTrue(_upsellRepo.HasUpsell(p.ID));
        }

        [TestMethod]
        public void Upsell_Is_Upsell_Amount_Increased_After_Saving()
        {
            var p = Common.CreateTestProduct();

            int count = _upsellRepo.GetByProduct(p.ID).Count;
            Assert.AreEqual(0, count);

            var upsell = CreateAndSaveUpsell(p);

            count = _upsellRepo.GetByProduct(p.ID).Count;
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void Upsell_Is_Upsell_Amount_Decreased_After_Removal()
        {
            var p = Common.CreateTestProduct();
            var upsell = CreateAndSaveUpsell(p);

            _upsellRepo.Delete(upsell);

            int count = _upsellRepo.GetByProduct(p.ID).Count;
            Assert.AreEqual(0, count);
        }

        [TestMethod]
        public void Upsell_Add_2_Upsells_Then_Remove_Should_Product_Have_No_Upsells()
        {
            var p = Common.CreateTestProduct();
            var upsell = CreateAndSaveUpsell(p);
            var upsell2 = CreateAndSaveUpsell(p);

            _upsellRepo.Delete(upsell);

            Assert.IsTrue(_upsellRepo.HasUpsell(p.ID));

            _upsellRepo.Delete(upsell2);

            Assert.IsFalse(_upsellRepo.HasUpsell(p.ID));
        }

        [TestMethod]
        public void Upsell_Delete_By_Product_Should_Remove_All_Upsells()
        {
            var p = Common.CreateTestProduct();
            var upsell = CreateAndSaveUpsell(p);
            var upsell2 = CreateAndSaveUpsell(p);

            _upsellRepo.DeleteByProduct(p.ID);

            Assert.IsFalse(_upsellRepo.HasUpsell(p.ID));
        }

        [TestMethod]
        public void Upsell_Add_2_Upsells_Then_Remove_Should_Disable_Checkbox()
        {
            var p = Common.CreateTestProduct();

            var upSettings = _prodService.GetUpsellSettings(p.ID);
            Assert.IsFalse(upSettings.IsUpsellsEnabled);

            var upsell = CreateAndSaveUpsell(p);

            upSettings = _prodService.GetUpsellSettings(p.ID);
            Assert.IsTrue(upSettings.IsUpsellsEnabled);

            var upsell2 = CreateAndSaveUpsell(p);

            upSettings = _prodService.GetUpsellSettings(p.ID);
            Assert.IsTrue(upSettings.IsUpsellsEnabled);

            _upsellRepo.Delete(upsell);

            upSettings = _prodService.GetUpsellSettings(p.ID);
            Assert.IsTrue(upSettings.IsUpsellsEnabled);

            _upsellRepo.Delete(upsell2);

            upSettings = _prodService.GetUpsellSettings(p.ID);
            Assert.IsFalse(upSettings.IsUpsellsEnabled);
        }

        [TestMethod]
        public void Upsell_Get_Upsells_By_Product()
        {
            var p = Common.CreateTestProduct();
            var u = CreateAndSaveUpsell(p);

            int count = _upsellRepo.GetByProduct(p.ID).Count;
            Assert.AreEqual(1, count);

            var u2 = CreateAndSaveUpsell(p);
            count = _upsellRepo.GetByProduct(p.ID).Count;

            Assert.AreEqual(2, count);
        }

        [TestMethod]
        public void Upsell_Are_Upsell_Settings_OK_After_Adding()
        {
            var p = Common.CreateTestProduct();
            var u = CreateAndSaveUpsell(p);

            var settings = _prodService.GetUpsellSettings(p.ID);

            Assert.IsTrue(settings.IsUpsellsEnabled);
            Assert.IsTrue(settings.HasProductUpsells);
            Assert.AreEqual(GetInterval(), settings.Interval);
            Assert.AreEqual(GetLanding(), settings.RedirectUri);
        }

        [TestMethod]
        public void Upsell_Are_Upsell_Settings_OK_After_Removal()
        {
            var p = Common.CreateTestProduct();
            var u = CreateAndSaveUpsell(p);

            _upsellRepo.Delete(u);

            var settings = _prodService.GetUpsellSettings(p.ID);

            Assert.IsFalse(settings.IsUpsellsEnabled);
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

        private Upsell CreateAndSaveUpsell(Product mainProduct)
        {
            var upsellProduct = Common.CreateTestProduct(mainProduct.Name, true);

            var upsell = new Upsell();
            upsell.OriginID = upsellProduct.ID;
            upsell.Price = 199.00;
            upsell.SuccessUri = SUCCESS_PAGE_URI;

            _upsellRepo.Save(upsell, mainProduct.ID);
            return upsell;
        }
    }
}

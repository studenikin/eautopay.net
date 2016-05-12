using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EAutopay.Tests
{
    [TestClass]
    public class UriProviderTest
    {
        [TestMethod]
        public void LogingUri_Returns_Correct_URI()
        {
            var up = new UriProvider("login");
            Assert.AreEqual(Common.LOGIN_URI, up.LoginUri);
        }

        [TestMethod]
        public void LogoutUri_Returns_Correct_URI()
        {
            var up = new UriProvider("login");
            Assert.AreEqual(Common.LOGOUT_URI, up.LogoutUri);
        }

        [TestMethod]
        public void SecretUri_Returns_Correct_URI()
        {
            var up = new UriProvider("login");
            Assert.AreEqual(Common.SECRET_URI, up.SecretUri);
        }

        [TestMethod]
        public void MainUri_Returns_Correct_URI()
        {
            var up = new UriProvider("login");
            Assert.AreEqual(Common.MAIN_URI, up.MainUri);
        }

        [TestMethod]
        public void ProductListUri_Returns_Correct_URI()
        {
            var up = new UriProvider("login");
            Assert.AreEqual(Common.PRODUCTS_URI, up.ProductListUri);
        }

        [TestMethod]
        public void ProductSaveUri_Returns_Correct_URI()
        {
            var up = new UriProvider("login");
            Assert.AreEqual(Common.PRODUCT_SAVE_URI, up.ProductSaveUri);
        }

        [TestMethod]
        public void ProductDeleteUri_Returns_Correct_URI()
        {
            var up = new UriProvider("login");
            Assert.AreEqual(Common.PRODUCT_DELETE_URI, up.ProductDeleteUri);
        }

        [TestMethod]
        public void FormListUri_Returns_Correct_URI()
        {
            var up = new UriProvider("login");
            Assert.AreEqual(Common.FORMS_URI, up.FormListUri);
        }

        [TestMethod]
        public void FormSaveUri_Returns_Correct_URI()
        {
            var up = new UriProvider("login");
            Assert.AreEqual(Common.FORM_SAVE_URI, up.FormSaveUri);
        }

        [TestMethod]
        public void FormDeleteUri_Returns_Correct_URI()
        {
            var up = new UriProvider("login");
            Assert.AreEqual(Common.FORM_DELETE_URI, up.FormDeleteUri);
        }

        [TestMethod]
        public void GetUpsellUri_Returns_Correct_URI()
        {
            var up = new UriProvider("login");
            var uri = "https://login.e-autopay.com/adminka/product/1/upsell/2";
            Assert.AreEqual(uri, up.GetUpsellUri(1, 2));
        }

        [TestMethod]
        public void GetUpsellUri_Returns_Shorten_URI_If_No_UpsellID()
        {
            var up = new UriProvider("login");
            var uri = "https://login.e-autopay.com/adminka/product/1/upsell/";
            Assert.AreEqual(uri, up.GetUpsellUri(1, 0));
        }

        [TestMethod]
        public void GetSendSettingsUri_Returns_Correct_URI()
        {
            var up = new UriProvider("login");
            var uri = "https://login.e-autopay.com/adminka/product/edit/1/sendsettings";
            Assert.AreEqual(uri, up.GetSendSettingsUri(1));
        }
    }
}

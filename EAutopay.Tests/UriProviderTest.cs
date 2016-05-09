using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EAutopay.Tests
{
    [TestClass]
    public class UriProviderTest
    {
        [TestMethod]
        public void LogingUri_Returns_Correct_URI()
        {
            var up = new UriProvider("test_login");
            var uri = "https://test_login.e-autopay.com/adminka/login";
            Assert.AreEqual(uri, up.LoginUri);
        }

        [TestMethod]
        public void LogoutUri_Returns_Correct_URI()
        {
            var up = new UriProvider("test_login");
            var uri = "https://test_login.e-autopay.com/adminka/logout";
            Assert.AreEqual(uri, up.LogoutUri);
        }

        [TestMethod]
        public void SecretUri_Returns_Correct_URI()
        {
            var up = new UriProvider("test_login");
            var uri = "https://test_login.e-autopay.com/adminka/identify";
            Assert.AreEqual(uri, up.SecretUri);
        }

        [TestMethod]
        public void MainUri_Returns_Correct_URI()
        {
            var up = new UriProvider("test_login");
            var uri = "https://test_login.e-autopay.com/adminka/main.php";
            Assert.AreEqual(uri, up.MainUri);
        }

        [TestMethod]
        public void ProductListUri_Returns_Correct_URI()
        {
            var up = new UriProvider("test_login");
            var uri = "https://test_login.e-autopay.com/adminka/tovars/list_tovars.php";
            Assert.AreEqual(uri, up.ProductListUri);
        }

        [TestMethod]
        public void ProductSaveUri_Returns_Correct_URI()
        {
            var up = new UriProvider("test_login");
            var uri = "https://test_login.e-autopay.com/adminka/product/save";
            Assert.AreEqual(uri, up.ProductSaveUri);
        }

        [TestMethod]
        public void ProductDeleteUri_Returns_Correct_URI()
        {
            var up = new UriProvider("test_login");
            var uri = "https://test_login.e-autopay.com/adminka/tovars/del_tovar.php";
            Assert.AreEqual(uri, up.ProductDeleteUri);
        }

        [TestMethod]
        public void FormListUri_Returns_Correct_URI()
        {
            var up = new UriProvider("test_login");
            var uri = "https://test_login.e-autopay.com/adminka/form_generator/index.php";
            Assert.AreEqual(uri, up.FormListUri);
        }

        [TestMethod]
        public void FormSaveUri_Returns_Correct_URI()
        {
            var up = new UriProvider("test_login");
            var uri = "https://test_login.e-autopay.com/adminka/form_generator/save_form.php";
            Assert.AreEqual(uri, up.FormSaveUri);
        }

        [TestMethod]
        public void FormDeleteUri_Returns_Correct_URI()
        {
            var up = new UriProvider("test_login");
            var uri = "https://test_login.e-autopay.com/adminka/form_generator/delete_form.php";
            Assert.AreEqual(uri, up.FormDeleteUri);
        }

        [TestMethod]
        public void GetUpsellUri_Returns_Correct_URI()
        {
            var up = new UriProvider("test_login");
            var uri = "https://test_login.e-autopay.com/adminka/product/1/upsell/2";
            Assert.AreEqual(uri, up.GetUpsellUri(1, 2));
        }

        [TestMethod]
        public void GetUpsellUri_Returns_Shorten_URI_If_No_UpsellID()
        {
            var up = new UriProvider("test_login");
            var uri = "https://test_login.e-autopay.com/adminka/product/1/upsell/";
            Assert.AreEqual(uri, up.GetUpsellUri(1, 0));
        }

        [TestMethod]
        public void GetSendSettingsUri_Returns_Correct_URI()
        {
            var up = new UriProvider("test_login");
            var uri = "https://test_login.e-autopay.com/adminka/product/edit/1/sendsettings";
            Assert.AreEqual(uri, up.GetSendSettingsUri(1));
        }
    }
}

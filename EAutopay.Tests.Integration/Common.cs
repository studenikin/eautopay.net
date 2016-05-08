using System;
using System.Web;
using System.Linq;
using System.Configuration;

using EAutopay.Forms;
using EAutopay.Products;
using EAutopay.Security;

namespace EAutopay.Tests.Integration
{
    class Common
    {
        internal const string TEST_PRODUCT_NAME = "#_TEST_PRODUCT_#";
        internal const string TEST_FORM_NAME = "#_TEST_FORM_#";
        internal const string TEST_UPSELL_NAME = "#_TEST_UPSELL_#";

        public static HttpContext GetHttpContext()
        {
            return new HttpContext(
                new HttpRequest(null, "http://tempuri.org", null),
                new HttpResponse(null));
        }

        public static AuthResult LoginAndGetResult() 
        {
            var settings = ConfigurationManager.AppSettings;
            var auth = new Auth(settings["eautopay_login"], settings["eautopay_password"], settings["eautopay_secret"]);
            return auth.Login();
        }

        public static void Login()
        {
            var authResult = LoginAndGetResult();
            if (authResult.Status != AuthStatus.Ok)
            {
                throw new Exception("Cannot login! Login status is: " + authResult.Status);
            }
        }

        public static void Logout()
        {
            new Auth().Logout();
        }

        public static Product CreateTestProduct()
        {
            return CreateTestProduct(TEST_PRODUCT_NAME, false);
        }

        public static Product CreateTestProduct(string name, bool isForUpsell)
        {
            var p = new Product();
            p.Name = name;
            p.Price = 999.99;

            var repo = new EAutopayProductRepository();
            repo.Save(p, isForUpsell);
            return p;
        }

        public static Form CreateTestForm()
        {
            var f = new Form();
            f.Name = TEST_FORM_NAME;

            var repo = new EAutopayFormRepository();
            repo.Save(f);
            return f;
        }

        public static void RemoveAllTestForms()
        {
            var repo = new EAutopayFormRepository();
            var forms = repo.GetAll();
            foreach (var form in repo.GetAll().Where(f => f.Name == TEST_FORM_NAME))
            {
                repo.Delete(form);
            }
        }

        public static void RemoveAllTestProducts()
        {
            var repo = new EAutopayProductRepository();
            var products = repo.GetAll();
            foreach (var product in products.Where(p => p.Name == TEST_PRODUCT_NAME || p.Name == TEST_UPSELL_NAME))
            {
                repo.Delete(product);
            }
        }
    }
}

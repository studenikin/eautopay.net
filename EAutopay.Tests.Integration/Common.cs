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
            if (authResult.Status != AuthResult.Statuses.Ok)
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
            var p = new Product();
            p.Name = TEST_PRODUCT_NAME;
            p.Price = 999.99;
            p.Save();
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
    }
}

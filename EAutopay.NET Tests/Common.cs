using System;
using System.IO;
using System.Web;
using System.Linq;
using System.Web.SessionState;
using System.Reflection;
using System.Configuration;

using EAutopay.NET;


namespace EAutopay.NET_Tests
{
    class Common
    {
        public const string TEST_PRODUCT_NAME = "#_TEST_PRODUCT_#";
        public const string TEST_FORM_NAME = "#_TEST_FORM_#";

        public static HttpContext GetHttpContext()
        {
            return new HttpContext(
                new HttpRequest(null, "http://tempuri.org", null),
                new HttpResponse(null));
        }

        public static AuthResult Login() 
        {
            var settings = ConfigurationManager.AppSettings;
            var auth = new Auth(settings["login"], settings["password"], settings["secret"]);
            return auth.Login();
        }

        public static void Logout()
        {
            Auth.Logout();
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
            f.Save();
            return f;
        }

        public static void RemoveAllTestForms()
        {
            var forms = FormRepository.GetAll();
            foreach (var form in forms.Where(f => f.Name == TEST_FORM_NAME))
            {
                form.Delete();
            }
        }
    }
}

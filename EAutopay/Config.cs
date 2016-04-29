namespace EAutopay
{
    public class Config
    {
        public const string URI_SAVE_PRODUCT = "https://demo4.e-autopay.com/adminka/product/save";
        public const string URI_DELETE_PRODUCT = "https://demo4.e-autopay.com/adminka/tovars/del_tovar.php";
        public const string URI_LOGIN = "https://demo4.e-autopay.com/adminka/login";
        public const string URI_LOGOUT = "https://demo4.e-autopay.com/adminka/logout";
        public const string URI_SECRET = "https://demo4.e-autopay.com/adminka/identify";
        public const string URI_PRODUCT_LIST = "https://demo4.e-autopay.com/adminka/tovars/list_tovars.php";
        public const string URI_MAIN = "https://demo4.e-autopay.com/adminka/main.php";
        public const string URI_FORM_LIST = "https://demo4.e-autopay.com/adminka/form_generator/index.php";
        public const string URI_FORM_SAVE = "https://demo4.e-autopay.com/adminka/form_generator/save_form.php";
        public const string URI_FORM_DELETE = "https://demo4.e-autopay.com/adminka/form_generator/delete_form.php ";

        public const string UPSELL_SUFFIX = "Upsell";
        public const int UPSELL_INTERVAL = 20;

        public static string GetUpsellURI(int productId)
        {
             return string.Format("https://demo4.e-autopay.com/adminka/product/{0}/upsell", productId);
        }

        public static string GetUpsellReferenceURI(int productId, int upsellId)
        {
            return string.Format("https://demo4.e-autopay.com/adminka/product/{0}/upsell/{1}", productId, upsellId);
        }

        // Is it used somewhere except tests?
        public static string GetSendSettingsURI(int productId)
        {
            return string.Format("https://demo4.e-autopay.com/adminka/product/edit/{0}/sendsettings", productId);
        }

        public static string GetUpsellPageURI()
        {
            return "http://ceoffer.ru/demo/upsell.php";
        }

        public static string GetUpsellSuccessPage()
        {
            return "http://ceoffer.ru/demo/1.html";
        }

    }
}

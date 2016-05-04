using System.Configuration;
using System.Collections.Specialized;

namespace EAutopay
{
    public class EAutopayConfig : IConfiguration
    {
        string _login;

        NameValueCollection _settings;

        public EAutopayConfig()
        {
            _settings = ConfigurationManager.AppSettings;

            if (string.IsNullOrWhiteSpace(_settings["eautopay_login"]))
            {
                throw new ConfigurationErrorsException("Login is missing in the configuration file. Section name: eautopay_login");
            }
            _login = _settings["eautopay_login"];
        }

        public string LoginUri
        {
            get { return string.Format("https://{0}.e-autopay.com/adminka/login", _login);}
        }

        public string LogoutUri
        {
            get { return string.Format("https://{0}.e-autopay.com/adminka/logout", _login); }
        }

        public string SecretUri
        {
            get { return string.Format("https://{0}.e-autopay.com/adminka/identify", _login); }
        }

        public string MainUri
        {
            get { return string.Format("https://{0}.e-autopay.com/adminka/main.php", _login); }
        }

        public string ProductListUri
        {
            get { return string.Format("https://{0}.e-autopay.com/adminka/tovars/list_tovars.php", _login); }
        }

        public string ProductSaveUri
        {
            get { return string.Format("https://{0}.e-autopay.com/adminka/product/save", _login); }
        }

        public string ProductDeleteUri
        {
            get { return string.Format("https://{0}.e-autopay.com/adminka/tovars/del_tovar.php", _login); }
        }

        public string FormListUri
        {
            get { return string.Format("https://{0}.e-autopay.com/adminka/form_generator/index.php", _login); }
        }

        public string FormSaveUri
        {
            get { return string.Format("https://{0}.e-autopay.com/adminka/form_generator/save_form.php", _login); }
        }

        public string FormDeleteUri
        {
            get { return string.Format("https://{0}.e-autopay.com/adminka/form_generator/delete_form.php", _login); }
        }

        public string GetUpsellUri(int productId)
        {
             return string.Format("https://{0}.e-autopay.com/adminka/product/{1}/upsell", _login, productId);
        }

        public string GetSendSettingsUri(int productId)
        {
            return string.Format("https://{0}.e-autopay.com/adminka/product/edit/{1}/sendsettings", _login, productId);
        }

        public string UpsellLandingPage
        {
            get { return _settings["eautopay_upsell_landing"] ?? ""; }
        }

        public string UpsellSuccessPage
        {
            get { return _settings["eautopay_upsell_success"] ?? ""; }
        }

        public int UpsellInterval
        {
            get
            {
                int ret;
                int.TryParse(_settings["eautopay_upsell_interval"], out ret);
                return ret > 0 ? ret : 20;
            }
        }
    }
}

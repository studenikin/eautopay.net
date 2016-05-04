using System.Configuration;
using System.Collections.Specialized;

namespace EAutopay
{
    /// <summary>
    /// Provides E-Autopay settings based on configuration file (AppSettings).
    /// </summary>
    public class EAutopayConfig : IConfiguration
    {
        string _login;

        NameValueCollection _settings;

        /// <summary>
        /// Initializes instance of the class.
        /// </summary>
        public EAutopayConfig()
        {
            _settings = ConfigurationManager.AppSettings;

            if (string.IsNullOrWhiteSpace(_settings["eautopay_login"]))
            {
                throw new ConfigurationMissingException("eautopay_login");
            }
            _login = _settings["eautopay_login"];
        }

        /// <summary>
        /// URI of the login page.
        /// </summary>
        public string LoginUri
        {
            get { return string.Format("https://{0}.e-autopay.com/adminka/login", _login);}
        }

        /// <summary>
        /// URI of the logout page.
        /// </summary>
        public string LogoutUri
        {
            get { return string.Format("https://{0}.e-autopay.com/adminka/logout", _login); }
        }

        /// <summary>
        /// URI of the secret page. Where secret answer is provided.
        /// </summary>
        public string SecretUri
        {
            get { return string.Format("https://{0}.e-autopay.com/adminka/identify", _login); }
        }

        /// <summary>
        /// URI of the main page. Where user is redirected after logging in.
        /// </summary>
        public string MainUri
        {
            get { return string.Format("https://{0}.e-autopay.com/adminka/main.php", _login); }
        }

        /// <summary>
        /// URI of the product list page.
        /// </summary>
        public string ProductListUri
        {
            get { return string.Format("https://{0}.e-autopay.com/adminka/tovars/list_tovars.php", _login); }
        }

        /// <summary>
        /// URI of the product save page.
        /// </summary>
        public string ProductSaveUri
        {
            get { return string.Format("https://{0}.e-autopay.com/adminka/product/save", _login); }
        }

        /// <summary>
        /// URI of the product delete page.
        /// </summary>
        public string ProductDeleteUri
        {
            get { return string.Format("https://{0}.e-autopay.com/adminka/tovars/del_tovar.php", _login); }
        }

        /// <summary>
        /// URI of the form list page.
        /// </summary>
        public string FormListUri
        {
            get { return string.Format("https://{0}.e-autopay.com/adminka/form_generator/index.php", _login); }
        }

        /// <summary>
        /// URI of the form save page.
        /// </summary>
        public string FormSaveUri
        {
            get { return string.Format("https://{0}.e-autopay.com/adminka/form_generator/save_form.php", _login); }
        }

        /// <summary>
        /// URI of the form delete page.
        /// </summary>
        public string FormDeleteUri
        {
            get { return string.Format("https://{0}.e-autopay.com/adminka/form_generator/delete_form.php", _login); }
        }

        /// <summary>
        /// Gets URI of the upsell page for specified product.
        /// </summary>
        /// <param name="pid">Product ID.</param>
        /// <returns></returns>
        public string GetUpsellUri(int productId)
        {
             return string.Format("https://{0}.e-autopay.com/adminka/product/{1}/upsell", _login, productId);
        }

        /// <summary>
        /// Gets URI of the send settings page for specified product.
        /// </summary>
        /// <param name="productId">ID of the product to get settings for.</param>
        /// <returns></returns>
        public string GetSendSettingsUri(int productId)
        {
            return string.Format("https://{0}.e-autopay.com/adminka/product/edit/{1}/sendsettings", _login, productId);
        }

        /// <summary>
        /// Gets URI of the landing page for upsell.
        /// </summary>
        public string UpsellLandingPage
        {
            get { return _settings["eautopay_upsell_landing"] ?? ""; }
        }

        /// <summary>
        /// Gets URI of the upsell success page - a page to be redirected after making an order with upsell.
        /// </summary>
        public string UpsellSuccessPage
        {
            get { return _settings["eautopay_upsell_success"] ?? ""; }
        }

        /// <summary>
        /// Get interval of time (in minutes) while upsell offer is available.
        /// </summary>
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

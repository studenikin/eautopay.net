namespace EAutopay
{
    /// <summary>
    /// Provides URIs for the E-Autopay pages.
    /// </summary>
    internal class UriProvider
    {
        readonly string _login;

        /// <summary>
        /// Initializes a new instance of the <see cref="UriProvider"/> class.
        /// </summary>
        /// <param name="login">The E-Autopay login.</param>
        public UriProvider(string login)
        {
            _login = login;
        }

        /// <summary>
        /// Gets the URI of the login page.
        /// </summary>
        public string LoginUri
        {
            get { return string.Format("https://{0}.e-autopay.com/adminka/login", _login); }
        }

        /// <summary>
        /// Gets the URI of the logout page.
        /// </summary>
        public string LogoutUri
        {
            get { return string.Format("https://{0}.e-autopay.com/adminka/logout", _login); }
        }

        /// <summary>
        /// Gets the URI of the secret page. Where secret answer is provided.
        /// </summary>
        public string SecretUri
        {
            get { return string.Format("https://{0}.e-autopay.com/adminka/identify", _login); }
        }

        /// <summary>
        /// Gets URI of the main page. Where user is redirected after logging in.
        /// </summary>
        public string MainUri
        {
            get { return string.Format("https://{0}.e-autopay.com/adminka/main.php", _login); }
        }

        /// <summary>
        /// Gets URI of the product list page.
        /// </summary>
        public string ProductListUri
        {
            get { return string.Format("https://{0}.e-autopay.com/adminka/tovars/list_tovars.php", _login); }
        }

        /// <summary>
        /// Gets URI of the product save page.
        /// </summary>
        public string ProductSaveUri
        {
            get { return string.Format("https://{0}.e-autopay.com/adminka/product/save", _login); }
        }

        /// <summary>
        /// Gets URI of the product delete page.
        /// </summary>
        public string ProductDeleteUri
        {
            get { return string.Format("https://{0}.e-autopay.com/adminka/tovars/del_tovar.php", _login); }
        }

        /// <summary>
        /// Gets URI of the form list page.
        /// </summary>
        public string FormListUri
        {
            get { return string.Format("https://{0}.e-autopay.com/adminka/form_generator/index.php", _login); }
        }

        /// <summary>
        /// Gets URI of the form save page.
        /// </summary>
        public string FormSaveUri
        {
            get { return string.Format("https://{0}.e-autopay.com/adminka/form_generator/save_form.php", _login); }
        }

        /// <summary>
        /// Gets URI of the form delete page.
        /// </summary>
        public string FormDeleteUri
        {
            get { return string.Format("https://{0}.e-autopay.com/adminka/form_generator/delete_form.php", _login); }
        }

        /// <summary>
        /// Gets Gets URI of the upsell page for specified product.
        /// </summary>
        /// <param name="productId">Product ID.</param>
        /// <param name="upsellId">Upsell ID.</param>
        /// <returns></returns>
        public string GetUpsellUri(int productId, int upsellId)
        {
            string upsell = upsellId > 0 ? upsellId.ToString() : "";
            return string.Format("https://{0}.e-autopay.com/adminka/product/{1}/upsell/{2}", _login, productId, upsell);
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
    }
}

using System.Configuration;
using System.Collections.Specialized;

namespace EAutopay
{
    /// <summary>
    /// Provides E-Autopay settings based on the configuration file (AppSettings).
    /// </summary>
    public class AppConfig : IConfiguration
    {
        string _login;

        NameValueCollection _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="EAutopayConfig"/> class.
        /// </summary>
        public AppConfig() :this (ConfigurationManager.AppSettings)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EAutopayConfig"/> class.
        /// </summary>
        /// <param name="settings">The list of settings. Default value is AppSettings.</param>
        public AppConfig(NameValueCollection settings)
        {
            _settings = settings;

            if (string.IsNullOrWhiteSpace(_settings["eautopay_login"]))
            {
                throw new ConfigurationMissingException("eautopay_login");
            }
            _login = _settings["eautopay_login"];
        }

        /// <summary>
        /// Gets the E-Autopay login.
        /// </summary>
        public string Login
        {
            get { return _login; }
        }

        /// <summary>
        /// Gets URI of the landing page for upsell.
        /// </summary>
        public string UpsellLandingPage
        {
            get { return _settings["eautopay_upsell_landing"] ?? ""; }
        }

        /// <summary>
        /// Gets interval of time (in minutes) while upsell offer is available.
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

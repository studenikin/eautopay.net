using System.Collections.Specialized;

namespace EAutopay.Tests.Fakes
{
    public class FakeConfig : IConfiguration
    {
        private NameValueCollection _settings;

        public FakeConfig()
        {
            _settings = new NameValueCollection();
        }

        public void SetLogin(string value)
        {
            _settings["eautopay_login"] = value;
        }

        public void SetUpsellLandingPage(string value)
        {
            _settings["eautopay_upsell_landing"] = value;
        }

        public void SetUpsellInterval(string value)
        {
            _settings["eautopay_upsell_interval"] = value;
        }

        public string Login
        {
            get { return (string)_settings["eautopay_login"]; }
        }

        public int UpsellInterval
        {
            get
            {
                int ret;
                int.TryParse(_settings["eautopay_upsell_interval"], out ret);
                return ret;
            }
        }

        public string UpsellLandingPage
        {
            get { return _settings["eautopay_upsell_landing"]; }
        }

        public NameValueCollection Settings
        {
            get { return _settings; }
        }
    }
}

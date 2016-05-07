namespace EAutopay.Tests
{
    public class FakedConfig : IConfiguration
    {
        public string LoginUri
        {
            get { return "http://tempuri.org"; }
        }

        public string LogoutUri
        {
            get { return "http://tempuri.org"; }
        }

        public string SecretUri
        {
            get { return "http://tempuri.org"; }
        }

        public string MainUri
        {
            get { return "http://tempuri.org"; }
        }

        public string ProductListUri
        {
            get { return "http://tempuri.org"; }
        }

        public string ProductSaveUri
        {
            get { return "http://tempuri.org"; }
        }

        public string ProductDeleteUri
        {
            get { return "http://tempuri.org"; }
        }

        public string FormListUri
        {
            get { return "http://tempuri.org"; }
        }

        public string FormSaveUri
        {
            get { return "http://tempuri.org"; }
        }

        public string FormDeleteUri
        {
            get { return "http://tempuri.org"; }
        }

        public string GetUpsellUri(int productId, int upsellId)
        {
            return "http://tempuri.org";
        }

        public string GetSendSettingsUri(int productId)
        {
            return "http://tempuri.org";
        }

        public int UpsellInterval
        {
            get { return 240; }
        }

        public string UpsellLandingPage
        {
            get { return ""; }
        }

        public string UpsellSuccessPage
        {
            get { return ""; }
        }
    }
}

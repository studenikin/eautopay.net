using System.Collections.Specialized;

namespace EAutopay.Security
{
    public class Auth
    {
        string _login;

        string _password;

        string _secret;

        readonly IConfiguration _config;

        /// <summary>
        /// Initializes empty instance of the class.
        /// Use this ctor for performing Logout and IsLogged operations.
        /// </summary>
        public Auth() : this(null, null, null, null)
        {}

        public Auth(string login, string password, string secret) : this(login, password, secret, null)
        {}

        public Auth(string login, string password, string secret, IConfiguration config)
        {
            _login = login;
            _password = password;
            _secret = secret;
            _config = config ?? new EAutopayConfig();
        }

        public AuthResult Login()
        {
            AuthResult result = PostLoginData();

            if (result.Status == AuthStatus.Need_Secret)
            {
                result = PostSecretAnswer();
            }
            return result;
        }

        public void Logout()
        {
            var crawler = new Crawler();
            using (var resp = crawler.HttpGet(_config.LogoutUri)) { }
        }

        public bool IsLogged()
        {
            var crawler = new Crawler();
            using (var resp = crawler.HttpGet(_config.MainUri))
            {
                var ud = new UriDetector(resp.ResponseUri);
                return ud.IsMainURI();
            }
        }

        private AuthResult PostLoginData()
        {
            var paramz = new NameValueCollection();
            paramz["login"] = _login;
            paramz["password"] = _password;

            var crawler = new Crawler();
            using (var resp = crawler.HttpPost(_config.LoginUri, paramz))
            {
                var result = new AuthResult();
                result.SetStatusFromLoginResponse(resp);
                return result;
            }
        }

        private AuthResult PostSecretAnswer()
        {
            NameValueCollection paramz = new NameValueCollection();
            paramz["secret_answer"] = _secret;

            var crawler = new Crawler();
            using (var resp = crawler.HttpPost(_config.SecretUri, paramz))
            {
                var result = new AuthResult();
                result.SetStatusFromSecretResponse(resp);
                return result;
            }
        }
    }
}

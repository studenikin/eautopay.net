using System.Collections.Specialized;

namespace EAutopay.Security
{
    /// <summary>
    /// Encapsulates authentication procedures in E-Autopay.
    /// </summary>
    public class Auth
    {
        string _login;

        string _password;

        string _secret;

        readonly IConfiguration _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="Auth"/> class.
        /// Use this ctor for performing Logout and IsLogged operations.
        /// </summary>
        public Auth() : this(null, null, null, null)
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="Auth"/> class.
        /// </summary>
        /// <param name="login">Login in E-Autopay.</param>
        /// <param name="password">Password in E-Autopay.</param>
        /// <param name="secret">Answer to secret question in E-Autopay.</param>
        public Auth(string login, string password, string secret) : this(login, password, secret, null)
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="Auth"/> class.
        /// </summary>
        /// <param name="login">Login in E-Autopay.</param>
        /// <param name="password">Password in E-Autopay.</param>
        /// <param name="secret">Answer to secret question in E-Autopay.</param>
        /// <param name="config">General E-Autopay settings.</param>
        public Auth(string login, string password, string secret, IConfiguration config)
        {
            _login = login;
            _password = password;
            _secret = secret;
            _config = config ?? new EAutopayConfig();
        }

        /// <summary>
        /// Attempts to login in E-Autopay.
        /// </summary>
        /// <returns>Result of the login operation.</returns>
        public AuthResult Login()
        {
            AuthResult result = PostLoginData();

            if (result.Status == AuthStatus.Need_Secret)
            {
                result = PostSecretAnswer();
            }
            return result;
        }

        /// <summary>
        /// Logout from E-Autopay.
        /// </summary>
        public void Logout()
        {
            var crawler = new Crawler();
            using (var resp = crawler.HttpGet(_config.LogoutUri)) { }
        }

        /// <summary>
        /// Returns a value indicating whether a user is logged in E-Autopay.
        /// </summary>
        /// <returns>true if the user is logged in; otherwise, false.</returns>
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

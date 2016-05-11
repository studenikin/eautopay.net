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

        readonly ICrawler _crawler;

        /// <summary>
        /// Initializes a new instance of the <see cref="Auth"/> class.
        /// Use this ctor for performing Logout and IsLogged operations.
        /// </summary>
        public Auth() : this(null, null, null, null, null)
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="Auth"/> class.
        /// </summary>
        /// <param name="login">Login in E-Autopay.</param>
        /// <param name="password">Password in E-Autopay.</param>
        /// <param name="secret">Answer to secret question in E-Autopay.</param>
        public Auth(string login, string password, string secret) : this(login, password, secret, null, null)
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="Auth"/> class.
        /// </summary>
        /// <param name="login">Login in E-Autopay.</param>
        /// <param name="password">Password in E-Autopay.</param>
        /// <param name="secret">Answer to secret question in E-Autopay.</param>
        /// <param name="config">General E-Autopay settings.</param>
        public Auth(string login, string password, string secret, IConfiguration config, ICrawler crawler)
        {
            _login = login;
            _password = password;
            _secret = secret;
            _config = config ?? new AppConfig();
            _crawler = crawler ?? new Crawler();
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
            var up = new UriProvider(_config.Login);
            _crawler.Get(up.LogoutUri);
        }

        /// <summary>
        /// Returns a value indicating whether a user is logged in E-Autopay.
        /// </summary>
        /// <returns>true if the user is logged in; otherwise, false.</returns>
        public bool IsLogged
        {
            get
            {
                var up = new UriProvider(_config.Login);

                var resp = _crawler.Get(up.MainUri);

                var ud = new UriDetector(resp.Uri);
                return ud.IsMainURI;
            }
        }

        private AuthResult PostLoginData()
        {
            var paramz = new NameValueCollection();
            paramz["login"] = _login;
            paramz["password"] = _password;

            var up = new UriProvider(_config.Login);

            var resp = _crawler.Post(up.LoginUri, paramz);

            var result = new AuthResult();
            result.SetStatusFromLoginResponse(resp);
            return result;
        }

        private AuthResult PostSecretAnswer()
        {
            NameValueCollection paramz = new NameValueCollection();
            paramz["secret_answer"] = _secret;

            var up = new UriProvider(_config.Login);

            var resp = _crawler.Post(up.SecretUri, paramz);

            var result = new AuthResult();
            result.SetStatusFromSecretResponse(resp);
            return result;
        }
    }
}

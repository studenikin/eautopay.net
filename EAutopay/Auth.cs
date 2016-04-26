using System;
using System.Collections.Specialized;

namespace EAutopay
{
    public class Auth
    {
        string _login;
        string _password;
        string _secret;

        public Auth(string login, string password, string secret)
        {
            _login = login;
            _password = password;
            _secret = secret;
        }

        public AuthResult Login()
        {
            AuthResult result = PostLoginData();

            if (result.Status == AuthResult.Statuses.Need_Secret)
            {
                result = PostSecretAnswer();
            }
            return result;
        }

        private AuthResult PostLoginData()
        {
            var paramz = new NameValueCollection();
            paramz["login"] = _login;
            paramz["password"] = _password;

            var crawler = new Crawler();
            using (var resp = crawler.HttpPost(Config.URI_LOGIN, paramz))
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
            using (var resp = crawler.HttpPost(Config.URI_SECRET, paramz)) 
            {
                var result = new AuthResult();
                result.SetStatusFromSecretResponse(resp);
                return result;
            }
        }

        public static void Logout()
        {
            var crawler = new Crawler();
            using (var resp = crawler.HttpGet(Config.URI_LOGOUT)) { }
        }

        public static bool IsLogged()
        {
            var crawler = new Crawler();
            using (var resp = crawler.HttpGet(Config.URI_MAIN))
            {
                var ud = new UriDetector(resp.ResponseUri);
                return ud.IsMainURI();
            }
        }
    }
}

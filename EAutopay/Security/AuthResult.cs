using System.Net;

namespace EAutopay.Security
{
    public class AuthResult
    {
        private AuthStatus _status;

        /// <summary>
        /// Initializes a new instance of the EAutopay.Security.AuthResult class.
        /// </summary>
        public AuthResult()
        {
            _status = AuthStatus.Login_Failed;
        }

        /// <summary>
        /// Gets results of the authentication.
        /// </summary>
        public AuthStatus Status { get { return _status; } }

        internal void SetStatusFromLoginResponse(HttpWebResponse resp)
        {
            var ud = new UriDetector(resp.ResponseUri);

            if (ud.IsMainURI())
            {
               _status = AuthStatus.Ok;
            }
            else if (ud.IsSecretAnswerURI())
            {
                _status = AuthStatus.Need_Secret;
            }
            else
            {
                _status = AuthStatus.Login_Failed;
            }
        }

        internal void SetStatusFromSecretResponse(HttpWebResponse resp)
        {
            var ud = new UriDetector(resp.ResponseUri);

            if (ud.IsMainURI())
            {
                _status = AuthStatus.Ok;
            }
            else if (ud.IsLoginURI())
            {
                _status = AuthStatus.Secret_Failed;
            }
        }
    }
}

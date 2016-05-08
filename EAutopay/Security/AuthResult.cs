using System.Net;

namespace EAutopay.Security
{
    /// <summary>
    /// Provides information about authentication result.
    /// </summary>
    public class AuthResult
    {
        private AuthStatus _status;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthResult"/> class.
        /// </summary>
        public AuthResult()
        {
            _status = AuthStatus.Login_Failed;
        }

        /// <summary>
        /// Gets result of the authentication.
        /// </summary>
        public AuthStatus Status { get { return _status; } }

        /// <summary>
        /// Sets authentication status based on response received on posting credentials.
        /// </summary>
        /// <param name="resp">Response object received on posting credentials.</param>
        internal void SetStatusFromLoginResponse(HttpWebResponse resp)
        {
            var ud = new UriDetector(resp.ResponseUri);

            if (ud.IsMainURI)
            {
               _status = AuthStatus.Ok;
            }
            else if (ud.IsSecretAnswerURI)
            {
                _status = AuthStatus.Need_Secret;
            }
            else
            {
                _status = AuthStatus.Login_Failed;
            }
        }

        /// <summary>
        /// Sets authentication status based on response received on posting secret answer.
        /// </summary>
        /// <param name="resp">Response object received on posting secret answer.</param>
        internal void SetStatusFromSecretResponse(HttpWebResponse resp)
        {
            var ud = new UriDetector(resp.ResponseUri);

            if (ud.IsMainURI)
            {
                _status = AuthStatus.Ok;
            }
            else if (ud.IsLoginURI)
            {
                _status = AuthStatus.Secret_Failed;
            }
        }
    }
}

using System;
using System.Net;


namespace EAutopay.NET
{
    public class AuthResult
    {
        public enum Statuses
        {
            Ok = 1,
            Login_Failed = 2,
            Need_Secret = 3,
            Secret_Failed = 4
        }

        private Statuses _status;
        public Statuses Status { get { return _status; } }

        public AuthResult()
        {
            _status = Statuses.Login_Failed;
        }

        internal void SetStatusFromLoginResponse(HttpWebResponse resp)
        {
            var ud = new UriDetector(resp.ResponseUri);

            if (ud.IsMainURI())
            {
               _status = Statuses.Ok;
            }
            else if (ud.IsSecretAnswerURI())
            {
                _status = Statuses.Need_Secret;
            }
            else
            {
                _status = Statuses.Login_Failed;
            }
        }

        internal void SetStatusFromSecretResponse(HttpWebResponse resp)
        {
            var ud = new UriDetector(resp.ResponseUri);

            if (ud.IsMainURI())
            {
                _status = Statuses.Ok;
            }
            else if (ud.IsLoginURI())
            {
                _status = Statuses.Secret_Failed;
            }
        }
    }
}

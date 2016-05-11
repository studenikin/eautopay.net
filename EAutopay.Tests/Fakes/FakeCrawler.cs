using System;
using System.Collections.Specialized;

namespace EAutopay.Tests.Fakes
{
    public class FakeCrawler : ICrawler
    {
        string _uri;

        NameValueCollection _paramz;

        string _responseUri;

        public EAutopayResponse Get(string uri)
        {
            return Get(uri, null);
        }

        public EAutopayResponse Get(string uri, NameValueCollection paramz)
        {
            _uri = uri;
            _paramz = paramz;

            return new EAutopayResponse { Uri = GetResponseUri()};
        }

        public EAutopayResponse Post(string uri, NameValueCollection paramz)
        {
            _uri = uri;
            _paramz = paramz;

            return new EAutopayResponse { Uri = GetResponseUri() };
        }

        public string Uri
        {
            get { return _uri; }
        }

        public bool WasCalled
        {
            get { return !string.IsNullOrEmpty(_uri); }
        }

        public NameValueCollection Paramz
        {
            get { return _paramz; }
        }

        public void SetResponseUri(string uri)
        {
            _responseUri = uri;
        }

        private Uri GetResponseUri()
        {
            string ret = "http://domain.com";
            if (!string.IsNullOrWhiteSpace(_responseUri))
            {
                ret = _responseUri;
            }
            return new Uri(ret);
        }
    }
}

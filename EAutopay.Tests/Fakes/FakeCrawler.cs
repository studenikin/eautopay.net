using System.Net;
using System.Collections.Specialized;

namespace EAutopay.Tests.Fakes
{
    public class FakeCrawler : ICrawler
    {
        bool _wasCalled;

        NameValueCollection _paramz;

        public EAutopayResponse Get(string uri)
        {
            return Get(uri, null);
        }

        public EAutopayResponse Get(string uri, NameValueCollection paramz)
        {
            _wasCalled = true;
            _paramz = paramz;
            return new EAutopayResponse();
        }

        public EAutopayResponse Post(string uri, NameValueCollection paramz)
        {
            _wasCalled = true;
            _paramz = paramz;
            return new EAutopayResponse();
        }

        public bool WasCalled
        {
            get { return _wasCalled; }
        }

        public NameValueCollection Paramz
        {
            get { return _paramz; }
        }
    }
}

using System.Net;
using System.Collections.Specialized;

namespace EAutopay.Tests.Fakes
{
    public class FakeCrawler : ICrawler
    {
        bool _wasCalled;

        public EAutopayResponse Get(string uri, NameValueCollection paramz)
        {
            _wasCalled = true;
            return new EAutopayResponse { Data = "", StatusCode = HttpStatusCode.OK };
        }

        public EAutopayResponse Post(string uri, NameValueCollection paramz)
        {
            _wasCalled = true;
            return new EAutopayResponse { Data = "", StatusCode = HttpStatusCode.OK };
        }

        public bool WasCalled
        {
            get { return _wasCalled; }
        }
    }
}

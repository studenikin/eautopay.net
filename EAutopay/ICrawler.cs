using System.Collections.Specialized;

namespace EAutopay
{
    public interface ICrawler
    {
        EAutopayResponse Get(string uri);

        EAutopayResponse Get(string uri, NameValueCollection paramz);

        EAutopayResponse Post(string uri, NameValueCollection paramz);
    }
}

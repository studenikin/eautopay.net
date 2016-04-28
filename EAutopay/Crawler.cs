using System.IO;
using System.Net;
using System.Collections.Specialized;

namespace EAutopay
{
    public class Crawler
    {
        private ICache _cache;

        private string Token
        {
            get
            {
                if (_cache.Get("eautopay_token") == null)
                {
                    Token = RetrieveToken();
                }
                return (string)_cache.Get("eautopay_token");
            }
            set
            {
                _cache.Set("eautopay_token", value);
            }
        }

        private CookieCollection Cookies
        {
            get
            {
                if (_cache.Get("eautopay_cookies") != null)
                {
                    return (CookieCollection)_cache.Get("eautopay_cookies");
                }
                return new CookieCollection();
            }
            set
            {
                var cookies = new CookieCollection();
                if (_cache.Get("eautopay_cookies") != null)
                {
                    cookies = (CookieCollection)_cache.Get("eautopay_cookies");
                }
                cookies.Add(value);
                _cache.Set("eautopay_cookies", cookies);
            }
        }

        public Crawler(): this(null)
        { }

        public Crawler(ICache cache)
        {
            _cache = cache ?? new HttpCache();

            if (string.IsNullOrEmpty(Token))
            {
                Token = RetrieveToken();
            }
        }

        private string RetrieveToken()
        {
            using (var resp = HttpGet(Config.URI_LOGIN))
            {
                var reader = new StreamReader(resp.GetResponseStream());
                var parser = new Parser(reader.ReadToEnd());
                return parser.GetToken();
            }
        }

        public HttpWebResponse HttpPost(string uri, NameValueCollection paramz)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = "Mozila";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.Headers.Add("Accept-Language: en-US,en;q=0.8,da;q=0.6,nb;q=0.4,ru;q=0.2,cs;q=0.2");

            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(Cookies);

            WritePostData(request, paramz);

            var resp = (HttpWebResponse)request.GetResponse();
            Cookies = resp.Cookies;
            return resp;
        }

        public HttpWebResponse HttpGet(string uri)
        {
            return HttpGet(uri, null);
        }

        public HttpWebResponse HttpGet(string uri, NameValueCollection paramz)
        {

            string uriWithParams = GetUriWithParams(uri, paramz);

            var request = (HttpWebRequest)WebRequest.Create(uriWithParams);
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(Cookies);

            var resp = (HttpWebResponse)request.GetResponse();
            Cookies = resp.Cookies;
            return resp;
        }

        private void WritePostData(HttpWebRequest request, NameValueCollection paramz)
        {
            using (var stream = request.GetRequestStream())
            {
                using (var writer = new StreamWriter(stream))
                {
                    string postData = "";
                    paramz.Add("_token", Token);
                    foreach (string p in paramz)
                    {
                        postData += string.Format("{0}={1}&", p, paramz[p]);
                    }
                    writer.Write(postData.Trim('&'));
                }
            }
        }

        private string GetUriWithParams(string uri, NameValueCollection paramz)
        {
            if (paramz != null)
            {
                uri += "?";
                foreach (string p in paramz)
                {
                    uri += string.Format("{0}={1}&", p, paramz[p]);
                }
                uri = uri.Trim('&');
            }
            return uri;
        }
    }
}

using System.IO;
using System.Net;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;

using EAutopay.Parsers;

[assembly: InternalsVisibleTo("EAutopay.Tests")]
namespace EAutopay
{
    /// <summary>
    /// Provides an easy way to post/get http requests from/to E-Autopay.
    /// </summary>
    public class Crawler : ICrawler
    {
        readonly ICache _cache;

        readonly IConfiguration _config;

        readonly ITokenParser _parser;

        /// <summary>
        /// Initializes a new instance of the <see cref="Crawler"/> class.
        /// </summary>
        public Crawler() : this(null, null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Crawler"/> class.
        /// </summary>
        /// <param name="cache"><see cref="ICache"/> for caching Cookies, etc.</param>
        /// <param name="config">General E-Autopay settings.</param>
        /// <param name="parser"><see cref="ITokenParser"/> to retrieve token.</param>
        public Crawler(ICache cache, IConfiguration config, ITokenParser parser)
        {
            _cache = cache ?? new HttpCache();

            _config = config ?? new AppConfig();

            _parser = parser ?? new EAutopayTokenParser();

            if (string.IsNullOrEmpty(Token))
            {
                Token = RetrieveToken();
            }
        }

        /// <summary>
        /// Posts data and gets the response from the specified URI in E-Autopay.
        /// </summary>
        /// <param name="uri">The URI of the page in E-Autopay.</param>
        /// <param name="paramz">The data to send to the page.</param>
        /// <returns>A <see cref="EAutopayResponse"/> that contains the response from the URI.</returns>
        public EAutopayResponse Post(string uri, NameValueCollection paramz)
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

            CrawlerService.WritePostData(request, paramz, Token);

            using (var resp = (HttpWebResponse)request.GetResponse())
            {
                Cookies = resp.Cookies;
                return GetEAutopayResponse(resp);
            }
        }

        /// <summary>
        /// Gets the response from the specified URI in E-Autopay.
        /// </summary>
        /// <param name="uri">The URI of the page in E-Autopay.</param>
        /// <param name="paramz">The data to send to the page.</param>
        /// <returns>A <see cref="EAutopayResponse"/> that contains the response from the URI.</returns>
        public EAutopayResponse Get(string uri)
        {
            return Get(uri, null);
        }

        /// <summary>
        /// Gets the response from the specified URI in E-Autopay.
        /// </summary>
        /// <param name="uri">The URI of the page in E-Autopay.</param>
        /// <param name="paramz">The data to send to the page.</param>
        /// <returns>A <see cref="EAutopayResponse"/> that contains the response from the URI.</returns>
        public EAutopayResponse Get(string uri, NameValueCollection paramz)
        {
            string uriWithParams = CrawlerService.CombineUriWithParams(uri, paramz);

            var request = (HttpWebRequest)WebRequest.Create(uriWithParams);
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(Cookies);

            using (var resp = (HttpWebResponse)request.GetResponse())
            {
                Cookies = resp.Cookies;
                return GetEAutopayResponse(resp);
            }
        }

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
            set { _cache.Set("eautopay_token", value); }
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

        private string RetrieveToken()
        {
            var up = new UriProvider(_config.Login);
            var resp = Get(up.LoginUri);
            return _parser.ExtractToken(resp.Data);
        }

        private EAutopayResponse GetEAutopayResponse(HttpWebResponse httpResp)
        {
            var ret = new EAutopayResponse();

            ret.Uri = httpResp.ResponseUri;
            ret.StatusCode = httpResp.StatusCode;
            
            using (var stream = httpResp.GetResponseStream())
            {
                var reader = new StreamReader(httpResp.GetResponseStream());
                ret.Data = reader.ReadToEnd();
            }
            return ret;
        }
    }
}

using System;
using System.IO;
using System.Net;
using System.Web;
using System.Collections.Specialized;

namespace EAutopay.NET
{
    public class Poster
    {
        private string Token
        {
            get
            {
                if (HttpContext.Current.Cache["eautopay_token"] == null)
                {
                    Token = RetrieveToken();
                }
                return (string)HttpContext.Current.Cache["eautopay_token"];
            }
            set
            {
                HttpContext.Current.Cache.Insert("eautopay_token", value);
            }
        }

        private CookieCollection Cookies
        {
            get
            {
                if (HttpContext.Current.Cache["eautopay_cookies"] != null)
                {
                    return (CookieCollection)HttpContext.Current.Cache["eautopay_cookies"];
                }
                return new CookieCollection();
            }
            set
            {
                var cookies = new CookieCollection();
                if (HttpContext.Current.Cache["eautopay_cookies"] != null)
                {
                    cookies = (CookieCollection)HttpContext.Current.Cache["eautopay_cookies"];
                }
                cookies.Add(value);
                HttpContext.Current.Cache.Insert("eautopay_cookies", cookies);
            }
        }


        public Poster()
        {
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
                return Parser.GetToken(reader.ReadToEnd());
            }
        }

        public HttpWebResponse HttpPost(string uri, NameValueCollection paramz)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = "Mozila";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.Headers.Add("Accept-Language: en-US,en;q=0.8,da;q=0.6,nb;q=0.4,ru;q=0.2,cs;q=0.2");

            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(Cookies);

            using (Stream stream = request.GetRequestStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
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
            if (paramz != null)
            {
                uri += "?";
                foreach (string p in paramz)
                {
                    uri += string.Format("{0}={1}&", p, paramz[p]);
                }
                uri = uri.Trim('&');
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(Cookies);

            var resp = (HttpWebResponse)request.GetResponse();
            Cookies = resp.Cookies;
            return resp;
        }
    }
}

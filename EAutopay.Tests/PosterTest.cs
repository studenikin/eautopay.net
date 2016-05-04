using System;
using System.Net;
using System.Collections.Specialized;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using EAutopay;


namespace EAutopay.Tests
{
    [TestClass]
    public class PosterTest
    {
        [TestMethod]
        public void Paramz_Added_To_URI_ON_GET()
        {
            var cache = new FackedCache();
            cache.Set("eautopay_token", "1234");

            var cookies = new CookieCollection();
            cache.Set("eautopay_cookies", cookies);

            IConfiguration config = new FakedConfig();

            var crawler = new Crawler(cache, config);

            string uri = "http://tempuri.org";
            var paramz = new NameValueCollection { { "param1", "1" }, { "param2", "xyz" } };
            crawler.HttpGet(uri, paramz);

            var mock = new { uri };

            //Assert.AreEqual(uri + "?param1=1&param2=xyz", mock.uri);
        }
    }
}

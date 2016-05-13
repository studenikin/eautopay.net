using System.IO;
using System.Net;
using System.Collections.Specialized;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace EAutopay.Tests
{
    [TestClass]
    public class CrawlerServiceTest
    {
        const string BASE_URI = "http://e-autopay.com/";
        const string TOKEN = "1234qwerty";

        [TestMethod]
        public void CombineUri_Should_Return_Base_Uri_If_Paramz_Not_Specified()
        {
            var uri = CrawlerService.CombineUriWithParams(BASE_URI, null);
            Assert.AreEqual(BASE_URI, uri);
        }

        [TestMethod]
        public void CombineUri_Should_Return_Base_Uri_If_Paramz_Are_Empty()
        {
            var paramz = new NameValueCollection();
            var uri = CrawlerService.CombineUriWithParams(BASE_URI, paramz);
            Assert.AreEqual(BASE_URI, uri);
        }

        [TestMethod]
        public void CombineUri_Should_Use_Question_Sign_For_First_Param()
        {
            var paramz = new NameValueCollection();
            paramz.Add("id", "1234");

            var uri = CrawlerService.CombineUriWithParams(BASE_URI, paramz);
            Assert.AreEqual(BASE_URI +"?id=1234", uri);
        }

        [TestMethod]
        public void CombineUri_Should_Use_Amp_Sign_For_Second_Param()
        {
            var paramz = new NameValueCollection();
            paramz.Add("id", "1234");
            paramz.Add("name", "test");

            var uri = CrawlerService.CombineUriWithParams(BASE_URI, paramz);
            Assert.AreEqual(BASE_URI + "?id=1234&name=test", uri);
        }

        [TestMethod]
        public void BuildPostData_Should_Have_Token_If_Paramz_Not_Specified()
        {
            string data = CrawlerService.BuildPostData(null, TOKEN);
            Assert.AreEqual("_token="+ TOKEN, data);        
        }

        [TestMethod]
        public void BuildPostData_Should_Have_Token_If_Paramz_Empty()
        {
            string data = CrawlerService.BuildPostData(new NameValueCollection(), TOKEN);
            Assert.AreEqual("_token="+ TOKEN, data);
        }

        [TestMethod]
        public void BuildPostData_Should_Use_Amp_As_Delimeter()
        {
            var paramz = new NameValueCollection();
            paramz["id"] = "1234";
            paramz["name"] = "test";

            string data = CrawlerService.BuildPostData(paramz, TOKEN);
            Assert.AreEqual("_token=" + TOKEN +"&id=1234&name=test", data);
        }
    }
}

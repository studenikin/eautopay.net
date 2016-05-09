using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using EAutopay;

namespace EAutopay.Tests
{
    [TestClass]
    public class UriDetectorTest
    {
        const string uriSecret = "https://login.e-autopay.com/adminka/identify";
        const string uriLogin = "https://login.e-autopay.com/adminka/login";
        const string uriMain = "https://login.e-autopay.com/adminka/main.php";
        const string uriProducts = "https://login.e-autopay.com/adminka/tovars/list_tovars.php";
        const string uriBad = "https://login.e-autopay.com/adminka/logout";

        [TestMethod]
        public void IsSecretAnswer_True_For_Good_URI()
        {
            var ud = new UriDetector(new Uri(uriSecret));
            Assert.IsTrue(ud.IsSecretAnswerURI);
        }

        [TestMethod]
        public void IsSecretAnswer_False_For_Bad_URI()
        {
            var ud = new UriDetector(new Uri(uriBad));
            Assert.IsFalse(ud.IsSecretAnswerURI);
        }

        [TestMethod]
        public void IsLogin_True_For_Good_URI()
        {
            var ud = new UriDetector(new Uri(uriLogin));
            Assert.IsTrue(ud.IsLoginURI);
        }

        [TestMethod]
        public void IsLogin_False_For_Bad_URI()
        {
            var ud = new UriDetector(new Uri(uriBad));
            Assert.IsFalse(ud.IsLoginURI);
        }

        [TestMethod]
        public void IsMain_True_For_Good_URI()
        {
            var ud = new UriDetector(new Uri(uriMain));
            Assert.IsTrue(ud.IsMainURI);
        }

        [TestMethod]
        public void IsMain_False_For_Bad_URI()
        {
            var ud = new UriDetector(new Uri(uriBad));
            Assert.IsFalse(ud.IsMainURI);
        }

        [TestMethod]
        public void IsProductList_True_For_Good_URI()
        {
            var ud = new UriDetector(new Uri(uriProducts));
            Assert.IsTrue(ud.IsProdutListURI);
        }

        [TestMethod]
        public void IsProductList_False_For_Bad_URI()
        {
            var ud = new UriDetector(new Uri(uriBad));
            Assert.IsFalse(ud.IsProdutListURI);
        }
    }
}

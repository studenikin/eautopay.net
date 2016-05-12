using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EAutopay.Tests
{
    [TestClass]
    public class UriDetectorTest
    {
        [TestMethod]
        public void IsSecretAnswer_True_For_Good_URI()
        {
            var ud = new UriDetector(new Uri(Common.SECRET_URI));
            Assert.IsTrue(ud.IsSecretAnswerURI);
        }

        [TestMethod]
        public void IsSecretAnswer_False_For_Bad_URI()
        {
            var ud = new UriDetector(new Uri(Common.FORMS_URI));
            Assert.IsFalse(ud.IsSecretAnswerURI);
        }

        [TestMethod]
        public void IsLogin_True_For_Good_URI()
        {
            var ud = new UriDetector(new Uri(Common.LOGIN_URI));
            Assert.IsTrue(ud.IsLoginURI);
        }

        [TestMethod]
        public void IsLogin_False_For_Bad_URI()
        {
            var ud = new UriDetector(new Uri(Common.FORMS_URI));
            Assert.IsFalse(ud.IsLoginURI);
        }

        [TestMethod]
        public void IsMain_True_For_Good_URI()
        {
            var ud = new UriDetector(new Uri(Common.MAIN_URI));
            Assert.IsTrue(ud.IsMainURI);
        }

        [TestMethod]
        public void IsMain_False_For_Bad_URI()
        {
            var ud = new UriDetector(new Uri(Common.FORMS_URI));
            Assert.IsFalse(ud.IsMainURI);
        }

        [TestMethod]
        public void IsProductList_True_For_Good_URI()
        {
            var ud = new UriDetector(new Uri(Common.PRODUCTS_URI));
            Assert.IsTrue(ud.IsProdutListURI);
        }

        [TestMethod]
        public void IsProductList_False_For_Bad_URI()
        {
            var ud = new UriDetector(new Uri(Common.FORMS_URI));
            Assert.IsFalse(ud.IsProdutListURI);
        }
    }
}

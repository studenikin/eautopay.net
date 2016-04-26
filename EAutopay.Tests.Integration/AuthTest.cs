using System;
using System.Web;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using EAutopay;


namespace EAutopay.Tests.Integration
{
    [TestClass]
    public class AuthTest
    {
        [TestInitialize]
        public void SetUp()
        {
            HttpContext.Current = Common.GetHttpContext();
            Common.Login();
        }

        [TestCleanup]
        public void TearDown()
        {
            Common.Logout();
            HttpContext.Current = null;
        }


        [TestMethod]
        public void Auth_LoginSuccess()
        {
            var result = Common.Login();

            Assert.AreEqual(AuthResult.Statuses.Ok, result.Status);
        }


        [TestMethod]
        public void Auth_LoginFailed()
        {
            LoginAndLogout();

            var auth = new Auth("invalid_login", "invalid_password", "invalid_secret");

            var result = auth.Login();

            Assert.AreEqual(AuthResult.Statuses.Login_Failed, result.Status);
        }


        [TestMethod]
        public void Auth_SecretAnswerFailed()
        {
            LoginAndLogout();

            var settings = ConfigurationManager.AppSettings;
            var auth = new Auth(settings["login"], settings["password"], "invalid_secret");

            var result = auth.Login();

            Assert.AreEqual(AuthResult.Statuses.Secret_Failed, result.Status);
        }


        [TestMethod]
        public void Auth_Logout()
        {
            LoginAndLogout();

            Common.Login();
            Common.Logout();
            
            Assert.IsFalse(Auth.IsLogged());
        }


        [TestMethod]
        public void Auth_IsLogged()
        {
            LoginAndLogout();

            Common.Login();

            Assert.IsTrue(Auth.IsLogged());
        }


        /// <summary>
        /// Resets the "failed attempts" counter.
        /// Otherwise E-Autopay blocks account after 3 unsuccessfull attempts.
        /// </summary>
        private void LoginAndLogout()
        {
            if (Auth.IsLogged())
            {
                Common.Logout();
            }
            Common.Login();
            Common.Logout();
        }

    }
}

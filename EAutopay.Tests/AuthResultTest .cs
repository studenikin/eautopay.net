using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using EAutopay.Security;

namespace EAutopay.Tests
{
    [TestClass]
    public class AuthResultTest
    {
        [TestMethod]
        public void Status_For_New_AuthResult_Should_Be_LoginFailed()
        {
            var ar = new AuthResult();
            Assert.AreEqual(AuthStatus.Login_Failed, ar.Status);
        }

        [TestMethod]
        public void SLoginStatus_Should_Be_OK_Form_MainUri()
        {
            var resp = new EAutopayResponse();
            resp.Uri = new Uri("https://login.e-autopay.com/adminka/main.php");

            var ar = new AuthResult();
            ar.SetStatusFromLoginResponse(resp);

            Assert.AreEqual(AuthStatus.Ok, ar.Status);
        }

        [TestMethod]
        public void LoginStatus_Should_Be_NeedSecret_For_SecretUri()
        {
            var resp = new EAutopayResponse();
            resp.Uri = new Uri("https://login.e-autopay.com/adminka/identify");

            var ar = new AuthResult();
            ar.SetStatusFromLoginResponse(resp);

            Assert.AreEqual(AuthStatus.Need_Secret, ar.Status);
        }

        [TestMethod]
        public void LoginStatus_Should_Be_Failed_For_OtherUri()
        {
            var resp = new EAutopayResponse();
            resp.Uri = new Uri("https://login.e-autopay.com/adminka/forms");

            var ar = new AuthResult();
            ar.SetStatusFromLoginResponse(resp);

            Assert.AreEqual(AuthStatus.Login_Failed, ar.Status);
        }

        [TestMethod]
        public void SecretStatus_Should_Be_OK_For_MainUri()
        {
            var resp = new EAutopayResponse();
            resp.Uri = new Uri("https://login.e-autopay.com/adminka/main.php");

            var ar = new AuthResult();
            ar.SetStatusFromSecretResponse(resp);

            Assert.AreEqual(AuthStatus.Ok, ar.Status);
        }

        [TestMethod]
        public void SecretStatus_Should_Be_SecretFailed_For_LoginUri()
        {
            var resp = new EAutopayResponse();
            resp.Uri = new Uri("https://login.e-autopay.com/adminka/login");

            var ar = new AuthResult();
            ar.SetStatusFromSecretResponse(resp);

            Assert.AreEqual(AuthStatus.Secret_Failed, ar.Status);
        }
    }
}

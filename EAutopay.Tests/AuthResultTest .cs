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
            resp.Uri = new Uri(Common.MAIN_URI);

            var ar = new AuthResult();
            ar.SetStatusFromLoginResponse(resp);

            Assert.AreEqual(AuthStatus.Ok, ar.Status);
        }

        [TestMethod]
        public void LoginStatus_Should_Be_NeedSecret_For_SecretUri()
        {
            var resp = new EAutopayResponse();
            resp.Uri = new Uri(Common.SECRET_URI);

            var ar = new AuthResult();
            ar.SetStatusFromLoginResponse(resp);

            Assert.AreEqual(AuthStatus.Need_Secret, ar.Status);
        }

        [TestMethod]
        public void LoginStatus_Should_Be_Failed_For_OtherUri()
        {
            var resp = new EAutopayResponse();
            resp.Uri = new Uri(Common.FORMS_URI);

            var ar = new AuthResult();
            ar.SetStatusFromLoginResponse(resp);

            Assert.AreEqual(AuthStatus.Login_Failed, ar.Status);
        }

        [TestMethod]
        public void SecretStatus_Should_Be_OK_For_MainUri()
        {
            var resp = new EAutopayResponse();
            resp.Uri = new Uri(Common.MAIN_URI);

            var ar = new AuthResult();
            ar.SetStatusFromSecretResponse(resp);

            Assert.AreEqual(AuthStatus.Ok, ar.Status);
        }

        [TestMethod]
        public void SecretStatus_Should_Be_SecretFailed_For_LoginUri()
        {
            var resp = new EAutopayResponse();
            resp.Uri = new Uri(Common.LOGIN_URI);

            var ar = new AuthResult();
            ar.SetStatusFromSecretResponse(resp);

            Assert.AreEqual(AuthStatus.Secret_Failed, ar.Status);
        }
    }
}

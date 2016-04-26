using System;
using System.Web;
using System.Linq;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using EAutopay.NET;

namespace EAutopay.NET_Tests
{
    [TestClass]
    public class FormRepositoryTest
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
            Common.RemoveAllTestForms();

            Common.Logout();
            HttpContext.Current = null;
        }

        [TestMethod]
        public void FormRepository_IsReturnNullWhenFormDoesntExist()
        {
            int id = 1; // form with such id doesn't exist
            var form = FormRepository.Get(id);

            Assert.IsNull(form);
        }

        [TestMethod]
        public void FormRepository_IsReturnCorrectFormByID()
        {
            var form = Common.CreateTestForm();
            var form2 = FormRepository.Get(form.ID);

            Assert.IsNotNull(form2);
            Assert.AreEqual(form.ID, form2.ID);
            Assert.AreEqual(form.Name, form2.Name);
        }
    }
}

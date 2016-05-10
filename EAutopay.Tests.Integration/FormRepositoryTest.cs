using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using EAutopay.Forms;

namespace EAutopay.Tests.Integration
{
    [TestClass]
    public class FormRepositoryTest
    {
        IFormRepository _formRepository;

        [TestInitialize]
        public void SetUp()
        {
            HttpContext.Current = Common.GetHttpContext();
            Common.Login();
            _formRepository = new EAutopayFormRepository();
        }

        [TestCleanup]
        public void TearDown()
        {
            Common.RemoveAllTestForms();

            Common.Logout();
            HttpContext.Current = null;
            _formRepository = null;
        }

        [TestMethod]
        public void FormRepository_IsReturnNullWhenFormDoesntExist()
        {
            int id = 1; // form with such id doesn't exist
            var form = _formRepository.Get(id);

            Assert.IsNull(form);
        }

        [TestMethod]
        public void FormRepository_IsReturnCorrectFormByID()
        {
            var form = Common.CreateTestForm();
            var form2 = _formRepository.Get(form.ID);

            Assert.IsNotNull(form2);
            Assert.AreEqual(form.ID, form2.ID);
            Assert.AreEqual(form.Name, form2.Name);
        }
    }
}

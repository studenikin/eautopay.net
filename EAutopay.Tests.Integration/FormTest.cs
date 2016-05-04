using System.Web;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using EAutopay.Forms;

namespace EAutopay.Tests.Integration
{
    [TestClass]
    public class FormTest
    {
        IFormRepository _formRepository = new EAutopayFormRepository();

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
        public void Form_Create()
        {
            int amountBefore = _formRepository.GetAll().Count();

            var form = Common.CreateTestForm();

            Assert.IsNotNull(form);
            Assert.IsTrue(form.ID > 0);
            Assert.AreEqual(Common.TEST_FORM_NAME, form.Name);

            int amountAfter = _formRepository.GetAll().Count();
            Assert.AreEqual(amountAfter, amountBefore + 1);
        }

        [TestMethod]
        public void Form_Remove()
        {
            var form = Common.CreateTestForm();
            int amountBefore = _formRepository.GetAll().Count();

            Assert.IsFalse(form.IsNew);

            _formRepository.Delete(form);

            Assert.IsTrue(form.IsNew);

            int amountAfter = _formRepository.GetAll().Count();
            Assert.AreEqual(amountAfter, amountBefore - 1);
        }
    }
}

using System.Web;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using EAutopay.Forms;

namespace EAutopay.Tests.Integration
{
    [TestClass]
    public class FormTest
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
        public void Form_Create()
        {
            int amountBefore = FormRepository.GetAll().Count();

            var form = Common.CreateTestForm();

            Assert.IsNotNull(form);
            Assert.IsTrue(form.ID > 0);
            Assert.AreEqual(Common.TEST_FORM_NAME, form.Name);

            int amountAfter = FormRepository.GetAll().Count();
            Assert.AreEqual(amountAfter, amountBefore + 1);
        }

        [TestMethod]
        public void Form_Remove()
        {
            var form = Common.CreateTestForm();
            int amountBefore = FormRepository.GetAll().Count();

            form.Delete();

            Assert.IsTrue(form.ID == 0);

            int amountAfter = FormRepository.GetAll().Count();
            Assert.AreEqual(amountAfter, amountBefore - 1);
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;

using EAutopay.Forms;

namespace EAutopay.Tests
{
    [TestClass]
    public class FormTest
    {
        [TestMethod]
        public void IsNew_Should_Be_True_For_Empty_Form()
        {
            var f = new Form();
            Assert.IsTrue(f.IsNew);
        }

        [TestMethod]
        public void ID_Should_Be_Zero_For_Empty_Form()
        {
            var f = new Form();
            Assert.AreEqual(0, f.ID);
        }
    }
}

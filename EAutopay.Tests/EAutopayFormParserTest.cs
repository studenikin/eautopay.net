using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using EAutopay.Parsers;

namespace EAutopay.Tests
{
    [TestClass]
    public class EAutopayFormParserTest
    {
        [TestMethod]
        public void FormParser_Returns_Correct_List_Of_Forms()
        {
            string  source = File.ReadAllText("../../Data/Forms.html");

            var parser = new EAutopayFormParser();
            var forms = parser.ExtractForms(source);

            Assert.AreEqual(2, forms.Count);

            var f = forms.OrderByDescending(form => form.ID).First();

            Assert.AreEqual(25801, f.ID);
            Assert.AreEqual("Форма 2.0", f.Name);
        }
    }
}

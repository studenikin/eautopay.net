using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using EAutopay.Parsers;

namespace EAutopay.Tests
{
    [TestClass]
    public class EAutopayUpsellParserTest
    {
        [TestMethod]
        public void UpsellParser_Returns_Correct_List_Of_Upsells()
        {
            string  source = File.ReadAllText("../../Data/Upsells.html");

            var parser = new EAutopayUpsellParser();
            var upsells = parser.ExtractUpsells(235043, source);

            Assert.AreEqual(1, upsells.Count);

            var u = upsells.First();

            Assert.AreEqual(85308, u.ID);
            Assert.AreEqual(235045, u.OriginID);
            Assert.AreEqual(235043, u.ParentID);
            Assert.AreEqual(700.00, u.Price);
            Assert.AreEqual("http://test-domain.com/1.html", u.SuccessUri);
            Assert.AreEqual("Апселл для Товара 1_UPSELL (физ.) - 700.00 руб", u.Title);
            Assert.AreEqual("http://demo.e-autopay.com/ordering/add_to_order.php?order_id=&lt;?php print $_GET['order_id'];?&gt;&tovar_id=235045", u.ClientUri);
        }

        [TestMethod]
        public void UpsellParser_Returns_Correct_UpsellSettings()
        {
            string source = File.ReadAllText("../../Data/Upsells.html");

            var parser = new EAutopayUpsellParser();
            var settings = parser.ExtractSettings(source);

            Assert.IsTrue(settings.HasProductUpsells);
            Assert.IsTrue(settings.IsUpsellsEnabled);
            Assert.AreEqual(8, settings.Interval);
            Assert.AreEqual("http://test-domain.com/upsell.php", settings.RedirectUri);
        }
    }
}

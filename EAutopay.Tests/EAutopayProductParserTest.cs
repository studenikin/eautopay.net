using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using EAutopay.Parsers;

namespace EAutopay.Tests
{
    [TestClass]
    public class EAutopayProductParserTest
    {
        [TestMethod]
        public void ProductParser_Returns_Correct_List_Of_Products()
        {
            string  source = File.ReadAllText("../../Data/Products.html");

            var parser = new EAutopayProductParser();
            var products = parser.ExtractProducts(source);

            Assert.AreEqual(3, products.Count);

            var p = products
                .Where(prod => !prod.IsUpsell)
                .OrderByDescending(prod => prod.ID)
                .First();

            Assert.AreEqual(235044, p.ID);
            Assert.AreEqual("Товар 2", p.Name);
            Assert.AreEqual(1400.00, p.Price);
        }
    }
}

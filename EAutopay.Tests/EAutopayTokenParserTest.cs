using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using EAutopay.Parsers;

namespace EAutopay.Tests
{
    [TestClass]
    public class EAutopayTokenParserTest
    {
        [TestMethod]
        public void TokenParser_Returns_Correct_Token()
        {
            string  source = File.ReadAllText("../../Data/Login.html");

            var parser = new EAutopayTokenParser();
            var token = parser.ExtractToken(source);

            Assert.AreEqual(token, "kjxJJ5T05H9CkyljvYvKWnwImLPgja3dE1oEciL7");
        }
    }
}

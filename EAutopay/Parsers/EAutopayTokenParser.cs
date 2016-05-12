using HtmlAgilityPack;

namespace EAutopay.Parsers
{
    /// <summary>
    /// Provides secure token over HTTPS.
    /// The token needs to be sent for every request to E-Autopay.
    /// </summary>
    internal class EAutopayTokenParser : ITokenParser
    {
        /// <summary>
        /// Gets secure token residing on the Login page.
        /// </summary>
        /// <param name="source">HTML to be parsed.</param>
        /// <returns>The token as string.</returns>
        public string ExtractToken(string source)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(source);

            var root = htmlDoc.DocumentNode;
            var token = root.SelectSingleNode("//input[@name='_token']");
            return token != null ? token.Attributes["value"].Value : "";
        }
    }

}

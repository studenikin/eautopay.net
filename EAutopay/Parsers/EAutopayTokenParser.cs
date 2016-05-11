using HtmlAgilityPack;

namespace EAutopay.Parsers
{
    /// <summary>
    /// Retrieves useful data from E-Autopay by parsing response data.
    /// </summary>
    internal class EAutopayTokenParser : ITokenParser
    {
        /// <summary>
        /// Gets secure token residing on the Login page.
        /// </summary>
        /// <param name="source">HTML to be parsed.</param>
        /// <returns>Secure token as string.</returns>
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

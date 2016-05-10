using System.Globalization;
using System.Collections.Generic;

using EAutopay.Upsells;

using HtmlAgilityPack;

namespace EAutopay.Parsers
{
    /// <summary>
    /// Retrieves useful data from E-Autopay by parsing response data.
    /// </summary>
    internal class Parser
    {
        string _html;

        /// <summary>
        /// Initializes a new instance of the <see cref="Parser"/> class.
        /// </summary>
        /// <param name="html">HTML (or JSON) to be parsed.</param>
        public Parser(string html)
        {
            _html = html;
        }

        /// <summary>
        /// Gets secure token residing on the Login page.
        /// </summary>
        /// <returns>Secure token as string.</returns>
        public string GetToken()
        {
            var root = GetRootNode(_html);
            var token = root.SelectSingleNode("//input[@name='_token']");
            return token != null ? token.Attributes["value"].Value : "";
        }

        private HtmlNode GetRootNode(string html)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            return htmlDoc.DocumentNode;
        }
    }

}

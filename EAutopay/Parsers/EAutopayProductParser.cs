using System.Globalization;
using System.Collections.Generic;

using EAutopay.Products;

using HtmlAgilityPack;

namespace EAutopay.Parsers
{
    /// <summary>
    /// Extracts the form data out of the "products" page in E-Autopay.
    /// </summary>
    internal class EAutopayProductParser : IProductParser
    {
        /// <summary>
        /// Gets the list of products on the "products" page in E-Autopay
        /// </summary>
        /// <param name="source">Html source to be parsed.</param>
        /// <returns>The list of <see cref="Product"/>.</returns>
        public List<Product> ExtractProducts(string source)
        {
            var ret = new List<Product>();

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(source);

            var root = htmlDoc.DocumentNode;
            var table = root.SelectSingleNode("//table");
            if (table != null)
            {
                var rows = table.SelectNodes("tr[@id]"); // takes TRs without header
                if (rows != null)
                {
                    foreach (var row in rows)
                    {
                        var p = new Product();
                        FillProductDataRow(p, row);
                        ret.Add(p);
                    }
                }
            }
            return ret;
        }

        private void FillProductDataRow(Product p, HtmlNode tr)
        {
            var tds = tr.SelectNodes("td");

            p.ID = int.Parse(tds[1].InnerText.Trim());
            p.Name = tds[2].InnerText.Trim();

            // price comes as: "999.00 руб."
            var price = tds[3].InnerHtml;
            p.Price = double.Parse(price.Substring(0, price.IndexOf(" ")).Trim(), CultureInfo.InvariantCulture);
        }
    }
}

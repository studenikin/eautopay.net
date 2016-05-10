using System.Web.Helpers;
using System.Globalization;
using System.Collections.Generic;

using EAutopay.Products;

using HtmlAgilityPack;

namespace EAutopay.Parsers
{
    internal class EAutopayProductParser : IProductParser
    {
        /// <summary>
        /// Retrieves product ID from the JSON data.
        /// Just after the product has been created and gets redirected to the "Save Price" page.
        /// </summary>
        /// <returns>ID of the product in E-Autopay.</returns>
        public int GetProductID(string source)
        {
            var json = Json.Decode(source);
            var uri = json.redirect.to; // comes as: "/adminka/product/edit/231455/name"

            return int.Parse(uri.Replace("/adminka/product/edit/", "").Replace("/name", ""));
        }

        /// <summary>
        /// Gets the list of products on the "products" page in E-Autopay
        /// </summary>
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

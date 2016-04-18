using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;

using HtmlAgilityPack;


namespace EAutopay.NET
{
    public class Parser
    {
        private string _html;

        public Parser()
        {

        }

        public Parser(string html)
        {
            _html = html;
        }

        public static int GetProductID(string body)
        {
            var regex = new Regex(@"edit\\/(.*?)\\/");
            var matches = regex.Matches(body);
            if (matches.Count > 0)
            {
                return int.Parse(matches[0].Groups[1].Value);
            }
            return 0;
        }

        public static string GetToken(string body)
        {
            Regex regex = new Regex("<input.*name=\"_token\".*?value=\"(.*?)\"");
            foreach (Match m in regex.Matches(body))
            {
                return m.Groups.Count > 1 ? m.Groups[1].Value : "";
            }
            return "";
        }

        public static List<Product> GetProducts(string body)
        {
            var products = new List<Product>();

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(body);
            var root = htmlDoc.DocumentNode;

            var table = root.SelectSingleNode("//table");
            if (table !=null)
            {
                var rows = table.SelectNodes("tr[@id]"); // takes TRs without header
                if (rows != null)
                {
                    foreach (var row in rows)
                    {
                        var p = new Product();
                        FillProduct(p, row);
                        products.Add(p);
                    }
                }
            }

            return products;
        }

        private static void FillProduct(Product p, HtmlNode tr)
        {
            var tds = tr.SelectNodes("td");

            p.ID = int.Parse(tds[1].InnerText.Trim());

            p.Name = tds[2].InnerText.Trim();

            // price comes as: "999.00 руб."
			var  price = tds[3].InnerHtml;
            p.Price = double.Parse(price.Substring(0, price.IndexOf(" ")).Trim(), CultureInfo.InvariantCulture);
        }

        internal List<IFormDataRow> GetFormDataRows()
        {
            var forms = new List<IFormDataRow>();

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(_html);
            var root = htmlDoc.DocumentNode;

            var table = root.SelectSingleNode("//table[@id='table_group_0']");
            if (table != null)
            {
                var rows = table.SelectNodes("tr[@id]");

                if (rows != null)
                {
                    foreach (var tr in rows)
                    {
                        var form = new HtmlFormDataRow();
                        FillFormDataRow(form, tr);
                        forms.Add(form);
                    }
                }
            }
            return forms;
        }

        private void FillFormDataRow(HtmlFormDataRow form, HtmlNode tr)
        {
            var tds = tr.SelectNodes("td");
            form.ID = int.Parse(tds[0].InnerText.Trim());
            form.Name = tds[2].InnerText.Trim();
        }
    }

}

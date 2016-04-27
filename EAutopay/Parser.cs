using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;

using HtmlAgilityPack;


namespace EAutopay
{
    internal class Parser
    {
        private string _html;

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

        public string GetToken()
        {
            var root = GetRootNode(_html);
            var token = root.SelectSingleNode("//input[@name='_token']");
            return token != null ? token.Attributes["value"].Value : "";
        }

        public List<IProductDataRow> GetProductDataRows()
        {
            var products = new List<IProductDataRow>();

            var root = GetRootNode(_html);
            var table = root.SelectSingleNode("//table");
            if (table !=null)
            {
                var rows = table.SelectNodes("tr[@id]"); // takes TRs without header
                if (rows != null)
                {
                    foreach (var row in rows)
                    {
                        var p = new HtmlProductDataRow();
                        FillProductDataRow(p, row);
                        products.Add(p);
                    }
                }
            }
            return products;
        }

        private void FillProductDataRow(IProductDataRow p, HtmlNode tr)
        {
            var tds = tr.SelectNodes("td");

            p.ID = int.Parse(tds[1].InnerText.Trim());
            p.Name = tds[2].InnerText.Trim();

            // price comes as: "999.00 руб."
			var  price = tds[3].InnerHtml;
            p.Price = double.Parse(price.Substring(0, price.IndexOf(" ")).Trim(), CultureInfo.InvariantCulture);
        }

        public List<IFormDataRow> GetFormDataRows()
        {
            var forms = new List<IFormDataRow>();

            var root = GetRootNode(_html);
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

        private void FillFormDataRow(IFormDataRow form, HtmlNode tr)
        {
            var tds = tr.SelectNodes("td");
            form.ID = int.Parse(tds[0].InnerText.Trim());
            form.Name = tds[2].InnerText.Trim();
        }

        private HtmlNode GetRootNode(string html)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            return htmlDoc.DocumentNode;
        }
    }

}

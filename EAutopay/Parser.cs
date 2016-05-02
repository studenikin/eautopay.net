using System.Collections.Generic;
using System.Globalization;
using System.Web.Helpers;

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
        /// <summary>
        /// Retrieves product id from the json source.
        /// Just after the product has been created and gets redirected to the "Save Price" page.
        /// </summary>
        /// <returns>Product ID as integer.</returns>
        public int GetProductID()
        {
            var json = Json.Decode(_html);
            var uri = json.redirect.to; // comes as: "/adminka/product/edit/231455/name"

            return int.Parse(uri.Replace("/adminka/product/edit/", "").Replace("/name", ""));
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

        /// <summary>
        /// Gets upsell settings of particular product in E-Autopay. 
        /// </summary>
        /// <returns>UpsellSettings object.</returns>
        public UpsellSettings GetUpsellSettings()
        {
            var ret = new UpsellSettings();
            var root = GetRootNode(_html);

            var checkbox = root.SelectSingleNode("//input[@name='additional_tovar_offer'][@type='checkbox']");
            if (checkbox.Attributes["checked"] != null)
            {
                ret.IsUpsellsEnabled = checkbox.Attributes["checked"].Value == "checked";
            }

            var interval = root.SelectSingleNode("//input[@name='time_for_add']");
            ret.Interval = int.Parse(interval.Attributes["value"].Value);

            var page = root.SelectSingleNode("//input[@name='additional_tovar_page_offer']");
            ret.RedirectUri = page.Attributes["value"].Value;

            ret.HasProductUpsells = root.InnerHtml.IndexOf("&tovar_id=") > -1;

            return ret;
        }
    }

}

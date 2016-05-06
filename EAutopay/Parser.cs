using System.Collections.Generic;
using System.Globalization;
using System.Web.Helpers;

using EAutopay.Forms;
using EAutopay.Products;
using EAutopay.Upsells;

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

        public List<Product> GetProducts()
        {
            var products = new List<Product>();

            var root = GetRootNode(_html);
            var table = root.SelectSingleNode("//table");
            if (table !=null)
            {
                var rows = table.SelectNodes("tr[@id]"); // takes TRs without header
                if (rows != null)
                {
                    foreach (var row in rows)
                    {
                        var p = new Product();
                        FillProductDataRow(p, row);
                        products.Add(p);
                    }
                }
            }
            return products;
        }

        private void FillProductDataRow(Product p, HtmlNode tr)
        {
            var tds = tr.SelectNodes("td");

            p.ID = int.Parse(tds[1].InnerText.Trim());
            p.Name = tds[2].InnerText.Trim();

            // price comes as: "999.00 руб."
			var  price = tds[3].InnerHtml;
            p.Price = double.Parse(price.Substring(0, price.IndexOf(" ")).Trim(), CultureInfo.InvariantCulture);
        }

        public List<Form> GetForms()
        {
            var forms = new List<Form>();

            var root = GetRootNode(_html);
            var table = root.SelectSingleNode("//table[@id='table_group_0']");
            if (table != null)
            {
                var rows = table.SelectNodes("tr[@id]");

                if (rows != null)
                {
                    foreach (var tr in rows)
                    {
                        var form = new Form();
                        FillFormDataRow(form, tr);
                        forms.Add(form);
                    }
                }
            }
            return forms;
        }

        private void FillFormDataRow(Form form, HtmlNode tr)
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

        /// <summary>
        /// Parses page and returns list of upsells for specified product.
        /// </summary>
        /// <param name="productId">Product to get upsells for.</param>
        /// <returns>List of upsells.</returns>
        public List<Upsell> GetUpsells(int productId)
        {
            var ret = new List<Upsell>();

            var root = GetRootNode(_html);
            var table = root.SelectSingleNode("//div[@id='upsell-form-list']");
            if (table != null)
            {
                var rows = table.SelectNodes("div[@class='tr upsell-rest-deleted']");
                if (rows != null)
                {
                    foreach (var row in rows)
                    {
                        var upsell = new Upsell();
                        upsell.ParentID = productId;
                        FillUpsellDataRow(upsell, row);
                        ret.Add(upsell);
                    }
                }
            }
            return ret;
        }

        private void FillUpsellDataRow(Upsell upsell, HtmlNode tr)
        {
            var tds = tr.SelectNodes("span");

            upsell.Title = tds[0].InnerText.Trim();
            upsell.SuccessUri = tds[2].SelectSingleNode("a").InnerText.Trim();
            upsell.ClientUri = tds[3].InnerText.Trim();

            // price comes as: "999.00&nbsp;руб"
            var price = tds[1].InnerText;
            upsell.Price = double.Parse(price.Substring(0, price.IndexOf("&")).Trim(), CultureInfo.InvariantCulture);

            // link comes as: "/adminka/product/233318/upsell/84604"
            var actionLink = tds[5].SelectNodes("div/button")[1].Attributes["data-action"].Value;
            upsell.ID = int.Parse(actionLink.Substring(actionLink.LastIndexOf("/") + 1));

            //client uri comes as: "?order_id=<?php print $_GET['order_id'];?>&tovar_id=233339"
            upsell.OriginID = int.Parse(upsell.ClientUri.Substring(upsell.ClientUri.LastIndexOf("=") + 1));
        }
    }

}

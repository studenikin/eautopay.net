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

        /// <summary>
        /// Gets upsell settings of particular product in E-Autopay. 
        /// </summary>
        /// <returns>A <see cref="UpsellSettings"/>.</returns>
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
        /// Gets the list of upsells for the specified product on the "edit product" page in E-Autopay.
        /// </summary>
        /// <param name="productId">Product to get upsells for.</param>
        /// <returns>The list of <see cref="Upsell"/>.</returns>
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

        private HtmlNode GetRootNode(string html)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            return htmlDoc.DocumentNode;
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

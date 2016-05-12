using System.Globalization;
using System.Collections.Generic;

using EAutopay.Upsells;

using HtmlAgilityPack;

namespace EAutopay.Parsers
{
    /// <summary>
    /// Extracts the upsell data out of the "upsells" page in E-Autopay.
    /// </summary>
    internal class EAutopayUpsellParser : IUpsellParser
    {
        /// <summary>
        /// Gets upsell settings of particular product in E-Autopay. 
        /// </summary>
        /// <param name="source">Html source to be parsed.</param>
        /// <returns>A <see cref="UpsellSettings"/>.</returns>
        public UpsellSettings ExtractSettings(string source)
        {
            var ret = new UpsellSettings();

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(source);
            var root = htmlDoc.DocumentNode;

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
        /// <param name="source">Html source to be parsed.</param>
        /// <returns>The list of <see cref="Upsell"/>.</returns>
        public List<Upsell> ExtractUpsells(int productId, string source)
        {
            var ret = new List<Upsell>();

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(source);

            var root = htmlDoc.DocumentNode;
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

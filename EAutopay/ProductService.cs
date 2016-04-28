using System.Collections.Specialized;

namespace EAutopay
{
    /// <summary>
    /// Encapsulates useful methods for E-Autopay product.
    /// </summary>
    public class ProductService
    {
        private ProductService()
        { }

        /// <summary>
        /// Tells E-Autopay that the product can have upsells.
        /// </summary>
        /// <param name="p">Product to enable upsells for.</param>
        public static void EnableUpsells(Product p)
        {
            ToggleUpsells(p, true);
        }

        /// <summary>
        /// Tells E-Autopay that the product is disabled for upsells.
        /// </summary>
        /// <param name="p">Product to disable upsells for.</param>
        public static void DisableUpsells(Product p)
        {
            ToggleUpsells(p, false);
        }

        /// <summary>
        /// Toggles ability to have upsells.
        /// </summary>
        /// <param name="enable">True if product can have upsells. False - otherwise.</param>
        private static void ToggleUpsells(Product p, bool enable)
        {
            var paramz = new NameValueCollection
            {
                {"tovar_type", "1"},
                {"edit[]", "sendsettings"},
                {"pay_nal", "0"},
                {"confirm_required", "0"},
                {"nal_pdt_url", ""},
                {"nal_ok_url", ""},
                {"nal_countries[]", "Россия"},
                {"additional_tovar_offer", enable ? "1" : "0"},
                {"time_for_add", Config.UPSELL_INTERVAL},
                {"no_multi_upsells", "0"},
                {"upsell_error_url", ""},
                {"additional_tovar_page_offer", Config.GetUpsellPageURI()},
                {"product_id", p.ID.ToString()}
            };

            var crawler = new Crawler();
            using (var resp = crawler.HttpPost(Config.URI_SAVE_PRODUCT, paramz)) { }
        }

        /// <summary>
        /// Adds upsell to the list of upsells of specified product.
        /// </summary>
        public static void BindUpsell(Product product, Product upsell)
        {
            var paramz = new NameValueCollection
            {
                {"tovar_type", "0"},
                {"action", "create"},
                {"commission[0][commission]", "0"},
                {"commission[0][currency]", "руб"},
                {"not_pay_commission", "0"},
                {"additional_tovar_id", upsell.ID.ToString()},
                {"additional_tovar_price", upsell.PriceInvariant},
                {"success_page", Config.GetUpsellSuccessPage()}
            };

            var crawler = new Crawler();
            using (var resp = crawler.HttpPost(Config.GetUpsellURI(product.ID), paramz)) { }
        }
    }
}

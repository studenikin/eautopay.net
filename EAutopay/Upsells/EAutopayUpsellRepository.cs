using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

using EAutopay.Parsers;
using EAutopay.Products;

namespace EAutopay.Upsells
{
    /// <summary>
    /// Provides CRUD operations for upsells in E-Autopay.
    /// </summary>
    public class EAutopayUpsellRepository : IUpsellRepository
    {
        readonly IConfiguration _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="EAutopayUpsellRepository"/> class.
        /// </summary>
        public EAutopayUpsellRepository() : this(null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EAutopayUpsellRepository"/> class.
        /// </summary>
        /// <param name="config">General E-Autopay settings.</param>
        public EAutopayUpsellRepository(IConfiguration config)
        {
            _config = config ?? new AppConfig();
        }

        /// <summary>
        /// Determines whether the specified product has an upsell(s) in E-Autopay.
        /// </summary>
        /// <param name="p">The <see cref="Product"/> to be checked.</param>
        /// <returns>true if the product doesn't have an upsell(s); otherwise, false.</returns>
        public bool HasUpsell(Product p)
        {
            return GetByProduct(p.ID).Count > 0;
        }

        /// <summary>
        /// Gets upsells for the specified product in E-Autopay.
        /// </summary>
        /// <param name="productId"><see cref="Product"/> ID.</param>
        /// <returns>The list of <see cref="Upsell"/>.</returns>
        public List<Upsell> GetByProduct(int productId)
        {
            var crawler = new Crawler();
            var up = new UriProvider(_config.Login);

            var resp = crawler.Get(up.GetSendSettingsUri(productId));
            
            var parser = new Parser(resp.Data);
            return parser.GetUpsells(productId);
        }

        /// <summary>
        /// Gets the specified upsell for the specified product.
        /// </summary>
        /// <param name="id"><see cref="Upsell"/> ID.</param>
        /// <param name="productId"><see cref="Product"/> ID.</param>
        /// <returns>An <see cref="Upsell"/>.</returns>
        public Upsell Get(int id, int productId)
        {
            return GetByProduct(productId).Where(u => u.ID == id).FirstOrDefault();
        }

        /// <summary>
        /// Creates a new upsell in E-Autopay; or updates existing one.
        /// </summary>
        /// <param name="upsell"><see cref="Upsell"/> to be created/updated.</param>
        /// <param name="productId"><see cref="Product"/> ID.</param>
        /// <returns><see cref="Upsell"/> ID.</returns>
        public int Save(Upsell upsell, int productId)
        {
            BindUpsell(upsell, productId);

            EnableUpsells(productId);

            if (upsell.IsNew)
            { 
                var lastUpsell = GetNewest(productId);
                upsell.ID = lastUpsell != null ? lastUpsell.ID : 0;
            }

            upsell.ParentID = productId;
            return upsell.ID;
        }

        /// <summary>
        /// Deletes the specified upsell from E-Autopay.
        /// </summary>
        /// <param name="upsell"><see cref="Upsell"/> to be deleted.</param>
        public void Delete(Upsell upsell)
        {
            if (upsell.IsNew) return;

            RemoveFromEAutopay(upsell);

            DisableIfNoProductLeft(upsell.ParentID);

            ResetValues(upsell);
        }

        /// <summary>
        /// Deletes all upsells for the specified product.
        /// </summary>
        /// <param name="p"><see cref="Upsell"/> to remove upsells from.</param>
        public void DeleteByProduct(Product p)
        {
            var upsells = GetByProduct(p.ID);
            foreach (var upsell in upsells)
            {
                Delete(upsell);
            }
        }

        /// <summary>
        /// Returns latest created upsell.
        /// </summary>
        private Upsell GetNewest(int productId)
        {
            var upsells = GetByProduct(productId);
            return upsells.OrderByDescending(u => u.ID).FirstOrDefault();
        }

        /// <summary>
        /// Adds upsell to the list of upsells of specified product.
        /// </summary>
        private void BindUpsell(Upsell upsell, int productId)
        {
            var paramz = new NameValueCollection
            {
                {"tovar_type", "0"},
                {"action", upsell.IsNew ? "create" : "put"},
                {"commission[0][commission]", "0"},
                {"commission[0][currency]", "руб"},
                {"not_pay_commission", "0"},
                {"additional_tovar_id", upsell.OriginID.ToString()},
                {"additional_tovar_price", upsell.PriceInvariant},
                {"success_page", upsell.SuccessUri}
            };

            var crawler = new Crawler();
            var up = new UriProvider(_config.Login);
            crawler.Post(up.GetUpsellUri(productId, 0), paramz);
        }

        /// <summary>
        /// Tells E-Autopay that the product can have upsells.
        /// </summary>
        /// <param name="productId">ID of the product to enable upsells for.</param>
        private void EnableUpsells(int productId)
        {
            ToggleUpsells(productId, true);
        }

        /// <summary>
        /// Tells E-Autopay that the product is disabled for upsells.
        /// </summary>
        /// <param name="productId">ID of the product to disable upsells for.</param>
        private void DisableUpsells(int productId)
        {
            ToggleUpsells(productId, false);
        }

        /// <summary>
        /// Toggles ability to have upsells.
        /// </summary>
        /// <param name="productId">ID of the product to toggle.</param>
        /// <param name="enable">True if product can have upsells. False - otherwise.</param>
        private void ToggleUpsells(int productId, bool enable)
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
                {"time_for_add", _config.UpsellInterval.ToString()},
                {"no_multi_upsells", "0"},
                {"upsell_error_url", ""},
                {"additional_tovar_page_offer", _config.UpsellLandingPage},
                {"product_id", productId.ToString()}
            };

            var crawler = new Crawler();
            var up = new UriProvider(_config.Login);
            crawler.Post(up.ProductSaveUri, paramz);
        }

        /// <summary>
        /// Disables the "can have upsells" checkbox if the product has no upsells left.
        /// </summary>
        private void DisableIfNoProductLeft(int productId)
        {
            var leftUpsells = GetByProduct(productId);
            if (leftUpsells.Count == 0)
            {
                DisableUpsells(productId);
            }
        }

        private void RemoveFromEAutopay(Upsell upsell)
        {
            var paramz = new NameValueCollection
            {
                {"_method", "DELETE"}
            };

            var crawler = new Crawler();
            var up = new UriProvider(_config.Login);
            crawler.Post(up.GetUpsellUri(upsell.ParentID, upsell.ID), paramz);
        }

        private void ResetValues(Upsell upsell)
        {
            upsell.ID = 0;
            upsell.ParentID = 0;
            upsell.OriginID = 0;
            upsell.Price = 0.0;
            upsell.SuccessUri = string.Empty;
            upsell.ClientUri = string.Empty;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

using EAutopay.Parsers;

namespace EAutopay.Upsells
{
    /// <summary>
    /// Provides CRUD operations for upsells in E-Autopay.
    /// </summary>
    public class EAutopayUpsellRepository : IUpsellRepository
    {
        readonly IConfiguration _config;

        readonly ICrawler _crawler;

        readonly IUpsellParser _parser;

        /// <summary>
        /// Initializes a new instance of the <see cref="EAutopayUpsellRepository"/> class.
        /// </summary>
        public EAutopayUpsellRepository() : this(null, null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EAutopayUpsellRepository"/> class.
        /// </summary>
        /// <param name="config">General E-Autopay settings.</param>
        /// <param name="crawler"><see cref="ICrawler"/> to make HTTP requests to E-Autopay.</param>
        /// <param name="parser"><see cref="IFormParser"/> to parse response delivered by the crawler.</param>
        public EAutopayUpsellRepository(IConfiguration config, ICrawler crawler, IUpsellParser parser)
        {
            _config = config ?? new AppConfig();
            _crawler = crawler ?? new Crawler();
            _parser = parser ?? new EAutopayUpsellParser();
        }

        /// <summary>
        /// Determines whether the specified product has an upsell(s) in E-Autopay.
        /// </summary>
        /// <param name="productId">The ID of the product to be checked.</param>
        /// <returns>true if the product doesn't have an upsell(s); otherwise, false.</returns>
        public bool HasUpsell(int productId)
        {
            return GetByProduct(productId).Count > 0;
        }

        /// <summary>
        /// Gets upsells for the specified product in E-Autopay.
        /// </summary>
        /// <param name="productId"><see cref="Product"/> ID.</param>
        /// <returns>The list of <see cref="Upsell"/>.</returns>
        public List<Upsell> GetByProduct(int productId)
        {
            var up = new UriProvider(_config.Login);
            var resp = _crawler.Get(up.GetSendSettingsUri(productId));
            return _parser.ExtractUpsells(productId, resp.Data);
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
        /// <exception cref="ArgumentException">Thrown when OriginID is less (or equals) than zero.</exception>
        public int Save(Upsell upsell, int productId)
        {
            if (upsell.OriginID <= 0) throw new ArgumentException(null, "OriginID");
            
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
        /// <param name="productId">ID of the product to remove upsells from.</param>
        public void DeleteByProduct(int productId)
        {
            var upsells = GetByProduct(productId);
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

            var up = new UriProvider(_config.Login);
            _crawler.Post(up.GetUpsellUri(productId, 0), paramz);
        }

        /// <summary>
        /// Tells E-Autopay that the product can have upsells.
        /// </summary>
        /// <param name="productId">ID of the product to enable upsells for.</param>
        internal void EnableUpsells(int productId)
        {
            ToggleUpsells(productId, true);
        }

        /// <summary>
        /// Tells E-Autopay that the product is disabled for upsells.
        /// </summary>
        /// <param name="productId">ID of the product to disable upsells for.</param>
        internal void DisableUpsells(int productId)
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
                {"time_for_add", GetInterval()},
                {"no_multi_upsells", "0"},
                {"upsell_error_url", ""},
                {"additional_tovar_page_offer", _config.UpsellLandingPage},
                {"product_id", productId.ToString()}
            };

            var up = new UriProvider(_config.Login);
            _crawler.Post(up.ProductSaveUri, paramz);
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

            var up = new UriProvider(_config.Login);
            _crawler.Post(up.GetUpsellUri(upsell.ParentID, upsell.ID), paramz);
        }

        private void ResetValues(Upsell upsell)
        {
            upsell.ID = 0;
            upsell.ParentID = 0;
            upsell.OriginID = 0;
            upsell.Price = 0.0;
            upsell.Title = string.Empty;
            upsell.SuccessUri = string.Empty;
            upsell.ClientUri = string.Empty;
        }

        private string GetInterval()
        {
            int ret;
            int.TryParse(_config.UpsellInterval.ToString(), out ret);
            return ret > 0 ? ret.ToString() : "20";
        }
    }
}

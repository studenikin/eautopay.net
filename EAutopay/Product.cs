using System;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace EAutopay
{
    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public string PriceFormatted
        {
            get
            {
                return Price.ToString("C");
            }
        }

        public string PriceInvariant
        {
            get
            {
                return Price.ToString("F", CultureInfo.InvariantCulture); // format: 999.50
            }
        }

        public bool IsUpsell
        {
            get
            {
                if (!string.IsNullOrEmpty(Name))
                {
                    return Name.ToUpper().Contains(Config.UPSELL_SUFFIX.ToUpper());
                }
                return false;
            }
        }

        private bool IsNew
        {
            get { return ID == 0; }
            set { ID = 0; }
        }

        public Product() { }


        /// <summary>
        /// Updates product or creates new one if ID equals zero.
        /// </summary>
        /// <returns>Product ID.</returns>
        public int Save()
        {
            var paramz = new NameValueCollection
            {
                {"tovar_type", "1"},
                {"edit[]", "name"},
                {"category_id", "0"},
                {"site_url", ""},
                {"author_fio", ""},
                {"name", Name},
                {"product_id", ID.ToString().Equals("0") ? "" : ID.ToString()}
            };

            var poster = new Poster();
            using (var resp = poster.HttpPost(Config.URI_SAVE_PRODUCT, paramz))
            {
                if (IsNew)
                {
                    var reader = new StreamReader(resp.GetResponseStream());
                    ID = Parser.GetProductID(reader.ReadToEnd());
                }
                SetPrice();
                return ID;
            }
        }

        private string GetNameForUpsell()
        {
            return string.Format("{0}_{1}_{2}", Name, Config.UPSELL_SUFFIX, ID);
        }

        /// <summary>
        /// Returns True if the product is a parent for specified product.
        /// </summary>
        internal bool IsParentFor(Product product)
        {
            return GetNameForUpsell().Equals(product.Name);
        }

        /// <summary>
        /// Returns True if the product is a child for specified product.
        /// </summary>
        internal bool IsChildOf(Product product)
        {
            return IsUpsell && Name.Equals(product.GetNameForUpsell());
        }

        private void SetPrice()
        {
            var paramz = new NameValueCollection
            {
                {"tovar_type", "1"},
                {"edit[]", "price"},
                {"currency", "руб"},
                {"is_any_price", "0"},
                {"summ_rashod", "0.00"},
                {"product_id", ID.ToString()},
                {"price1", PriceInvariant}
            };

            var poster = new Poster();
            using (var resp = poster.HttpPost(Config.URI_SAVE_PRODUCT, paramz)) { }
        }


        public bool HasUpsell()
        {
            return ProductFactory.GetUpsell(this) != null;
        }

        /// <summary>
        /// Adds upsell to the product.
        /// </summary>
        /// <param name="price">Upsell price.</param>
        /// <returns>Reference to the upsell created.</returns>
        public Product AddUpsell(double price)
        {
            EnableUpsells();

            Product upsell = ProductFactory.CreateCopy(this);
            upsell.IsNew = true;
            upsell.Name = GetNameForUpsell();
            upsell.Price = price;
            upsell.Save();

            BindUpsell(upsell);
            return upsell;
        }

        /// <summary>
        /// Tells E-Autopay that the product can have upsells.
        /// </summary>
        private void EnableUpsells()
        {
            ToggleUpsells(true);
        }

        /// <summary>
        /// Tells E-Autopay that the product is disabled for upsells.
        /// </summary>
        private void DisableUpsells()
        {
            ToggleUpsells(false);
        }

        /// <summary>
        /// Toggles ability to have upsells.
        /// </summary>
        /// <param name="enable">True if product can have upsells. False - otherwise.</param>
        private void ToggleUpsells(bool enable)
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
                {"product_id", ID.ToString()}
            };

            var poster = new Poster();
            using (var resp = poster.HttpPost(Config.URI_SAVE_PRODUCT, paramz)) { }
        }

        /// <summary>
        /// Adds upsell to the list of upsells of main product.
        /// </summary>
        private void BindUpsell(Product upsell)
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

            var poster = new Poster();
            using (var resp = poster.HttpPost(Config.GetUpsellURI(ID), paramz)) { }
        }

        /// <summary>
        /// Removes product and corresponding upsell (if any).
        /// </summary>
        public void Delete()
        {
            if (IsNew) return;

            if (HasUpsell())
            {
                RemoveUpsell();
            }
            else if (IsUpsell)
            {
                var parent = ProductFactory.GetProductByUpsell(this);
                if (parent != null)
                {
                    parent.DisableUpsells();
                }
            }

            Remove();
            IsNew = true;
        }

        /// <summary>
        /// Removes upsell of the product.
        /// </summary>
        private void RemoveUpsell()
        {
            var upsell = ProductFactory.GetUpsell(this);
            if (upsell != null)
            {
                upsell.Remove();
            }
        }


        /// <summary>
        /// Removes product in E-Autopay.
        /// </summary>
        private void Remove()
        {
            var paramz = new NameValueCollection 
            {
                {"id", ID.ToString()}
            };

            var poster = new Poster();
            using (var resp = poster.HttpGet(Config.URI_DELETE_PRODUCT, paramz)) { }
        }
    }
}

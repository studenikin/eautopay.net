using System.IO;
using System.Globalization;
using System.Collections.Specialized;

namespace EAutopay
{
    public class Product
    {
        public int ID { get; private set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public string PriceFormatted
        {
            get { return Price.ToString("C");}
        }

        public string PriceInvariant
        {
            get { return Price.ToString("F", CultureInfo.InvariantCulture); /* format: 999.50 */ }
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

            var crawler = new Crawler();
            using (var resp = crawler.HttpPost(Config.URI_SAVE_PRODUCT, paramz))
            {
                if (IsNew)
                {
                    var reader = new StreamReader(resp.GetResponseStream());
                    var parser = new Parser(reader.ReadToEnd());
                    ID = parser.GetProductID();
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

            var crawler = new Crawler();
            using (var resp = crawler.HttpPost(Config.URI_SAVE_PRODUCT, paramz)) { }
        }

        /// <summary>
        /// Returns True if the product has at least one upsell. Otherwise - False.
        /// </summary>
        /// <returns></returns>
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
            ProductService.EnableUpsells(this);

            Product upsell = ProductFactory.CreateCopy(this);
            upsell.IsNew = true;
            upsell.Name = GetNameForUpsell();
            upsell.Price = price;
            upsell.Save();

            ProductService.BindUpsell(this, upsell);
            return upsell;
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
                    ProductService.DisableUpsells(parent);
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

            var crawler = new Crawler();
            using (var resp = crawler.HttpGet(Config.URI_DELETE_PRODUCT, paramz)) { }
        }

        internal void Fill(IProductDataRow dr)
        {
            ID = dr.ID;
            Name = dr.Name;
            Price = dr.Price;
        }
    }
}

using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace EAutopay.Products
{
    /// <summary>
    /// Provides CRUD operations for products in E-Autopay.
    /// </summary>
    public class EAutopayProductRepository : IProductRepository
    {
        readonly IConfiguration _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="EAutopayProductRepository"/> class.
        /// </summary>
        public EAutopayProductRepository() : this(null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EAutopayProductRepository"/> class.
        /// </summary>
        /// <param name="config">General E-Autopay settings.</param>
        public EAutopayProductRepository(IConfiguration config)
        {
            _config = config ?? new AppConfig();
        }

        /// <summary>
        /// Gets the product in E-Autopay for the specified ID.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>A <see cref="Product"/></returns>
        public Product Get(int id)
        {
            var allProducts = GetAll();
            return allProducts.Where(p => p.ID == id).FirstOrDefault();
        }

        /// <summary>
        /// Gets all products in E-Autopay.
        /// </summary>
        /// <returns>The list of <see cref="Product"/>.</returns>
        public List<Product> GetAll()
        {
            var crawler = new Crawler();
            var up = new UriProvider(_config.Login);

            using (var resp = crawler.HttpGet(up.ProductListUri))
            {
                var reader = new StreamReader(resp.GetResponseStream());
                var parser = new Parser(reader.ReadToEnd());
                return parser.GetProducts();
            }
        }

        /// <summary>
        /// Creates a new product in E-Autopay; or updates existing one.
        /// </summary>
        /// <param name="p"><see cref="Product"/> to be updated/created.</param>
        /// <param name="isForUpsell">Indicates whether a specified product is an upsell.</param>
        /// <returns><see cref="Product"/> ID.</returns>
        public int Save(Product p, bool isForUpsell)
        {
            var paramz = new NameValueCollection
            {
                {"tovar_type", "1"},
                {"edit[]", "name"},
                {"category_id", "0"},
                {"site_url", ""},
                {"author_fio", ""},
                {"name", isForUpsell ? GetNameForUpsell(p.Name) : p.Name},
                {"product_id", p.ID.ToString().Equals("0") ? "" : p.ID.ToString()}
            };
            var crawler = new Crawler();
            var up = new UriProvider(_config.Login);

            using (var resp = crawler.HttpPost(up.ProductSaveUri, paramz))
            {
                if (p.IsNew)
                {
                    var reader = new StreamReader(resp.GetResponseStream());
                    var parser = new Parser(reader.ReadToEnd());
                    p.ID = parser.GetProductID();
                }
                SetPrice(p);
                return p.ID;
            }
        }

        /// <summary>
        /// Deletes the specified product from E-Autopay.
        /// </summary>
        /// <param name="p"><see cref="Product"/> to be removed.</param>
        public void Delete(Product p)
        {
            if (p.IsNew) return;

            RemoveFromEAutopay(p);
            ResetValues(p);
        }

        private void SetPrice(Product p)
        {
            var paramz = new NameValueCollection
            {
                {"tovar_type", "1"},
                {"edit[]", "price"},
                {"currency", "руб"},
                {"is_any_price", "0"},
                {"summ_rashod", "0.00"},
                {"product_id", p.ID.ToString()},
                {"price1", p.PriceInvariant}
            };
            var crawler = new Crawler();
            var up = new UriProvider(_config.Login);

            using (var resp = crawler.HttpPost(up.ProductSaveUri, paramz)) { }
        }

        private void RemoveFromEAutopay(Product p)
        {
            var paramz = new NameValueCollection
            {
                {"id", p.ID.ToString()}
            };

            var crawler = new Crawler();
            var up = new UriProvider(_config.Login);

            using (var resp = crawler.HttpGet(up.ProductDeleteUri, paramz)) { }
        }

        private void ResetValues(Product p)
        {
            p.ID = 0;
            p.Name = string.Empty;
            p.Price = 0.0;
        }

        private string GetNameForUpsell(string name)
        {
            return string.Format("{0}_{1}", name, Product.UPSELL_SUFFIX);
        }
    }
}
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;

using EAutopay.Parsers;

namespace EAutopay.Products
{
    /// <summary>
    /// Provides CRUD operations for products in E-Autopay.
    /// </summary>
    public class EAutopayProductRepository : IProductRepository
    {
        readonly IConfiguration _config;

        readonly IProductParser _parser;

        readonly ICrawler _crawler;

        /// <summary>
        /// Initializes a new instance of the <see cref="EAutopayProductRepository"/> class.
        /// </summary>
        public EAutopayProductRepository() : this(null, null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EAutopayProductRepository"/> class.
        /// </summary>
        /// <param name="config">General E-Autopay settings.</param>
        /// <param name="crawler"><see cref="ICrawler"/> to make HTTP requests to E-Autopay.</param>
        /// <param name="parser"><see cref="IProductParser"/> to parse response delivered by the crawler.</param>
        public EAutopayProductRepository(IConfiguration config, ICrawler crawler, IProductParser parser)
        {
            _config = config ?? new AppConfig();
            _crawler = crawler ?? new Crawler();
            _parser = parser ?? new EAutopayProductParser();
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
            var up = new UriProvider(_config.Login);
            var resp = _crawler.Get(up.ProductListUri, null);
            return _parser.ExtractProducts(resp.Data);
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
            var up = new UriProvider(_config.Login);
            _crawler.Post(up.ProductSaveUri, paramz);

            if (p.IsNew)
            {
                p.ID = GetLatestProductID();
            }
            SetPrice(p);

            return p.ID;
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
            var up = new UriProvider(_config.Login);
            _crawler.Post(up.ProductSaveUri, paramz);
        }

        private void RemoveFromEAutopay(Product p)
        {
            var paramz = new NameValueCollection
            {
                {"id", p.ID.ToString()}
            };
            var up = new UriProvider(_config.Login);
            _crawler.Get(up.ProductDeleteUri, paramz);
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

        private int GetLatestProductID()
        {
            var latest = GetAll()
                .OrderByDescending(p => p.ID)
                .FirstOrDefault();

            return latest != null ? latest.ID : 0;
        }
    }
}
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace EAutopay.Products
{
    public class EAutopayProductRepository : IProductRepository
    {
        IConfiguration _config;

        public EAutopayProductRepository() : this(null)
        { }

        public EAutopayProductRepository(IConfiguration config)
        {
            _config = config ?? new EAutopayConfig();
        }

        /// <summary>
        /// Returns product with specified ID.
        /// </summary>
        /// <param name="id">Product ID.</param>
        /// <returns>Product object.</returns>
        public Product Get(int id)
        {
            var allProducts = GetAll();
            return allProducts.Where(p => p.ID == id).FirstOrDefault();
        }

        /// <summary>
        /// Returns all products existing in E-Autopay for specified account.
        /// </summary>
        /// <returns></returns>
        public List<Product> GetAll()
        {
            var crawler = new Crawler();
            using (var resp = crawler.HttpGet(_config.ProductListUri))
            {
                var reader = new StreamReader(resp.GetResponseStream());
                var parser = new Parser(reader.ReadToEnd());
                return parser.GetProducts();
            }
        }

        /// <summary>
        /// Gets upsell of specified product.
        /// </summary>
        /// <param name="product">Product object to get upsell for.</param>
        /// <returns>Product object representing upsell.</returns>
        public Product GetUpsell(Product product)
        {
            var allProducts = GetAll();
            return allProducts.Where(upsell => upsell.IsChildOf(product)).FirstOrDefault();
        }

        /// <summary>
        /// Returns parent product for specified upsell.
        /// </summary>
        public Product GetByUpsell(Product upsell)
        {
            var allProducts = GetAll();
            return allProducts.Where(p => p.IsParentFor(upsell)).FirstOrDefault();
        }

        /// <summary>
        /// Removes product in E-Autopay.
        /// </summary>
        public void Remove(Product p)
        {
            var paramz = new NameValueCollection
            {
                {"id", p.ID.ToString()}
            };

            var crawler = new Crawler();
            using (var resp = crawler.HttpGet(_config.ProductDeleteUri, paramz)) { }
        }

        /// <summary>
        /// Updates product or creates new one if ID equals zero.
        /// </summary>
        /// <returns>Product ID.</returns>
        public int Save(Product p)
        {
            var paramz = new NameValueCollection
            {
                {"tovar_type", "1"},
                {"edit[]", "name"},
                {"category_id", "0"},
                {"site_url", ""},
                {"author_fio", ""},
                {"name", p.Name},
                {"product_id", p.ID.ToString().Equals("0") ? "" : p.ID.ToString()}
            };
            var crawler = new Crawler();
            using (var resp = crawler.HttpPost(_config.ProductSaveUri, paramz))
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
            using (var resp = crawler.HttpPost(_config.ProductSaveUri, paramz)) { }
        }
    }
}
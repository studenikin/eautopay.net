using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace EAutopay
{
    public class ProductFactory
    {
        private ProductFactory() { }

        /// <summary>
        /// Returns product with specified ID.
        /// </summary>
        /// <param name="id">Product ID.</param>
        /// <returns>Product object.</returns>
        public static Product Get(int id)
        {
            var allProducts = GetAll();
            return allProducts.Where(p => p.ID == id).FirstOrDefault();
        }

        /// <summary>
        /// Returns all products existing in E-Autopay for specified account.
        /// </summary>
        /// <returns></returns>
        public static List<Product> GetAll()
        {
            var crawler = new Crawler();
            using (var resp = crawler.HttpGet(Config.URI_PRODUCT_LIST))
            {
                var reader = new StreamReader(resp.GetResponseStream());
                var parser = new Parser(reader.ReadToEnd());

                var ret = new List<Product>();
                var productRows = parser.GetProductDataRows();
                foreach (var row in productRows)
                {
                    var p = new Product();
                    p.Fill(row);
                    ret.Add(p);
                }
                return ret;
            }
        }

        public static Product GetUpsell(Product product)
        {
            var allProducts = GetAll();
            return allProducts.Where(upsell => upsell.IsChildOf(product)).FirstOrDefault();
        }

        /// <summary>
        /// Returns parent product for specified upsell.
        /// </summary>
        public static Product GetProductByUpsell(Product upsell)
        {
            var allProducts = GetAll();
            return allProducts.Where(p => p.IsParentFor(upsell)).FirstOrDefault();
        }

        public static Product CreateCopy(Product other)
        {
            return Get(other.ID);
        }
    }
}
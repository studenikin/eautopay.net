using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace EAutopay.NET
{
    public class ProductFactory
    {
        private ProductFactory() { }

        public static Product Get(int id)
        {
            var allProducts = GetAll();
            return allProducts.Where(p => p.ID == id).FirstOrDefault();
        }

        public static List<Product> GetAll()
        {
            var poster = new Poster();
            using (var resp = poster.HttpGet(Config.URI_PRODUCT_LIST))
            {
                var reader = new StreamReader(resp.GetResponseStream());
                return Parser.GetProducts(reader.ReadToEnd());
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
            var p = new Product();
            p.ID = other.ID;
            p.Name = other.Name;
            p.Price = other.Price;
            return p;
        }
    }
}
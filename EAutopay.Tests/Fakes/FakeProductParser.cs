using System.Collections.Generic;

using EAutopay.Products;
using EAutopay.Parsers;

namespace EAutopay.Tests.Fakes
{
    public class FakeProductParser : IProductParser
    {
        public int GetProductID(string source)
        {
            return 10;
        }

        public List<Product> ExtractProducts(string source)
        {
            var ret = new List<Product>();

            var p1 = new Product();
            p1.ID = 1;
            p1.Name = "product 1";
            p1.Price = 999.00;

            var p2 = new Product();
            p2.ID = 2;
            p2.Name = "product 2";
            p2.Price = 1234.00;

            ret.Add(p1);
            ret.Add(p2);

            return ret;
        }
    }
}

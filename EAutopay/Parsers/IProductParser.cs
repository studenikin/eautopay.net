using System.Collections.Generic;

using EAutopay.Products;

namespace EAutopay.Parsers
{
    public interface IProductParser
    {
        int GetProductID(string source);

        List<Product> ExtractProducts(string source);
    }
}

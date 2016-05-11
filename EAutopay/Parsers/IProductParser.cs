using System.Collections.Generic;

using EAutopay.Products;

namespace EAutopay.Parsers
{
    public interface IProductParser
    {
        List<Product> ExtractProducts(string source);
    }
}

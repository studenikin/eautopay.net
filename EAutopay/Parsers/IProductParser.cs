using System.Collections.Generic;

using EAutopay.Products;

namespace EAutopay.Parsers
{
    /// <summary>
    /// Extracts the product data out of the scpecific source.
    /// </summary>
    public interface IProductParser
    {
        /// <summary>
        /// Gets the list of products in E-Autopay
        /// </summary>
        /// <param name="source">Html source to be parsed.</param>
        /// <returns>The list of <see cref="Product"/>.</returns>
        List<Product> ExtractProducts(string source);
    }
}

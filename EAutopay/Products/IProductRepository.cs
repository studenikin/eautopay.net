using System.Collections.Generic;

namespace EAutopay.Products
{
    /// <summary>
    /// Provides CRUD operations for products in E-Autopay.
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// Gets the product in E-Autopay for the specified ID.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>A <see cref="Product"/></returns>
        Product Get(int id);

        /// <summary>
        /// Gets all products in E-Autopay.
        /// </summary>
        /// <returns>The list of <see cref="Product"/>.</returns>
        List<Product> GetAll();

        /// <summary>
        /// Creates a new product in E-Autopay; or updates existing one.
        /// </summary>
        /// <param name="p"><see cref="Product"/> to be updated/created.</param>
        /// <param name="isForUpsell">Indicates whether a specified product is an upsell.</param>
        /// <returns><see cref="Product"/> ID.</returns>
        int Save(Product p, bool isForUpsell);

        /// <summary>
        /// Deletes the specified product from E-Autopay.
        /// </summary>
        /// <param name="p"><see cref="Product"/> to be removed.</param>
        void Delete(Product p);
    }
}

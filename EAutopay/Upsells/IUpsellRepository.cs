using System.Collections.Generic;

using EAutopay.Products;

namespace EAutopay.Upsells
{
    /// <summary>
    /// Provides CRUD operations for upsells in E-Autopay.
    /// </summary>
    public interface IUpsellRepository
    {
        /// <summary>
        /// Gets upsell from E-Autopay by id.
        /// </summary>
        /// <param name="id">ID of the upsell to get.</param>
        /// <param name="id">ID of the product which the upsell belongs to.</param>
        /// <returns>Upsell object.</returns>
        Upsell Get(int id, int productId);

        /// <summary>
        /// Checks whether specified product has upsells.
        /// </summary>
        /// <param name="p">Product to check.</param>
        /// <returns>True if the product has at least one upsell. Otherwise - False.</returns>
        bool HasUpsell(Product p);

        /// <summary>
        /// Updates upsell or creates new one in E-Autopay.
        /// </summary>
        /// <param name="upsell">Upsell to be updated/created.</param>
        /// <param name="productId">ID of the product to bind upsell to.</param>
        /// <returns>ID of the upsell created.</returns>
        int Save(Upsell upsell, int productId);

        /// <summary>
        /// Gets upsells for specified product.
        /// </summary>
        /// <param name="productId">ID of the product to get upsell for.</param>
        /// <returns>List of upsells belonging to specified product.</returns>
        List<Upsell> GetByProduct(int productId);

        /// <summary>
        /// Removes upsell from E-Autopay.
        /// </summary>
        /// <param name="upsell">Upsell to be deleted.</param>
        void Delete(Upsell upsell);

        /// <summary>
        /// Removes all product upsells from E-Autopay.
        /// </summary>
        /// <param name="p">Product to remove upsells from.</param>
        void DeleteByProduct(Product p);
    }
}

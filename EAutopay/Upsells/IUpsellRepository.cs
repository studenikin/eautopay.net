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
        /// Gets the specified upsell for the specified product.
        /// </summary>
        /// <param name="id"><see cref="Upsell"/> ID.</param>
        /// <param name="productId"><see cref="Product"/> ID.</param>
        /// <returns>An <see cref="Upsell"/>.</returns>
        Upsell Get(int id, int productId);

        /// <summary>
        /// Determines whether the specified product has an upsell(s) in E-Autopay.
        /// </summary>
        /// <param name="p">The <see cref="Product"/> to be checked.</param>
        /// <returns>true if the product doesn't have an upsell(s); otherwise, false.</returns>
        bool HasUpsell(Product p);

        /// <summary>
        /// Creates a new upsell in E-Autopay; or updates existing one.
        /// </summary>
        /// <param name="upsell"><see cref="Upsell"/> to be created/updated.</param>
        /// <param name="productId"><see cref="Product"/> ID.</param>
        /// <returns><see cref="Upsell"/> ID.</returns>
        int Save(Upsell upsell, int productId);

        /// <summary>
        /// Gets upsells for the specified product in E-Autopay.
        /// </summary>
        /// <param name="productId"><see cref="Product"/> ID.</param>
        /// <returns>The list of <see cref="Upsell"/>.</returns>
        List<Upsell> GetByProduct(int productId);

        /// <summary>
        /// Deletes the specified upsell from E-Autopay.
        /// </summary>
        /// <param name="upsell"><see cref="Upsell"/> to be deleted.</param>
        void Delete(Upsell upsell);

        /// <summary>
        /// Deletes all upsells for the specified product.
        /// </summary>
        /// <param name="p"><see cref="Upsell"/> to remove upsells from.</param>
        void DeleteByProduct(Product p);
    }
}

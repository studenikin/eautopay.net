using System.Collections.Generic;

using EAutopay.Upsells;

namespace EAutopay.Parsers
{
    /// <summary>
    /// Extracts the upsell data out of the scpecific source.
    /// </summary>
    public interface IUpsellParser
    {
        /// <summary>
        /// Gets upsell settings of particular product in E-Autopay. 
        /// </summary>
        /// <param name="source">Html source to be parsed.</param>
        /// <returns>A <see cref="UpsellSettings"/>.</returns>
        UpsellSettings ExtractSettings(string source);

        /// <summary>
        /// Gets the list of upsells for the specified product in E-Autopay.
        /// </summary>
        /// <param name="productId">Product to get upsells for.</param>
        /// <param name="source">Html source to be parsed.</param>
        /// <returns>The list of <see cref="Upsell"/>.</returns>
        List<Upsell> ExtractUpsells(int productId, string source);
    }
}

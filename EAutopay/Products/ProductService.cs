using System.IO;

using EAutopay.Upsells;

namespace EAutopay.Products
{
    /// <summary>
    /// Encapsulates useful methods for E-Autopay product.
    /// </summary>
    public class ProductService
    {
        readonly IConfiguration _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductService"/> class.
        /// </summary>
        public ProductService() : this(null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductService"/> class.
        /// </summary>
        /// <param name="config">General E-Autopay settings.</param>
        public ProductService(IConfiguration config)
        {
            _config = config ?? new EAutopayConfig();
        }

        /// <summary>
        /// Gets upsell settings for the specified product in E-Autopay.
        /// </summary>
        /// <param name="productId"><see cref="Product"/> ID.</param>
        /// <returns>A <see cref="UpsellSettings"/>.</returns>
        public UpsellSettings GetUpsellSettings(int productId)
        {
            var crawler = new Crawler();
            using (var resp = crawler.HttpGet(_config.GetSendSettingsUri(productId)))
            {
                var reader = new StreamReader(resp.GetResponseStream());
                var parser = new Parser(reader.ReadToEnd());

                return parser.GetUpsellSettings();
            }
        }
    }
}

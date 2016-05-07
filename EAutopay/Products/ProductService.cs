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

        public ProductService() : this(null)
        { }

        public ProductService(IConfiguration config)
        {
            _config = config ?? new EAutopayConfig();
        }

        /// <summary>
        /// Gets upsell settings for specified product in E-Autopay.
        /// </summary>
        /// <param name="productId">Product to get settings for.</param>
        /// <returns>UpsellSettings object.</returns>
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

using EAutopay.Parsers;
using EAutopay.Upsells;

namespace EAutopay.Products
{
    /// <summary>
    /// Encapsulates useful methods for E-Autopay product.
    /// </summary>
    public class ProductService
    {
        readonly IConfiguration _config;

        readonly IUpsellParser _upsellParser;

        readonly ICrawler _crawler;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductService"/> class.
        /// </summary>
        public ProductService() : this(null, null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductService"/> class.
        /// </summary>
        /// <param name="config">General E-Autopay settings.</param>
        public ProductService(IConfiguration config, ICrawler crawler, IUpsellParser upsellParser)
        {
            _config = config ?? new AppConfig();
            _crawler = crawler ?? new Crawler();
            _upsellParser = upsellParser ?? new EAutopayUpsellParser();
        }

        /// <summary>
        /// Gets upsell settings for the specified product in E-Autopay.
        /// </summary>
        /// <param name="productId"><see cref="Product"/> ID.</param>
        /// <returns>A <see cref="UpsellSettings"/>.</returns>
        public UpsellSettings GetUpsellSettings(int productId)
        {
            var up = new UriProvider(_config.Login);
            var resp = _crawler.Get(up.GetSendSettingsUri(productId));

            return _upsellParser.ExtractSettings(resp.Data);
        }
    }
}

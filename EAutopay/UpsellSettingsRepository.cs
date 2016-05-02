using System.IO;

namespace EAutopay
{
    /// <summary>
    /// Provides CRUD and Query operations for the UpsellSettings entity.
    /// </summary>
    public class UpsellSettingsRepository
    {
        private UpsellSettingsRepository()
        { }

        /// <summary>
        /// Gets upsell settings of specified product in E-Autopay.
        /// </summary>
        /// <param name="productId">Product to get settings for.</param>
        /// <returns>UpsellSettings object.</returns>
        public static UpsellSettings Get(int productId)
        {
            var crawler = new Crawler();
            using (var resp = crawler.HttpGet(Config.GetSendSettingsURI(productId)))
            {
                var reader = new StreamReader(resp.GetResponseStream());
                var parser = new Parser(reader.ReadToEnd());

                return parser.GetUpsellSettings();
            }
        }
    }
}

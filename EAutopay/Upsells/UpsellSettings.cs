namespace EAutopay.Upsells
{
    /// <summary>
    /// Encapsulates the "Upsell Settings" section in E-Autopay.
    /// </summary>
    public class UpsellSettings
    {
        /// <summary>
        /// Gets the value indicating that the "Allow Upsells" checkbox is enabled in E-Autopay.
        /// </summary>
        public bool IsUpsellsEnabled { get; set; }

        /// <summary>
        /// Gets the "Upsell Interval" property in E-Autopay.
        /// </summary>
        public int Interval { get; set; }

        /// <summary>
        /// Gets the URI to redirect to after ordering an upsell.
        /// </summary>
        public string RedirectUri { get; set; }

        /// <summary>
        /// Determines whether the specified product has an upsell(s) in E-Autopay.
        /// </summary>
        public bool HasProductUpsells{ get; set; }
    }
}

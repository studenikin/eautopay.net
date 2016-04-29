namespace EAutopay
{
    /// <summary>
    /// Encapsulates the "Upsell Settings" section in E-Autopay.
    /// </summary>
    public class UpsellSettings
    {
        /// <summary>
        /// True if the "Allow Upsells" checkbox is enabled in E-Autopay. Otherwise - false.
        /// </summary>
        public bool IsUpsellsEnabled { get; set; }

        /// <summary>
        /// Represents the "Upsell Interval" property in E-Autopay.
        /// </summary>
        public int Interval { get; set; }

        /// <summary>
        /// URI to redirect to after ordering an upsell.
        /// </summary>
        public string RedirectUri { get; set; }

        /// <summary>
        /// True if product has upsells in E-Autopay. Otherwise - false.
        /// </summary>
        public bool HasProductUpsells{ get; set; }

        /// <summary>
        /// Initializes instance of the class.
        /// </summary>
        public UpsellSettings()
        { }

        ///// <summary>
        ///// Product in E-Autopay.
        ///// </summary>
        //public Product Product { get; private set; }

        ///// <summary>
        ///// Initializes instance of the class.
        ///// </summary>
        ///// <param name="p">Product to check the "Upsell Settings" against.</param>
        //public UpsellSettings(Product p)
        //{
        //    Product = p;
        //}
    }
}

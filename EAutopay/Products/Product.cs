using System.Globalization;

namespace EAutopay.Products
{
    /// <summary>
    /// Encapsulates product in E-Autopay.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Gets the upsell marker for a product.
        /// </summary>
        internal const string UPSELL_SUFFIX = "UPSELL";

        /// <summary>
        /// Get the product ID.
        /// </summary>
        public int ID { get; internal set; }

        /// <summary>
        /// Gets or sets the product name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the product price.
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Gets the currency value of the price.
        /// </summary>
        public string PriceFormatted
        {
            get { return Price.ToString("C");}
        }

        /// <summary>
        /// Gets the culture-independent value of the price.
        /// </summary>
        public string PriceInvariant
        {
            get { return Price.ToString("F", CultureInfo.InvariantCulture); /* format: 999.50 */ }
        }

        /// <summary>
        /// Determines whether the product represents an upsell.
        /// </summary>
        public bool IsUpsell
        {
            get
            {
                if (!string.IsNullOrEmpty(Name))
                {
                    return Name.ToUpper().Contains(UPSELL_SUFFIX.ToUpper());
                }
                return false;
            }
        }

        /// <summary>
        /// Determines whether the product exists in E-Autopay.
        /// </summary>
        public bool IsNew
        {
            get { return ID == 0; }
            internal set { ID = 0; }
        }
    }
}

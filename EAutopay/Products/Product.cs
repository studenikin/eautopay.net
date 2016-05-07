using System.Globalization;

namespace EAutopay.Products
{
    /// <summary>
    /// Encapsulates product in E-Autopay.
    /// </summary>
    public class Product
    {
        const string UPSELL_SUFFIX = "UPSELL";

        /// <summary>
        /// Product ID.
        /// </summary>
        public int ID { get; internal set; }

        /// <summary>
        /// Product name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Product price.
        /// </summary>
        public double Price { get; set; }

        public string PriceFormatted
        {
            get { return Price.ToString("C");}
        }

        public string PriceInvariant
        {
            get { return Price.ToString("F", CultureInfo.InvariantCulture); /* format: 999.50 */ }
        }

        /// <summary>
        /// Returns True if the product represents upsell. Otherwise - False.
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
        /// Returns True if the product doesn't exist in E-Autopay. Otherwise - False.
        /// </summary>
        public bool IsNew
        {
            get { return ID == 0; }
            internal set { ID = 0; }
        }

        /// <summary>
        /// Returns True if the product is a parent for specified product.
        /// </summary>
        internal bool IsParentFor(Product product)
        {
            return GetNameForUpsell().Equals(product.Name);
        }

        /// <summary>
        /// Returns True if the product is a child for specified product.
        /// </summary>
        internal bool IsChildOf(Product product)
        {
            return IsUpsell && Name.Equals(product.GetNameForUpsell());
        }

        private string GetNameForUpsell()
        {
            return string.Format("{0}_{1}_{2}", Name, UPSELL_SUFFIX, ID);
        }
    }
}

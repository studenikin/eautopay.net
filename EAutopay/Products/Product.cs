﻿using System;
using System.Globalization;

namespace EAutopay.Products
{
    public class Product : ICloneable
    {
        const string UPSELL_SUFFIX = "UPSELL";

        readonly IConfiguration _config;

        readonly IProductRepository _repository;

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

        public Product() : this(null, null)
        {}

        public Product(IConfiguration config, IProductRepository repository)
        {
            _config = config ?? new EAutopayConfig();
            _repository = repository ?? new EAutopayProductRepository();
        }

        private string GetNameForUpsell()
        {
            return string.Format("{0}_{1}_{2}", Name, UPSELL_SUFFIX, ID);
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

        /// <summary>
        /// Returns True if the product has at least one upsell. Otherwise - False.
        /// </summary>
        /// <returns></returns>
        public bool HasUpsell()
        {
            return _repository.GetUpsell(this) != null;
        }

        /// <summary>
        /// Adds upsell to the product.
        /// </summary>
        /// <param name="price">Upsell price.</param>
        /// <returns>Reference to the upsell created.</returns>
        public Product AddUpsell(double price)
        {
            var service = new ProductService(_config);
            service.EnableUpsells(this);

            Product upsell = (Product)this.Clone();
            upsell.IsNew = true;
            upsell.Name = GetNameForUpsell();
            upsell.Price = price;
            _repository.Save(upsell);

            service.BindUpsell(this, upsell);
            return upsell;
        }

        /// <summary>
        /// Creates a copy of the product.
        /// </summary>
        /// <returns>Object type of Product.</returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}

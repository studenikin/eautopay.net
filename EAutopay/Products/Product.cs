using System;
using System.Globalization;

namespace EAutopay.Products
{
    public class Product : ICloneable
    {
        IProductRepository _repository;

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
                    return Name.ToUpper().Contains(Config.UPSELL_SUFFIX.ToUpper());
                }
                return false;
            }
        }

        internal bool IsNew
        {
            get { return ID == 0; }
            set { ID = 0; }
        }

        public Product()
        {
            _repository = new EAutopayProductRepository();
        }

        public Product(IProductRepository repository)
        {
            _repository = repository;
        }

        private string GetNameForUpsell()
        {
            return string.Format("{0}_{1}_{2}", Name, Config.UPSELL_SUFFIX, ID);
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
            ProductService.EnableUpsells(this);

            Product upsell = (Product)this.Clone();
            upsell.IsNew = true;
            upsell.Name = GetNameForUpsell();
            upsell.Price = price;
            _repository.Save(upsell);

            ProductService.BindUpsell(this, upsell);
            return upsell;
        }

        /// <summary>
        /// Removes product and corresponding upsell (if any).
        /// </summary>
        public void Delete()
        {
            if (IsNew) return;

           if (IsUpsell)
            {
                UnBindUpsell(this);
            }
           else
            {
                if (HasUpsell())
                {
                    RemoveUpsell();
                }
            }
            _repository.Remove(this);
            IsNew = true;
        }

        /// <summary>
        /// Removes upsell of the product.
        /// </summary>
        private void RemoveUpsell()
        {
            var upsell = _repository.GetUpsell(this);
            if (upsell != null)
            {
                _repository.Remove(upsell);
            }
        }

        internal void Fill(IProductDataRow dr)
        {
            ID = dr.ID;
            Name = dr.Name;
            Price = dr.Price;
        }

        /// <summary>
        /// Removes all references between this upsell and its parent product.
        /// </summary>
        private void UnBindUpsell(Product upsell)
        {
            var parent = _repository.GetByUpsell(upsell);
            if (parent != null)
            {
                ProductService.DisableUpsells(parent);
            }
        }

        /// <summary>
        /// Updates product or creates new one if ID equals zero.
        /// </summary>
        /// <returns>Product ID.</returns>
        public int Save()
        {
            return _repository.Save(this);
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

using System.Collections.Generic;

namespace EAutopay.Products
{
    public interface IProductRepository
    {
        Product Get(int id);

        List<Product> GetAll();

        Product GetUpsell(Product p);

        Product GetByUpsell(Product upsell);

        Product CreateCopy(Product other);

        int Save(Product p);

        void Remove(Product p);
    }
}

using System.Collections.Generic;

namespace EAutopay.Products
{
    public interface IProductRepository
    {
        Product Get(int id);

        List<Product> GetAll();

        Product GetUpsell(Product p);

        Product GetByUpsell(Product upsell);

        int Save(Product p);

        void Delete(Product p);
    }
}

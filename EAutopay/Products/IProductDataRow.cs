namespace EAutopay.Products
{
    internal interface IProductDataRow
    {
        int ID { get; set; }
        string Name { get; set; }
        double Price { get; set; }
    }
}
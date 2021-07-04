namespace PriceEngine.Models
{
    public static class ProductsData
    {
        public static void Init(ProductContext context)
        {
            context.Products.AddRange(
                new Product()
                {
                    ProductNumber = "1",
                    ProductName = "Product1",
                    CartonSize = 20,
                    BoxFullCost = 175m,
                    SingleUnitMarkupPercentage = 30
                },
                new Product()
                {
                    ProductNumber = "2",
                    ProductName = "Product2",
                    CartonSize = 5,
                    BoxFullCost = 825m,
                    SingleUnitMarkupPercentage = 30
                }
            );

            context.SaveChanges();
        }
    }
}

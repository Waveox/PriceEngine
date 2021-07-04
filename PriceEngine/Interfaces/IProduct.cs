namespace PriceEngine.Interfaces
{
    public interface IProduct
    {
        string ProductNumber { get; set; }
        string ProductName { get; set; }
        int CartonSize { get; set; }
        decimal? BoxFullCost { get; set; }
        int SingleUnitMarkupPercentage { get; set; }
    }
}

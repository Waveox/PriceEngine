namespace PriceEngine.Interfaces
{
    public interface IFullPrice
    {
        string ProductNumber { get; set; }
        string ProductName { get; set; }
        int RequestedUnits { get; set; }
        decimal? TotalCost { get; set; }
    }
}

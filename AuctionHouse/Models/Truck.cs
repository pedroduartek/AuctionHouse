namespace AuctionHouse.Models
{
    public class Truck : Vehicle
    {
        public double? LoadCapacity { get; }

        public Truck(double? capacity, string? manufacturer, string? model, int? year, double? startingBid) : base (manufacturer, model, year, startingBid) 
        {
            if (capacity is null) throw new ArgumentNullException (nameof (capacity));

            LoadCapacity = capacity;
        }
    }
}

namespace AuctionHouse.Models
{
    public class Sedan : Vehicle
    {
        public int? NumberOfDoors { get; }
        public Sedan(int? numberOfDoors, string? manufacturer, string? model, int? year, double? startingBid) : base (manufacturer, model, year, startingBid)
        {
            if(numberOfDoors is null) throw new ArgumentNullException (nameof (NumberOfDoors));

            NumberOfDoors = numberOfDoors;
        }
    }
}

namespace AuctionHouse.Models
{
    public class Hatchback : Vehicle
    {
        public int? NumberOfDoors { get; }

        public Hatchback(int? numberOfDoors, string? manufacturer, string? model, int? year, double? startingBid) : base (manufacturer, model, year, startingBid)
        {
            if(numberOfDoors is null) throw new ArgumentNullException (nameof (NumberOfDoors));

            NumberOfDoors = numberOfDoors;
        }
    }
}

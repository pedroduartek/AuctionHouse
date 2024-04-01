namespace AuctionHouse.Models
{
    public class SUV : Vehicle
    {
        public int? NumberOfSeats { get; }

        public SUV(int? numberOfSeats, string? manufacturer, string? model, int? year, double? startingBid) : base (manufacturer, model, year, startingBid)
        {
            if(numberOfSeats is null) throw new ArgumentNullException (nameof (NumberOfSeats));

            NumberOfSeats = numberOfSeats;
        }
    }
}

namespace AuctionHouse.Models
{
    public abstract class Vehicle
    {
        public string? Manufacturer { get; }
        public string? Model { get; }
        public int? Year { get; }
        public double? StartingBid { get; }
        public AuctionInfo Auction { get; }

        public Vehicle(string? manufacturer, string? model, int? year, double? startingBid)
        {
            if(manufacturer is null) throw new ArgumentNullException(nameof(Manufacturer));
            if(model is null) throw new ArgumentNullException(nameof(Model));
            if(year is null) throw new ArgumentNullException(nameof(Year));
            if(startingBid is null) throw new ArgumentNullException(nameof(StartingBid));

            Manufacturer = manufacturer;
            Model = model;
            Year = year;
            StartingBid = startingBid;
            Auction = new AuctionInfo()
            {
                IsAuctionActive = false,
                Id = Guid.NewGuid()
            };
        }
        
    }
}

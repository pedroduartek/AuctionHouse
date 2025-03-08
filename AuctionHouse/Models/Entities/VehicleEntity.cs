
namespace AuctionHouse.Models.Entities
{
    public class VehicleEntity
    {
        public Guid Id { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public double StartingBid { get; set; }
        public int NumberOfDoors { get; set; }
        public int NumberOfSeats { get; set; }
        public double LoadCapacity { get; set; }
        public VehicleType Type { get; set; }
        public AuctionInfo AuctionInfo { get; set; }
    }

}

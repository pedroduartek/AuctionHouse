namespace AuctionHouse.Models
{
    public class VehicleInputModel
    {
        public Guid Id { get; set; }
        public string? Type { get; set; }
        public int? NumberOfDoors { get; set; }
        public int? NumberOfSeats { get; set; }
        public double? LoadCapacity { get; set; }
        public string? Manufacturer { get; set; }
        public string? Model { get; set; }
        public int? Year { get; set; }
        public double? StartingBid { get; set; }
    }
}

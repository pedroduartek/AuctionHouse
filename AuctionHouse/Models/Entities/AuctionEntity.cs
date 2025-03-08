
namespace AuctionHouse.Models.Entities
{
    public class AuctionEntity
    {
        public Guid Id { get; set; }
        public double? CurrentBid { get; set; }
        public string? CurrentBidder { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public VehicleEntity Vehcile { get; set; }
    }

}

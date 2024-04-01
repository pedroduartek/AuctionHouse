namespace AuctionHouse.Models
{
    public class AuctionInfo
    {
        public Guid Id { get; set; }
        public bool IsAuctionActive { get; set; }
        public double? CurrentBid { get; set; }
        public string? CurrentBidder { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}

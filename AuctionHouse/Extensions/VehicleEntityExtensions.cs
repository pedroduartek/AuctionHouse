namespace AuctionHouse.Models.Entities
{
    public static class VehicleEntityExtensions
    {
        public static void StartAuction(this VehicleEntity vehicle)
        {
            if (!vehicle.AuctionInfo.IsAuctionActive)
            {
                vehicle.AuctionInfo.CurrentBid = vehicle.StartingBid;
                vehicle.AuctionInfo.IsAuctionActive = true;
                vehicle.AuctionInfo.StartTime = DateTime.Now;
            }
        }

        public static void CloseAuction(this VehicleEntity vehicle)
        {
            if (vehicle.AuctionInfo.IsAuctionActive)
            {
                vehicle.AuctionInfo.IsAuctionActive = false;
                vehicle.AuctionInfo.EndTime = DateTime.Now;
            }
        }

        public static void PlaceBid(this VehicleEntity vehicle, double bidAmount, string bidder)
        {
            if (vehicle.AuctionInfo.IsAuctionActive && bidAmount > vehicle.AuctionInfo.CurrentBid)
            {
                vehicle.AuctionInfo.CurrentBid = bidAmount;
                vehicle.AuctionInfo.CurrentBidder = bidder;
            }
        }
    }
}

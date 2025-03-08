namespace AuctionHouse.Models.Entities
{
    public static class AuctionEntityExtensions
    {
        public static void StartAuction(this AuctionEntity auction)
        {
            if (!auction.IsAuctionActive())
            {
                auction.CurrentBid = auction.Vehcile.StartingBid;
                auction.StartTime = DateTime.Now;
            }
        }

        public static void CloseAuction(this AuctionEntity auction)
        {
            if (auction.IsAuctionActive())
            {
                auction.EndTime = DateTime.Now;
            }
        }

        public static void PlaceBid(this AuctionEntity auction, double bidAmount, string bidder)
        {
            if (auction.IsAuctionActive() && bidAmount > auction.CurrentBid)
            {
                auction.CurrentBid = bidAmount;
                auction.CurrentBidder = bidder;
            }
        }

        public static bool IsAuctionActive(this AuctionEntity auction) => auction.StartTime <= DateTime.Now && (auction.EndTime >= DateTime.Now || auction.EndTime == null);

        public static IQueryable<AuctionEntity> FilterByActiveStatus(this IQueryable<AuctionEntity> query, bool? active)
        {
            if (active.HasValue)
            {
                if (active == true)
                {
                    query = query.Where(a => a.StartTime <= DateTime.Now && (a.EndTime >= DateTime.Now || a.EndTime == null));
                }
                else
                {
                    query = query.Where(a => a.StartTime > DateTime.Now || a.EndTime < DateTime.Now || a.StartTime == null);
                }
            }
            return query;
        }
    }
}

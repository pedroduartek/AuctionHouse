using AuctionHouse.Data;
using AuctionHouse.Models.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionHouse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionController : ControllerBase
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public AuctionController(IApplicationDbContext context, IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }

        [HttpPost("start")]
        public IActionResult StartAuction(Guid auctionId)
        {
            var auction = _dbContext.Auctions
                                    .Include(v => v.Vehcile)
                                    .FirstOrDefault(v => v.Id == auctionId);

            if (auction == null)
            {
                return NotFound("Vehicle not found.");
            }

            if (auction.IsAuctionActive())
            {
                return BadRequest("Auction is already active for this vehicle.");
            }

            auction.StartAuction();

            _dbContext.SaveChanges();

            return Ok($"Auction started for vehicle with ID: {auctionId}.");
        }

        [HttpPost("close")]
        public IActionResult CloseAuction(Guid auctionId)
        {
            var auction = _dbContext.Auctions
                                    .Include(v => v.Vehcile)
                                    .FirstOrDefault(v => v.Id == auctionId);

            if (auction == null)
            {
                return NotFound("Vehicle not found.");
            }

            if (!auction.IsAuctionActive())
            {
                return BadRequest("Auction is not active for this vehicle.");
            }

            auction.CloseAuction();
            auction.Vehcile.SoldDate = DateTime.Now;
            _dbContext.SaveChanges();

            return Ok($"Auction closed for vehicle with ID: {auctionId}.");
        }

        [HttpPost("place-bid")]
        public IActionResult PlaceBid(Guid auctionId, double bidAmount, string bidder)
        {
            var auction = _dbContext.Auctions.Include(v => v.Vehcile).FirstOrDefault(v => v.Id == auctionId);

            if (auction == null)
            {
                return NotFound("Vehicle not found.");
            }

            if (!auction.IsAuctionActive())
            {
                return BadRequest("Auction is not active for this vehicle.");
            }

            if (bidAmount <= auction.CurrentBid)
            {
                return BadRequest("Bid amount must be higher than the current bid.");
            }

            auction.PlaceBid(bidAmount, bidder);

            _dbContext.SaveChanges();

            return Ok($"Bid placed successfully for vehicle with ID: {auctionId}.");
        }

        [HttpGet("auctions")]
        public IActionResult GetAuctions(bool? active, Guid? vehicleId)
        {
            var query = _dbContext.Auctions
                                        .Include(v => v.Vehcile)
                                        .AsQueryable();

            if (active.HasValue)
            {
                query = query.FilterByActiveStatus(active);
            }
            if (vehicleId.HasValue)
            {
                query = query.Where(a => a.Vehcile.Id == vehicleId);
            }


            var auctionResults = query.ToList();

            return Ok(auctionResults);
        }
    }
}

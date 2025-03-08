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
        public IActionResult StartAuction(Guid vehicleId)
        {
            var vehicle = _dbContext.Vehicles
                                    .Include(v => v.AuctionInfo)
                                    .FirstOrDefault(v => v.Id == vehicleId);

            if (vehicle == null)
            {
                return NotFound("Vehicle not found.");
            }

            if (vehicle.AuctionInfo.IsAuctionActive)
            {
                return BadRequest("Auction is already active for this vehicle.");
            }

            vehicle.StartAuction();

            _dbContext.SaveChanges();

            return Ok($"Auction started for vehicle with ID: {vehicleId}.");
        }

        [HttpPost("close")]
        public IActionResult CloseAuction(Guid vehicleId)
        {
            var vehicle = _dbContext.Vehicles
                                    .Include(v => v.AuctionInfo)
                                    .FirstOrDefault(v => v.Id == vehicleId);

            if (vehicle == null)
            {
                return NotFound("Vehicle not found.");
            }

            if (!vehicle.AuctionInfo.IsAuctionActive)
            {
                return BadRequest("Auction is not active for this vehicle.");
            }

            vehicle.CloseAuction();

            _dbContext.SaveChanges();

            return Ok($"Auction closed for vehicle with ID: {vehicleId}.");
        }

        [HttpPost("place-bid")]
        public IActionResult PlaceBid(Guid vehicleId, double bidAmount, string bidder)
        {
            var vehicle = _dbContext.Vehicles.Include(v => v.AuctionInfo).FirstOrDefault(v => v.Id == vehicleId);

            if (vehicle == null)
            {
                return NotFound("Vehicle not found.");
            }

            if (!vehicle.AuctionInfo.IsAuctionActive)
            {
                return BadRequest("Auction is not active for this vehicle.");
            }

            if (bidAmount <= vehicle.AuctionInfo.CurrentBid)
            {
                return BadRequest("Bid amount must be higher than the current bid.");
            }

            vehicle.PlaceBid(bidAmount, bidder);

            _dbContext.SaveChanges();

            return Ok($"Bid placed successfully for vehicle with ID: {vehicleId}.");
        }

        [HttpGet("active-auctions")]
        public IActionResult GetActiveAuctions()
        {
            var activeAuctions = _dbContext.Vehicles
                                        .Include(v => v.AuctionInfo)
                                        .Where(v => v.AuctionInfo.IsAuctionActive)
                                        .ToList();
            return Ok(activeAuctions);
        }
    }
}

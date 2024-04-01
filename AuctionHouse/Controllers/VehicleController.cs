using AuctionHouse.Business;
using AuctionHouse.Data;
using AuctionHouse.Models;
using AuctionHouse.Models.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionHouse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public VehicleController(ApplicationDbContext context, IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }

        [HttpPost("add")]
        public IActionResult AddVehicle([FromBody] VehicleInputModel vehicleInput)
        {
            var existingVehicle = _dbContext.Vehicles.FirstOrDefault(v => v.Id == vehicleInput.Id);

            if (existingVehicle != null)
            {
                return BadRequest("Vehicle already added.");
            }

            try
            {
                var vehicleToAdd = VehicleFactory.CreateVehicle(vehicleInput);
                var newVehicleEntity = _mapper.Map<VehicleEntity>(vehicleToAdd);

                newVehicleEntity.Id = vehicleInput.Id; 
                
                _dbContext.Vehicles.Add(newVehicleEntity);
                _dbContext.SaveChanges();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok($"Vehicle added to inventory with Id: {vehicleInput.Id}.");
        }

        [HttpGet("search")]
        public IActionResult SearchVehicles(string? type, string? manufacturer, string? model, int? year)
        {
            var query = _dbContext.Vehicles.AsQueryable();

            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(v => v.Type.ToLower() == type.ToLower());
            }
            if (!string.IsNullOrEmpty(manufacturer))
            {
                query = query.Where(v => v.Manufacturer.ToLower() == manufacturer.ToLower());
            }
            if (!string.IsNullOrEmpty(model))
            {
                query = query.Where(v => v.Model.ToLower() == model.ToLower());
            }
            if (year.HasValue)
            {
                query = query.Where(v => v.Year == year);
            }

            var searchResults = query.Include(v => v.AuctionInfo).ToList();

            return Ok(searchResults);
        }
    }
}

using AuctionHouse.Controllers;
using AuctionHouse.Data;
using AuctionHouse.Models;
using AuctionHouse.Models.Entities;
using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace AuctionHouseTests.Controllers
{
    public class VehicleControllerTests
    {
        private ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly VehicleController _sut;
        private readonly IFixture _fixture;

        public VehicleControllerTests()
        {
            CreateInMemoryDbContext();
            _mapper = Substitute.For<IMapper>();
            _fixture = new Fixture();
            _sut = new VehicleController(_dbContext, _mapper);
        }

        

        [Theory]
        [InlineData("Sedan")]
        [InlineData("Hatchback")]
        [InlineData("SUV")]
        [InlineData("Truck")]
        public void AddVehicle_WithValidVehicleInputModel_AddsVehicle(string vehicleType)
        {
            //Arrange
            var vehicleInputModel = _fixture.Build<VehicleInputModel>()
                                            .With(v => v.Type, vehicleType)
                                            .Create();
            
            var vehicleEntity = CreateObjectForMapper(vehicleInputModel);
            _mapper.Map<VehicleEntity>(vehicleInputModel).ReturnsForAnyArgs(vehicleEntity);

            //Act
            _sut.AddVehicle(vehicleInputModel);
            var vehicleAdded = _dbContext.Vehicles.FirstOrDefault(v => v.Id == vehicleInputModel.Id);

            //Assert
            Assert.NotNull(vehicleAdded);
        }

        

        [Theory]
        [InlineData("Sport")]
        [InlineData("City")]
        public void AddVehicle_WithInvalidVehicletype_ReturnsBadRequest(string vehicleType)
        {
            //Arrange
            var vehicleInputModel = _fixture.Build<VehicleInputModel>()
                                            .With(v => v.Type, vehicleType)
                                            .Create();

            var vehicleEntity = CreateObjectForMapper(vehicleInputModel);
            _mapper.Map<VehicleEntity>(vehicleInputModel).ReturnsForAnyArgs(vehicleEntity);

            //Act
            var result = _sut.AddVehicle(vehicleInputModel);
            var vehicleAdded = _dbContext.Vehicles.FirstOrDefault(v => v.Id == vehicleInputModel.Id);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Null(vehicleAdded);
        }

        [Fact]
        public void AddVehicle_WithExistingId_ReturnsBadRequest()
        {
            //Arrange
            var vehicleInputModel = _fixture.Build<VehicleInputModel>()
                                            .With(v => v.Type, "Sedan")
                                            .Create();

            var vehicleEntity = CreateObjectForMapper(vehicleInputModel);
            _mapper.Map<VehicleEntity>(vehicleInputModel).ReturnsForAnyArgs(vehicleEntity);

            _dbContext.Vehicles.Add(vehicleEntity);
            _dbContext.SaveChanges();

            //Act
            var result = _sut.AddVehicle(vehicleInputModel);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Theory]
        [InlineData(null, null, null)]
        [InlineData("", "", "")]
        public void SearchVehicles_WithNullOrEmptyParams_ReturnsAllVehicles(string? type, string? manufacturer,string? model)
        {
            //Arrange
            var vehicleInputModel = _fixture.Build<VehicleInputModel>()
                                            .With(v => v.Type, "Sedan")
                                            .Create();

            var vehicleEntity = CreateObjectForMapper(vehicleInputModel);
            _mapper.Map<VehicleEntity>(vehicleInputModel).ReturnsForAnyArgs(vehicleEntity);

            _dbContext.Vehicles.Add(vehicleEntity);
            _dbContext.SaveChanges();

            var listOfVehicles = new List<VehicleEntity>();

            //Act
            var result = _sut.SearchVehicles(type, manufacturer, model, null);
            if (result is OkObjectResult)
            {
                var okObjectResult = result as OkObjectResult;
                listOfVehicles = okObjectResult?.Value as List<VehicleEntity>;
            }

            //Assert
            Assert.True(listOfVehicles?.Contains(vehicleEntity));
        }

        [Fact]
        public void SearchVehicles_WithSpecificParams_ReturnsMatchingVehicles()
        {
            //Arrange
            var vehicleInputModel = _fixture.Build<VehicleInputModel>()
                                            .With(v => v.Type, "Sedan")
                                            .Create();

            var vehicleEntity = CreateObjectForMapper(vehicleInputModel);
            _mapper.Map<VehicleEntity>(vehicleInputModel).ReturnsForAnyArgs(vehicleEntity);

            _dbContext.Vehicles.Add(vehicleEntity);
            _dbContext.SaveChanges();

            var listOfVehicles = new List<VehicleEntity>();

            //Act
            var result = _sut.SearchVehicles(vehicleEntity.Type, vehicleEntity.Manufacturer, vehicleEntity.Model, vehicleEntity.Year);
            if (result is OkObjectResult)
            {
                var okObjectResult = result as OkObjectResult;
                listOfVehicles = okObjectResult?.Value as List<VehicleEntity>;
            }

            //Assert
            Assert.True(listOfVehicles?.Contains(vehicleEntity));
        }

        private VehicleEntity CreateObjectForMapper(VehicleInputModel vehicleInputModel)
        {
            return new VehicleEntity()
            {
                Id = vehicleInputModel.Id,
                LoadCapacity = (double)vehicleInputModel.LoadCapacity,
                Manufacturer = vehicleInputModel.Manufacturer,
                Model = vehicleInputModel.Model,
                NumberOfDoors = (int)vehicleInputModel.NumberOfDoors,
                NumberOfSeats = (int)vehicleInputModel.NumberOfSeats,
                StartingBid = (double)vehicleInputModel.StartingBid,
                Type = vehicleInputModel.Type,
                Year = (int)vehicleInputModel.Year,
                AuctionInfo = new AuctionInfo()
            };
        }

        private void CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                                   .UseInMemoryDatabase("VehicleControllerTests")
                                   .Options;

            _dbContext = new ApplicationDbContext(options);
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();
        }
    }
}

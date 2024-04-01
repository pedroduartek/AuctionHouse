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
    public class AuctionControllerTests
    {
        private ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly AuctionController _sut;
        private readonly IFixture _fixture;

        public AuctionControllerTests()
        {
            CreateInMemoryDbContext();
            _mapper = Substitute.For<IMapper>();
            _fixture = new Fixture();
            _sut = new AuctionController(_dbContext, _mapper);
        }

        [Fact]
        public void StartAuction_WithValidParams_StartsAuction()
        {
            //Arrange
            var vehicleEntity = CreateVehicleEntityFixtureWithAuction(false);

            _dbContext.Vehicles.Add(vehicleEntity);
            _dbContext.SaveChanges();

            //Act
            _sut.StartAuction(vehicleEntity.Id);
            var vehicleAfterAct = _dbContext.Vehicles.Include(v => v.AuctionInfo).FirstOrDefault(v => v.Id == vehicleEntity.Id);

            //Assert
            Assert.NotNull(vehicleAfterAct);
            Assert.Equal(vehicleAfterAct.AuctionInfo.CurrentBid, vehicleEntity.StartingBid);
            Assert.True(vehicleAfterAct.AuctionInfo.IsAuctionActive);
        }

        [Fact]
        public void StartAuction_WithNotFoundVehicle_ReturnsNotFound()
        {
            //Arrange
            var vehicleEntity = CreateVehicleEntityFixtureWithAuction(false);

            //Act
            var result = _sut.StartAuction(vehicleEntity.Id);

            //Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void StartAuction_WithActiveAuction_ReturnsBadRequest()
        {
            //Arrange
            var vehicleEntity = CreateVehicleEntityFixtureWithAuction(true);

            _dbContext.Vehicles.Add(vehicleEntity);
            _dbContext.SaveChanges();

            //Act
            var result = _sut.StartAuction(vehicleEntity.Id);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CloseAuction_WithValidParams_ClosesAuction()
        {
            //Arrange
            var vehicleEntity = CreateVehicleEntityFixtureWithAuction(true);

            _dbContext.Vehicles.Add(vehicleEntity);
            _dbContext.SaveChanges();

            //Act
            _sut.CloseAuction(vehicleEntity.Id);
            var vehicleAfterAct = _dbContext.Vehicles.Include(v => v.AuctionInfo).FirstOrDefault(v => v.Id == vehicleEntity.Id);

            //Assert
            Assert.NotNull(vehicleAfterAct);
            Assert.False(vehicleAfterAct.AuctionInfo.IsAuctionActive);
        }

        [Fact]
        public void CloseAuction_WithNotFoundVehicle_ReturnsNotFound()
        {
            //Arrange
            var vehicleEntity = CreateVehicleEntityFixtureWithAuction(true);

            //Act
            var result = _sut.CloseAuction(vehicleEntity.Id);

            //Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void CloseAuction_WithInactiveAuction_ReturnsBadRequest()
        {
            //Arrange
            var vehicleEntity = CreateVehicleEntityFixtureWithAuction(false);

            _dbContext.Vehicles.Add(vehicleEntity);
            _dbContext.SaveChanges();

            //Act
            var result = _sut.CloseAuction(vehicleEntity.Id);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void PlaceBid_WithValidParams_PlacesBid()
        {
            //Arrange
            var bidder = "Pedro";
            var bidAmount = 10000;

            var vehicleEntity = CreateVehicleEntityFixtureWithAuction(true);

            _dbContext.Vehicles.Add(vehicleEntity);
            _dbContext.SaveChanges();

            //Act
            _sut.PlaceBid(vehicleEntity.Id, bidAmount, bidder);
            var vehicleAfterAct = _dbContext.Vehicles.Include(v => v.AuctionInfo).FirstOrDefault(v => v.Id == vehicleEntity.Id);

            //Assert
            Assert.NotNull(vehicleAfterAct);
            Assert.Equal(vehicleAfterAct.AuctionInfo.CurrentBid, bidAmount);
            Assert.Equal(vehicleAfterAct.AuctionInfo.CurrentBidder, bidder);
        }

        [Fact]
        public void PlaceBid_WithNotFoundVehicle_ReturnsNotFound()
        {
            //Arrange
            var vehicleEntity = CreateVehicleEntityFixtureWithAuction(true);

            //Act
            var result = _sut.PlaceBid(vehicleEntity.Id, 10000, "Pedro");

            //Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void PlaceBid_WithInactiveAuction_ReturnsBadRequest()
        {
            //Arrange
            var vehicleEntity = CreateVehicleEntityFixtureWithAuction(false);

            _dbContext.Vehicles.Add(vehicleEntity);
            _dbContext.SaveChanges();

            //Act
            var result = _sut.PlaceBid(vehicleEntity.Id, 10000, "Pedro");

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void PlaceBid_WithLowAmount_ReturnsBadRequest(int bidChange)
        {
            //Arrange
            var vehicleEntity = CreateVehicleEntityFixtureWithAuction(true);
            var invalidBidAmount = (vehicleEntity.AuctionInfo.CurrentBid ?? 0) - bidChange;
            _dbContext.Vehicles.Add(vehicleEntity);
            _dbContext.SaveChanges();

            //Act
            var result = _sut.PlaceBid(vehicleEntity.Id, invalidBidAmount, "Pedro");

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        private void CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                                   .UseInMemoryDatabase("AuctionControllerMockDb")
                                   .Options;

            _dbContext = new ApplicationDbContext(options);
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();
        }

        private VehicleEntity CreateVehicleEntityFixtureWithAuction(bool auctionActive)
        {
            var auctionInfo = _fixture.Build<AuctionInfo>()
                                      .With(a => a.IsAuctionActive, auctionActive)
                                      .Create();
            var vehicleEntity = _fixture.Build<VehicleEntity>()
                                        .With(v => v.AuctionInfo, auctionInfo)
                                        .Create();
            return vehicleEntity;
        }
    }
}

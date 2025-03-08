using AuctionHouse.Controllers;
using AuctionHouse.Data;
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

            if (_dbContext != null)
            {
                _sut = new AuctionController(_dbContext, _mapper);
            }
        }

        [Fact]
        public void StartAuction_WithValidParams_StartsAuction()
        {
            //Arrange
            var vehicleEntity = CreateAuctionEntityFixtureWithVehicle(false);

            _dbContext.Auctions.Add(vehicleEntity);
            _dbContext.SaveChanges();

            //Act
            _sut.StartAuction(vehicleEntity.Id);
            var vehicleAfterAct = _dbContext.Vehicles;

            //Assert
            Assert.NotNull(vehicleAfterAct);
        }

        [Fact]
        public void StartAuction_WithNotFoundVehicle_ReturnsNotFound()
        {
            //Arrange
            var vehicleEntity = CreateAuctionEntityFixtureWithVehicle(false);

            //Act
            var result = _sut.StartAuction(vehicleEntity.Id);

            //Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void CloseAuction_WithNotFoundVehicle_ReturnsNotFound()
        {
            //Arrange
            var auctionEntity = CreateAuctionEntityFixtureWithVehicle(true);

            //Act
            var result = _sut.CloseAuction(auctionEntity.Id);

            //Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void CloseAuction_WithInactiveAuction_ReturnsBadRequest()
        {
            //Arrange
            var auctionEntity = CreateAuctionEntityFixtureWithVehicle(false);

            _dbContext.Auctions.Add(auctionEntity);
            _dbContext.SaveChanges();

            //Act
            var result = _sut.CloseAuction(auctionEntity.Id);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void PlaceBid_WithValidParams_PlacesBid()
        {
            //Arrange
            var bidder = "Pedro";
            var bidAmount = 10000;

            var auctionEntity = CreateAuctionEntityFixtureWithVehicle(true);

            _dbContext.Auctions.Add(auctionEntity);
            _dbContext.SaveChanges();

            //Act
            _sut.PlaceBid(auctionEntity.Id, bidAmount, bidder);
            var vehicleAfterAct = _dbContext.Auctions.FirstOrDefault(v => v.Id == auctionEntity.Id);

            //Assert
            Assert.NotNull(vehicleAfterAct);
            Assert.Equal(vehicleAfterAct.CurrentBid, bidAmount);
            Assert.Equal(vehicleAfterAct.CurrentBidder, bidder);
        }

        [Fact]
        public void PlaceBid_WithNotFoundVehicle_ReturnsNotFound()
        {
            //Arrange
            var auctionEntity = CreateAuctionEntityFixtureWithVehicle(true);

            //Act
            var result = _sut.PlaceBid(auctionEntity.Id, 10000, "Pedro");

            //Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void PlaceBid_WithInactiveAuction_ReturnsBadRequest()
        {
            //Arrange
            var auctionEntity = CreateAuctionEntityFixtureWithVehicle(false);

            _dbContext.Auctions.Add(auctionEntity);
            _dbContext.SaveChanges();

            //Act
            var result = _sut.PlaceBid(auctionEntity.Id, 10000, "Pedro");

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void PlaceBid_WithLowAmount_ReturnsBadRequest(int bidChange)
        {
            //Arrange
            var auctionEntity = CreateAuctionEntityFixtureWithVehicle(true);
            var invalidBidAmount = (auctionEntity.CurrentBid ?? 0) - bidChange;
            _dbContext.Auctions.Add(auctionEntity);
            _dbContext.SaveChanges();

            //Act
            var result = _sut.PlaceBid(auctionEntity.Id, invalidBidAmount, "Pedro");

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

        private AuctionEntity CreateAuctionEntityFixtureWithVehicle(bool auctionActive)
        {
            var vehicleEntity = _fixture.Build<VehicleEntity>()
                                        .Create();

            var auctionInfo = _fixture.Build<AuctionEntity>()
                                      .With(a => a.StartTime, auctionActive ? DateTime.UtcNow : (DateTime?)null)
                                      .With(a => a.EndTime, auctionActive ? DateTime.UtcNow.AddHours(1) : (DateTime?)null)
                                      .With(a => a.Vehcile, vehicleEntity)
                                      .Create();

            return auctionInfo;
        }
    }
}

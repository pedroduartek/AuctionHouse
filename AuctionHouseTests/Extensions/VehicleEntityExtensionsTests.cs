using AuctionHouse.Models.Entities;
using AutoFixture;

namespace AuctionHouseTests.Extensions
{
    public class VehicleEntityExtensionsTests
    {
        private Fixture _fixture;

        public VehicleEntityExtensionsTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void StartAuction_WithInactiveAuction_StartsAuction()
        {
            //Arrange
            var auctionEntity = CreateVehicleEntityFixtureWithAuction(false);

            //Act
            auctionEntity.StartAuction();

            //Assert
            Assert.True(auctionEntity.IsAuctionActive());
            Assert.Equal(auctionEntity.CurrentBid, auctionEntity.Vehcile.StartingBid);
        }

        [Fact]
        public void StartAuction_WithActiveAuction_DoesNothing()
        {
            //Arrange
            var vehicleEntity = CreateVehicleEntityFixtureWithAuction(true);

            var vehicleEntityBeforeAct = vehicleEntity;

            //Act
            vehicleEntity.StartAuction();

            //Assert
            Assert.Equal(vehicleEntity, vehicleEntityBeforeAct);
        }

        [Fact]
        public void CloseAuction_WithActiveAuction_ClosesAuction()
        {
            //Arrange
            var vehicleEntity = CreateVehicleEntityFixtureWithAuction(true);

            //Act
            vehicleEntity.CloseAuction();

            //Assert
            Assert.True(vehicleEntity.IsAuctionActive() == false);
        }

        [Fact]
        public void CloseAuction_WithInactiveAuction_DoesNothing()
        {
            //Arrange
            var vehicleEntity = CreateVehicleEntityFixtureWithAuction(false);

            var vehicleEntityBeforeAct = vehicleEntity;

            //Act
            vehicleEntity.CloseAuction();

            //Assert
            Assert.Equal(vehicleEntity, vehicleEntityBeforeAct);
        }

        [Fact]
        public void PlaceBid_WithInactiveAuction_DoesNothing()
        {
            //Arrange
            var vehicleEntity = CreateVehicleEntityFixtureWithAuction(false);

            var vehicleEntityBeforeAct = vehicleEntity;
            var bidAmount = _fixture.Create<double>();
            var bidder = _fixture.Create<string>();

            //Act
            vehicleEntity.PlaceBid(bidAmount, bidder);

            //Assert
            Assert.Equal(vehicleEntity, vehicleEntityBeforeAct);
        }

        [Fact]
        public void PlaceBid_WithActiveAuctionAndValidBid_PlacesBid()
        {
            //Arrange
            var vehicleEntity = CreateVehicleEntityFixtureWithAuction(true);
            var bidAmount = (vehicleEntity.CurrentBid ?? 0) + 1;
            var bidder = _fixture.Create<string>();

            //Act
            vehicleEntity.PlaceBid(bidAmount, bidder);

            //Assert
            Assert.Equal(vehicleEntity.CurrentBid, bidAmount);
            Assert.Equal(vehicleEntity.CurrentBidder, bidder);
        }

        [Fact]
        public void PlaceBid_WithActiveAuctionAndInvalidBid_DoesNothing()
        {
            //Arrange
            var vehicleEntity = CreateVehicleEntityFixtureWithAuction(true);
            var bidAmount = (vehicleEntity.CurrentBid ?? 0) - 1;
            var bidder = _fixture.Create<string>();

            var vehicleEntityBeforeAct = vehicleEntity;

            //Act
            vehicleEntity.PlaceBid(bidAmount, bidder);

            //Assert
            Assert.Equal(vehicleEntity, vehicleEntityBeforeAct);
        }

        private AuctionEntity CreateVehicleEntityFixtureWithAuction(bool auctionActive)
        {
            var vehicleEntity = _fixture.Build<VehicleEntity>()
                                        .Create();

            var auctionInfo = _fixture.Build<AuctionEntity>()
                                      .With(a => a.StartTime, auctionActive ? DateTime.Now : (DateTime?)null)
                                      .With(a => a.EndTime, auctionActive ? DateTime.Now.AddHours(1) : (DateTime?)null)
                                      .With(a => a.Vehcile, vehicleEntity)
                                      .Create();

            return auctionInfo;
        }
    }
}

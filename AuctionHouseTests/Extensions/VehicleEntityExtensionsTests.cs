using AuctionHouse.Models;
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
            var vehicleEntity = CreateVehicleEntityFixtureWithAuction(false);

            //Act
            vehicleEntity.StartAuction();

            //Assert
            Assert.True(vehicleEntity.AuctionInfo.IsAuctionActive);
            Assert.Equal(vehicleEntity.AuctionInfo.CurrentBid, vehicleEntity.StartingBid);
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
            Assert.True(vehicleEntity.AuctionInfo.IsAuctionActive == false);
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
            var bidAmount = (vehicleEntity.AuctionInfo.CurrentBid ?? 0) + 1;
            var bidder = _fixture.Create<string>();

            //Act
            vehicleEntity.PlaceBid(bidAmount, bidder);

            //Assert
            Assert.Equal(vehicleEntity.AuctionInfo.CurrentBid, bidAmount);
            Assert.Equal(vehicleEntity.AuctionInfo.CurrentBidder, bidder);
        }

        [Fact]
        public void PlaceBid_WithActiveAuctionAndInvalidBid_DoesNothing()
        {
            //Arrange
            var vehicleEntity = CreateVehicleEntityFixtureWithAuction(true);
            var bidAmount = (vehicleEntity.AuctionInfo.CurrentBid ?? 0) - 1;
            var bidder = _fixture.Create<string>();

            var vehicleEntityBeforeAct = vehicleEntity;

            //Act
            vehicleEntity.PlaceBid(bidAmount, bidder);

            //Assert
            Assert.Equal(vehicleEntity,vehicleEntityBeforeAct);
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

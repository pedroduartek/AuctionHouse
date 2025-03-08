using AuctionHouse.Business;
using AuctionHouse.Models;
using AutoFixture;

namespace AuctionHouseTests.Business
{
    public class VehicleFactoryTests
    {
        private readonly Fixture _fixture;

        public VehicleFactoryTests()
        {
            _fixture = new Fixture();
        }

        [Theory]
        [InlineData(VehicleType.Sedan)]
        [InlineData(VehicleType.Hatchback)]
        [InlineData(VehicleType.SUV)]
        [InlineData(VehicleType.Truck)]
        public void CreateVehicle_WithValidParams_ReturnsValidVehicle(VehicleType vehicleType)
        {
            // Arrange
            var vehicleInputModel = _fixture.Build<VehicleInputModel>()
                .With(v => v.Type, vehicleType)
                .Create();

            // Act
            var vehicle = VehicleFactory.CreateVehicle(vehicleInputModel);

            // Assert
            Assert.NotNull(vehicle);
            Assert.Equal(vehicle.GetType().Name, vehicleType.ToString());
            Assert.Equal(vehicle.Model, vehicleInputModel.Model);
            Assert.Equal(vehicle.Manufacturer, vehicleInputModel.Manufacturer);
            Assert.Equal(vehicle.Year, vehicleInputModel.Year);
            Assert.Equal(vehicle.StartingBid, vehicleInputModel.StartingBid);
            Assert.NotNull(vehicle.Auction);
        }

        [Theory]
        [InlineData(VehicleType.Sedan)]
        [InlineData(VehicleType.Hatchback)]
        public void CreateSedanAndHacthBack_WithoutNumberOfDoors_ThrowsArgumentNullException(VehicleType vehicleType)
        {
            // Arrange
            var vehicleInputModel = _fixture.Build<VehicleInputModel>()
                .With(v => v.Type, vehicleType)
                .Without(v => v.NumberOfDoors)
                .Create();

            // Act && Assert
            Assert.Throws<ArgumentNullException>(() => VehicleFactory.CreateVehicle(vehicleInputModel));
        }

        [Fact]
        public void CreateSUV_WithoutNumberOfSeats_ThrowsArgumentNullException()
        {
            // Arrange
            var vehicleInputModel = _fixture.Build<VehicleInputModel>()
                .With(v => v.Type, VehicleType.SUV)
                .Without(v => v.NumberOfSeats)
                .Create();

            // Act && Assert
            Assert.Throws<ArgumentNullException>(() => VehicleFactory.CreateVehicle(vehicleInputModel));
        }

        [Fact]
        public void CreateTruck_WithoutLoadCapacity_ThrowsArgumentNullException()
        {
            // Arrange
            var vehicleInputModel = _fixture.Build<VehicleInputModel>()
                .With(v => v.Type, VehicleType.Truck)
                .Without(v => v.LoadCapacity)
                .Create();

            // Act && Assert
            Assert.Throws<ArgumentNullException>(() => VehicleFactory.CreateVehicle(vehicleInputModel));
        }

        [Theory]
        [InlineData("VW", null)]
        [InlineData(null, "Golf")]
        public void CreateVehicle_WithoutManufacturerOrModel_ThrowsArgumentNullException(string? manufacturer, string? model)
        {
            // Arrange
            var vehicleInputModel = _fixture.Build<VehicleInputModel>()
                .With(v => v.Type, VehicleType.Sedan)
                .With(v => v.Manufacturer, manufacturer)
                .With(v => v.Model, model)
                .Create();

            // Act && Assert
            Assert.Throws<ArgumentNullException>(() => VehicleFactory.CreateVehicle(vehicleInputModel));
        }

        [Fact]
        public void CreateVehicle_WithoutYear_ThrowsArgumentNullException()
        {
            // Arrange
            var vehicleInputModel = _fixture.Build<VehicleInputModel>()
                .With(v => v.Type, VehicleType.Sedan)
                .Without(v => v.Year)
                .Create();

            // Act && Assert
            Assert.Throws<ArgumentNullException>(() => VehicleFactory.CreateVehicle(vehicleInputModel));
        }

        [Fact]
        public void CreateVehicle_WithoutStartingBid_ThrowsArgumentNullException()
        {
            // Arrange
            var vehicleInputModel = _fixture.Build<VehicleInputModel>()
                .With(v => v.Type, VehicleType.Sedan)
                .Without(v => v.StartingBid)
                .Create();

            // Act && Assert
            Assert.Throws<ArgumentNullException>(() => VehicleFactory.CreateVehicle(vehicleInputModel));
        }
    }
}
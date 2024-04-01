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
        [InlineData("Sedan")]
        [InlineData("Hatchback")]
        [InlineData("SUV")]
        [InlineData("Truck")]
        public void CreateVehicle_WithValidParams_ReturnsValidVehicle(string vehicleType)
        {
            // Arrange
            var vehicleInputModel = _fixture.Build<VehicleInputModel>()
                .With(v => v.Type, vehicleType)
                .Create();

            // Act
            var vehicle = VehicleFactory.CreateVehicle(vehicleInputModel);

            // Assert
            Assert.NotNull(vehicle);
            Assert.Equal(vehicle.GetType().Name, vehicleType);
        }

        [Theory]
        [InlineData("Sport")]
        [InlineData("City")]
        public void CreateVehicle_WithInvalidVehicleType_ThrowsArgumentException(string vehicleType)
        {
            // Arrange
            var vehicleInputModel = _fixture.Build<VehicleInputModel>()
                .With(v => v.Type, vehicleType)
                .Create();

            // Act && Assert
            Assert.Throws<ArgumentException>(() => VehicleFactory.CreateVehicle(vehicleInputModel));
        }

        [Theory]
        [InlineData("Sedan")]
        [InlineData("Hatchback")]
        public void CreateSedanAndHacthBack_WithoutNumberOfDoors_ThrowsArgumentNullException(string vehicleType)
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
                .With(v => v.Type, "SUV")
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
                .With(v => v.Type, "Truck")
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
                .With(v => v.Type, "Sedan")
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
                .With(v => v.Type, "Sedan")
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
                .With(v => v.Type, "Sedan")
                .Without(v => v.StartingBid)
                .Create();

            // Act && Assert
            Assert.Throws<ArgumentNullException>(() => VehicleFactory.CreateVehicle(vehicleInputModel));
        }
    }
}
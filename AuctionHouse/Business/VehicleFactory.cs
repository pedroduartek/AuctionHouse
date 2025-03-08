using AuctionHouse.Models;

namespace AuctionHouse.Business
{
    public static class VehicleFactory
    {
        public static Vehicle CreateVehicle(VehicleInputModel vehicleInput)
        {
            switch (vehicleInput.Type)
            {
                case VehicleType.Sedan:
                    return new Sedan(vehicleInput.NumberOfDoors, vehicleInput.Manufacturer, vehicleInput.Model, vehicleInput.Year, vehicleInput.StartingBid);
                case VehicleType.SUV:
                    return new SUV(vehicleInput.NumberOfSeats, vehicleInput.Manufacturer, vehicleInput.Model, vehicleInput.Year, vehicleInput.StartingBid);
                case VehicleType.Truck:
                    return new Truck(vehicleInput.LoadCapacity, vehicleInput.Manufacturer, vehicleInput.Model, vehicleInput.Year, vehicleInput.StartingBid);
                case VehicleType.Hatchback:
                    return new Hatchback(vehicleInput.NumberOfDoors, vehicleInput.Manufacturer, vehicleInput.Model, vehicleInput.Year, vehicleInput.StartingBid);
                default:
                    throw new ArgumentException("Missing Implementation for this vehicle type.");
            }
        }
    }

}

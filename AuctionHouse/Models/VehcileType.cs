using System.Text.Json.Serialization;

namespace AuctionHouse.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum VehicleType
    {
        Sedan,
        SUV,
        Truck,
        Hatchback
    }
}

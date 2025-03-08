using AutoMapper;
using AuctionHouse.Models.Entities;
using AuctionHouse.Models;
using System.Diagnostics.CodeAnalysis;

namespace AuctionHouse.Infra
{
    [ExcludeFromCodeCoverage]
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Vehicle, VehicleEntity>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.GetType().Name));
        }
    }
}
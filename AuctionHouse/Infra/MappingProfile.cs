using AutoMapper;
using AuctionHouse.Models.Entities;
using AuctionHouse.Models;

namespace AuctionHouse.Infra
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Vehicle, VehicleEntity>()
                .ForMember(dest => dest.AuctionInfo, opt => opt.MapFrom(src => src.Auction))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.GetType().Name));
        }
    }
}
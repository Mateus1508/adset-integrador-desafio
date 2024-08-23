using AdSetSolution.Application.DTOs;
using AdSetSolution.Domain.Models;
using AutoMapper;

namespace AdSetSolution.Infrastructure.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Vehicle, VehicleDTO>()
                .ForMember(dest => dest.OptionalIds, opt => opt.MapFrom(src => src.VehicleOptional.Select(vo => vo.OptionalId).ToList()))
                .ReverseMap();

            CreateMap<VehicleImg, VehicleImgDTO>()
                .ReverseMap();

            CreateMap<VehiclePackage, VehiclePackageDTO>()
                .ReverseMap();

            CreateMap<Package, PackageDTO>()
                .ForMember(dest => dest.Used, opt => opt.MapFrom(src => src.Used))
                .ForMember(dest => dest.Available, opt => opt.MapFrom(src => src.Available))
                .ForMember(dest => dest.IsExhausted, opt => opt.MapFrom(src => src.IsExhausted))
                .ReverseMap();

            CreateMap<Optional, OptionalDTO>()
                .ReverseMap();

            CreateMap<VehicleOptional, VehicleOptionalDTO>()
                .ReverseMap();
        }
    }
}

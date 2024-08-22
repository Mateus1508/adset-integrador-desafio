﻿using AdSetSolution.Application.DTOs;
using AdSetSolution.Domain.Models;
using AutoMapper;

namespace AdSetSolution.Infrastructure.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Vehicle, VehicleDTO>()
                .ReverseMap();
            CreateMap<VehicleImg, VehicleImgDTO>()
                .ReverseMap();
            CreateMap<VehiclePackage, VehiclePackageDTO>()
                .ReverseMap();
            CreateMap<Package, PackageDTO>()
                .ReverseMap();
            CreateMap<Optional, OptionalDTO>()
                .ReverseMap();
            CreateMap<VehicleOptional, VehicleOptionalDTO>()
                .ReverseMap();
        }
    }
}

using System;
using AutoMapper;
using AutoMobileBackend.Entities;
using AutoMobileBackend.Models;

namespace AutoMobileBackend.Mapper
{
	public class AutoMobileMappingProfile : Profile
	{
		public AutoMobileMappingProfile()
		{
			CreateMap<Vehicle, VehicleDto>();
			CreateMap<CreateVehicleDto, Vehicle>();
			CreateMap<Repair, RepairDto>()
				.ForMember(m => m.VIN, c => c.MapFrom(s => s.Vehicle.VIN));
		}
	}
}


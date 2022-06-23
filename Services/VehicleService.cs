using System;
using AutoMapper;
using AutoMobileBackend.Entities;
using AutoMobileBackend.Exceptions;
using AutoMobileBackend.Models;

namespace AutoMobileBackend.Services;

public class VehicleService : IVehicleService
{
    private readonly AutoMobileDbContext _dbContext;
    private readonly IMapper _mapper;

    public VehicleService(AutoMobileDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public void CreateVehicle(CreateVehicleDto dto)
    {
        _dbContext.Vehicles.Add(_mapper.Map<Vehicle>(dto));
        _dbContext.SaveChanges();
    }

    public VehicleDto GetVehicleWithRegistration(string registration)
    {
        var vehicle = _dbContext.Vehicles.FirstOrDefault(v => v.Registration == registration);
        if(vehicle is null)
        {
            throw new BadRequestException("Vehicle not found");
            //return null;
        }
        return _mapper.Map<VehicleDto>(vehicle);
    }
}


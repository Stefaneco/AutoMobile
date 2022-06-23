using System;
using AutoMobileBackend.Models;

namespace AutoMobileBackend.Services;

public interface IVehicleService
{
    public void CreateVehicle(CreateVehicleDto dto);
    public VehicleDto GetVehicleWithRegistration(string registration);
}


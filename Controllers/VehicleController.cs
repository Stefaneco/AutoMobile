using System;
using AutoMobileBackend.Models;
using AutoMobileBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace AutoMobileBackend.Controllers;


[ApiController]
[Route("api/vehicles")]
public class VehicleController : ControllerBase
{
	private readonly IVehicleService _service;

    public VehicleController(IVehicleService service)
    {
        _service = service;
    }

    [HttpPost]
    public ActionResult CreateVehicle([FromBody] CreateVehicleDto dto)
    {
        _service.CreateVehicle(dto);
        return Ok();
    }

    [HttpGet]
    public ActionResult<VehicleDto> GetVehicle([FromQuery(Name = "reg")] string registration)
    {
        var vehicle =_service.GetVehicleWithRegistration(registration);
        return Ok(vehicle);
    }
}


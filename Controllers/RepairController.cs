using System;
using System.Security.Claims;
using AutoMobileBackend.Entities;
using AutoMobileBackend.Models;
using AutoMobileBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoMobileBackend.Controllers;

[ApiController]
[Authorize]
[Route("api/repairs")]
public class RepairController : ControllerBase
{
	private readonly IRepairService _service;

    public RepairController(IRepairService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(Roles = Roles.MECHANIC)]
    public ActionResult CreateRepair([FromBody] CreateRepairDto dto)
    {
        _service.CreateRepair(dto, HttpContext);
        return Ok();
    }

    [HttpGet]
    public ActionResult<IEnumerable<RepairDto>> GetRepairs()
    {
        var repairs = _service.GetRepairs(HttpContext);
        return Ok(repairs);
    }
}


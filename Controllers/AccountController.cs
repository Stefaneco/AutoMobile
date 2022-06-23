using System;
using AutoMobileBackend.Entities;
using AutoMobileBackend.Models;
using AutoMobileBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoMobileBackend.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController : ControllerBase
{
	private readonly IAccountService _service;

	public AccountController(IAccountService service)
	{
		_service = service;
	}

	[HttpPost]
	[Route("customer/register")]
	public ActionResult RegisterCustomer([FromBody] RegisterDto dto)
	{
		SessionDto session = _service.RegisterCustomer(dto);
		return Ok(session);
	}

	[HttpPost]
	[Route("customer/login")]
	public ActionResult<SessionDto> LoginCustomer([FromBody] LoginDto dto)
	{
		SessionDto session = _service.GenerateSession(dto, Roles.CUSTOMER);
		return Ok(session);
	}

	[HttpPost]
	[Route("mechanic/register")]
	public ActionResult RegisterMechanic([FromBody] RegisterDto dto)
	{
		SessionDto session = _service.RegisterMechanic(dto);
		return Ok(session);
	}

	[HttpPost]
	[Route("mechanic/login")]
	public ActionResult<SessionDto> LoginMechanic([FromBody] LoginDto dto)
	{
		SessionDto session = _service.GenerateSession(dto, Roles.MECHANIC);
		return Ok(session);
	}

	[HttpPost]
	[Route("refresh")]
	public ActionResult<SessionDto> Refresh([FromBody] SessionDto dto)
    {
		SessionDto session = _service.RefreshSession(dto);
		return Ok(session);
    } 

	[HttpGet]
	[Route("test")]
	[Authorize]
	public ActionResult<string> Text()
    {
		return Ok("Test");
    }
}



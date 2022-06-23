using System;
using AutoMobileBackend.Models;

namespace AutoMobileBackend.Services;

public interface IAccountService
{
    public SessionDto RegisterCustomer(RegisterDto dto);
    public SessionDto RegisterMechanic(RegisterDto dto);
    public SessionDto GenerateSession(LoginDto dto, string role);
    public SessionDto RefreshSession(SessionDto dto);
}


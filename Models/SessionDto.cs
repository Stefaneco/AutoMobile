using System;
namespace AutoMobileBackend.Models;

public class SessionDto
{
    public string Jwt { get; set; }
    public string RefreshToken { get; set; }
}


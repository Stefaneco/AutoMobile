using System;
using System.ComponentModel.DataAnnotations;

namespace AutoMobileBackend.Entities;

public class RefreshToken
{
    public int Id { get; set; }

    public string Jwt { get; set; }

    public string Token { get; set; }

    public bool IsValid { get; set; }

    public int UserId { get; set; }

    public virtual User User { get; set; }
}


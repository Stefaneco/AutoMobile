using System;
using System.ComponentModel.DataAnnotations;

namespace AutoMobileBackend.Models;

public class LoginDto
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}


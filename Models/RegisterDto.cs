using System;
using System.ComponentModel.DataAnnotations;

namespace AutoMobileBackend.Models;

public class RegisterDto
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Surname { get; set; }

    [Required]
    [Phone]
    public string Phone { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}


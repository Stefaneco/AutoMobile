﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoMobileBackend.Entities;

[Table("Users")]
public abstract class User
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public string Phone { get; set; }

    public string? Email { get; set; }

    public string? PasswordHash { get; set; }
}



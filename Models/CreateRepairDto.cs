using System;
namespace AutoMobileBackend.Models;

public class CreateRepairDto
{
    public string ProblemDescription { get; set; }

    public string VIN { get; set; }

    public string CustomerPhone { get; set; }
}


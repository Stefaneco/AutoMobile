using System;
namespace AutoMobileBackend.Models;

public class RepairDto
{
    public int Id { get; set; }

    public string ProblemDescription { get; set; }

    public string VIN { get; set; }

    public string CustomerPhone { get; set; }
}


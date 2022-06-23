using System;
namespace AutoMobileBackend.Models;

public class CreateVehicleDto
{
    public string VIN { get; set; }

    public string Registration { get; set; }

    public string Manufacturer { get; set; }

    public string Model { get; set; }

    public int Year { get; set; }

    public string Engine { get; set; }
}


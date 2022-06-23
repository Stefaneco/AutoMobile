using System;
namespace AutoMobileBackend.Entities;

public class Repair
{
	public int Id { get; set; }

    public DateTime Started { get; set; }

    public DateTime? Finished { get; set; }

    public string ProblemDescription { get; set; }

    public string? RepairDescription { get; set; }

    public int VehicleId { get; set; }

    public int MechanicId { get; set; }

    public int CustomerId { get; set; }

    public virtual Customer Customer { get; set; }

    public virtual Vehicle Vehicle { get; set; }

    public virtual Mechanic Mechanic { get; set; }

    public virtual List<ReplacePart> ReplaceParts { get; set; }
}


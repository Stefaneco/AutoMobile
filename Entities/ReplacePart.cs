using System;
namespace AutoMobileBackend.Entities;

public class ReplacePart
{
    public int Id { get; set; }

    public bool IsNew { get; set; }

    public float Price { get; set; }

    public float CustomerPrice { get; set; }

    public int PartId { get; set; }

    public virtual Part Part { get; set; }

    public int RepairId { get; set; }
}


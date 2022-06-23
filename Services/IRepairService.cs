using System;
using AutoMobileBackend.Models;

namespace AutoMobileBackend.Services;

public interface IRepairService
{
    public void CreateRepair(CreateRepairDto dto, HttpContext context);

    public IEnumerable<RepairDto> GetRepairs(HttpContext context);
}


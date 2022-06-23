using System;
using System.Security.Claims;
using AutoMapper;
using AutoMobileBackend.Entities;
using AutoMobileBackend.Exceptions;
using AutoMobileBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoMobileBackend.Services;

public class RepairService : IRepairService
{
    private readonly AutoMobileDbContext _dbContext;
    private readonly IMapper _mapper;

    public RepairService(AutoMobileDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public void CreateRepair(CreateRepairDto dto, HttpContext context)
    {
        var nameIdentifier = context.User.FindFirst(ClaimTypes.NameIdentifier);
        if (nameIdentifier is null)
        {
            throw new BadRequestException("Missing claims");
        }

        var vehicle = _dbContext.Vehicles.FirstOrDefault(v => v.VIN == dto.VIN);
        if (vehicle is null)
        {
            throw new BadRequestException("Vehicle not found");
        }

        var customer = _dbContext.Customers.FirstOrDefault(c => c.Phone == dto.CustomerPhone);
        int customerId;
        if (customer is null)
        {
            var newCustomer = new Customer()
            {
                Phone = dto.CustomerPhone
            };
            var newCustomerEntity = _dbContext.Customers.Add(newCustomer).Entity;
            _dbContext.SaveChanges();
            customerId = newCustomerEntity.Id;
        }
        else
        {
            customerId = customer.Id;
        }

        int mechanicId = Int32.Parse(nameIdentifier.Value);

        Repair newRepair = new Repair()
        {
            Started = DateTime.Now,
            ProblemDescription = dto.ProblemDescription,
            VehicleId = vehicle.Id,
            MechanicId = mechanicId,
            CustomerId = customerId
        };

        _dbContext.Repairs.Add(newRepair);
        _dbContext.SaveChanges();
    }

    public IEnumerable<RepairDto> GetRepairs(HttpContext context)
    {
        var nameIdentifier = context.User.FindFirst(ClaimTypes.NameIdentifier);
        if (nameIdentifier is null)
        {
            throw new BadRequestException("Missing claims");
        }

        var repairs = _dbContext.Repairs
            .Include(r => r.Vehicle)
            .Where(r => r.CustomerId == Int32.Parse(nameIdentifier.Value)).ToList();

        return _mapper.Map<List<RepairDto>>(repairs);
    }
}


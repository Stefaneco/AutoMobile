using System;
using Microsoft.EntityFrameworkCore;

namespace AutoMobileBackend.Entities;

public class AutoMobileDbContext : DbContext
{
    public AutoMobileDbContext(DbContextOptions<AutoMobileDbContext> options) : base(options)
    {

    }

    public DbSet<User> Users { get; set; }

    public DbSet<Mechanic> Mechanics { get; set; }

    public DbSet<Customer> Customers { get; set; }

    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public DbSet<Vehicle> Vehicles { get; set; }

    public DbSet<Repair> Repairs { get; set; }

    public DbSet<Part> Parts { get; set; }

    public DbSet<ReplacePart> ReplaceParts { get; set; }
}

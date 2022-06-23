using System;
using AutoMobileBackend.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoMobileBackend;

public class AutomobileDbSeeder
{
    private readonly AutoMobileDbContext _dbContext;

    public AutomobileDbSeeder(AutoMobileDbContext dbContext)
	{
		_dbContext = dbContext;
	}
	public void Seed()
    {
		if (_dbContext.Database.CanConnect())
        {
            var pendingMigrations = _dbContext.Database.GetPendingMigrations();
            if (pendingMigrations != null && pendingMigrations.Any())
            {
                _dbContext.Database.Migrate();
            }
        }
	}
}


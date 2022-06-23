using System;
using AutoMobileBackend.Entities;
using AutoMobileBackend.Models;
using FluentValidation;

namespace AutoMobileBackend.Validators;

public class CreateRepairDtoValidator : AbstractValidator<CreateRepairDto>
{
	public CreateRepairDtoValidator(AutoMobileDbContext dbContext)
	{
		RuleFor(dto => dto.VIN)
			.Length(17)
			.Custom((value, context) =>
			{
				var vinInUse = dbContext.Vehicles.Any(m => m.VIN == value);
				if (!vinInUse)
				{
					context.AddFailure("VIN", "VIN doesn't exist in database");
				}
			}
			);
	} 
}


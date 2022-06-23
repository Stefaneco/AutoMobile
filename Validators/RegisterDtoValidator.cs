using System;
using AutoMobileBackend.Entities;
using AutoMobileBackend.Models;
using FluentValidation;

namespace AutoMobileBackend.Validators;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
	public RegisterDtoValidator(AutoMobileDbContext dbContext)
	{
	RuleFor(r => r.Email)
		.NotEmpty()
		.EmailAddress()
		.Custom((value, context) =>
		{
			var emailInUse = dbContext.Users.Any(m => m.Email == value);
			if (emailInUse)
			{
				context.AddFailure("Email", "Email taken");
			}
		}
		);

	RuleFor(r => r.Password)
		.MinimumLength(6);
	}
}


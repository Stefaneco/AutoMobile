using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMobileBackend.Entities;
using AutoMobileBackend.Exceptions;
using AutoMobileBackend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace AutoMobileBackend.Services;

public class AccountService : IAccountService
{
	private readonly AutoMobileDbContext _dbContext;
	private readonly IPasswordHasher<User> _userPasswordHasher;
	private readonly AuthenticationSettings _authenticationSettings;

	public AccountService(
		AutoMobileDbContext dbContext,
		IPasswordHasher<User> userPasswordHasher,
		AuthenticationSettings authenticationSettings)
	{
		_dbContext = dbContext;
		_userPasswordHasher = userPasswordHasher;
		_authenticationSettings = authenticationSettings;
	}

	public SessionDto GenerateSession(LoginDto dto, string role)
    {
		User? user = null;

		switch (role)
        {
            case Roles.CUSTOMER:
				user = _dbContext.Customers.FirstOrDefault(m => m.Email == dto.Email);
				break;
			case Roles.MECHANIC:
				user = _dbContext.Mechanics.FirstOrDefault(m => m.Email == dto.Email);
				break;
		}
		
		VerifyCredentials(dto, user);

		var claims = new List<Claim>
		{
			new Claim(ClaimTypes.NameIdentifier, user!.Id.ToString()),
			new Claim(ClaimTypes.Role, role)
		};

		return GenerateSessionFromClaims(claims, user.Id);
	}

    public SessionDto RegisterCustomer(RegisterDto dto)
    {
		Customer newUser = new Customer()
		{
			Name = dto.Name,
			Surname = dto.Surname,
			Phone = dto.Phone,
			Email = dto.Email
		};

		var hashedPassword = _userPasswordHasher.HashPassword(newUser, dto.Password);
		newUser.PasswordHash = hashedPassword;

		int dbUserId = UpsertCustomerIntoDb(newUser);
		_dbContext.SaveChanges();

		var claims = new List<Claim>
		{
			new Claim(ClaimTypes.NameIdentifier, dbUserId.ToString()),
			new Claim(ClaimTypes.Role, Roles.MECHANIC)
		};

		return GenerateSessionFromClaims(claims, dbUserId);
	}

	private int UpsertCustomerIntoDb(Customer newUser)
    {
		var customer = _dbContext.Customers.FirstOrDefault(c => c.Phone == newUser.Phone);
		if (customer is not null && customer.Email is null)
		{
			newUser.Id = customer.Id;
		}
		var insertedUser = _dbContext.Customers.Add(newUser).Entity;
		_dbContext.SaveChanges();
		return insertedUser.Id;
	}

	public SessionDto RegisterMechanic(RegisterDto dto)
	{
		Mechanic newUser = new Mechanic()
		{
			Name = dto.Name,
			Surname = dto.Surname,
			Phone = dto.Phone,
			Email = dto.Email
		};

		var hashedPassword = _userPasswordHasher.HashPassword(newUser, dto.Password);
		newUser.PasswordHash = hashedPassword;
		var dbUser = _dbContext.Mechanics.Add(newUser);
		_dbContext.SaveChanges();

		var claims = new List<Claim>
		{
			new Claim(ClaimTypes.NameIdentifier, dbUser.Entity.Id.ToString()),
			new Claim(ClaimTypes.Role, Roles.MECHANIC)
		};

		return GenerateSessionFromClaims(claims, dbUser.Entity.Id);
	}

	public SessionDto RefreshSession(SessionDto dto)
    {
		var refreshToken = _dbContext.RefreshTokens.FirstOrDefault(
			m => m.Token == dto.RefreshToken && m.IsValid && m.Jwt == dto.Jwt
			);

		if(refreshToken is null)
        {
			throw new BadRequestException("Invalid refresh token or token already used");
        }

		var tokenHandler = new JwtSecurityTokenHandler();
		var oldToken = tokenHandler.ReadJwtToken(dto.Jwt);
		var userId = oldToken.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;

		var claims = oldToken.Claims.ToList();

		refreshToken.IsValid = false;
		_dbContext.RefreshTokens.Update(refreshToken);
		_dbContext.SaveChanges();

		return GenerateSessionFromClaims(claims, Int32.Parse(userId));
	}

	private SessionDto GenerateSessionFromClaims(List<Claim> claims, int userId)
	{
		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
		var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
		var expires = DateTime.Now.AddMinutes(_authenticationSettings.JwtExpireMinutes);

		var token = new JwtSecurityToken(
			_authenticationSettings.JwtIssuer,
			_authenticationSettings.JwtIssuer,
			claims,
			expires: expires,
			signingCredentials: credentials
			);

		var tokenHandler = new JwtSecurityTokenHandler();
		var tokenString = tokenHandler.WriteToken(token);
		var refreshToken = new RefreshToken()
		{
			Token = GenerateRandomString(32),
			Jwt = tokenString,
			IsValid = true,
			UserId = userId
		};

		_dbContext.RefreshTokens.Add(refreshToken);
		_dbContext.SaveChanges();

		var session = new SessionDto()
		{
			Jwt = tokenString,
			RefreshToken = refreshToken.Token
		};
		return session;
	}

	private void VerifyCredentials(LoginDto dto, User? user)
	{
		if (user is null)
		{
			throw new BadRequestException("Invalid email or password");
		}

		var passwordVerificationResult = _userPasswordHasher
			.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

		if (passwordVerificationResult == PasswordVerificationResult.Failed)
		{
			throw new BadRequestException("Invalid email or password");
		}
	}

	private string GenerateRandomString(int size)
    {
		char[] chars =
			"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();

		byte[] data = new byte[4 * size];
		using (var crypto = RandomNumberGenerator.Create())
		{
			crypto.GetBytes(data);
		}
		StringBuilder result = new StringBuilder(size);
		for (int i = 0; i < size; i++)
		{
			var rnd = BitConverter.ToUInt32(data, i * 4);
			var idx = rnd % chars.Length;

			result.Append(chars[idx]);
		}

		return result.ToString();
	}
}



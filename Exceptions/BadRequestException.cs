using System;
namespace AutoMobileBackend.Exceptions;

public class BadRequestException : Exception
{
	public BadRequestException(string message) : base(message)
	{
	}
}


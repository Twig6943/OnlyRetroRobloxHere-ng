using System;

namespace OnlyRetroRobloxHere.Launcher;

internal class DeprecatedApiException : Exception
{
	public DeprecatedApiException()
	{
	}

	public DeprecatedApiException(string message)
		: base(message)
	{
	}

	public DeprecatedApiException(string message, Exception inner)
		: base(message, inner)
	{
	}
}

using System.Collections.Generic;
using System.Net;

namespace OnlyRetroRobloxHere.ServerAuthInterface;

internal class AuthService
{
	private List<IPAddress> _authorisedIps = new List<IPAddress>
	{
		IPAddress.Parse("127.0.0.1"),
		IPAddress.Parse("0.0.0.1"),
		IPAddress.Parse("::1")
	};

	public static AuthService Instance { get; } = new AuthService();

	public bool IsIPAuthorised(IPAddress ip)
	{
		return _authorisedIps.Contains(ip);
	}

	public void AuthoriseIP(IPAddress ip)
	{
		_authorisedIps.Add(ip);
	}
}

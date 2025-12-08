using System.Collections.Generic;
using System.Linq;

namespace OnlyRetroRobloxHere.Launcher;

internal static class Müllschutz
{
	public static IReadOnlyCollection<string> WhitelistedClients = (IReadOnlyCollection<string>)(object)new string[22]
	{
		"2007E", "2007E-FakeFeb", "2007M", "2007L", "2008E", "2008M", "2008L", "2009E", "2009M", "2009L",
		"2010E", "2010M", "2010L", "2011E", "2011M", "2011L", "2012E", "2012M", "2012L", "2013E",
		"2013M", "2013L"
	};

	public static bool IsWhitelisted(string name)
	{
		return WhitelistedClients.Contains(name);
	}
}

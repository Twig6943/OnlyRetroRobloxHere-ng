using System.Collections.Generic;
using OnlyRetroRobloxHere.Common.Enums;
using OnlyRetroRobloxHere.Common.Models;

namespace OnlyRetroRobloxHere.Launcher.Comparer;

internal class ClientNameComparer : IComparer<string>
{
	public int Compare(string? x, string? y)
	{
		if (x == null && y == null)
		{
			return 0;
		}
		if (x != null && y == null)
		{
			return 1;
		}
		if (x == null && y != null)
		{
			return -1;
		}
		ClientYear clientYear = new ClientYear(x);
		ClientYear clientYear2 = new ClientYear(y);
		if (clientYear.IsBlank && clientYear2.IsBlank)
		{
			return x.CompareTo(y);
		}
		if (!clientYear.IsBlank && clientYear2.IsBlank)
		{
			return -1;
		}
		if (clientYear.IsBlank && !clientYear2.IsBlank)
		{
			return 1;
		}
		if (clientYear.Year > clientYear2.Year)
		{
			return 1;
		}
		if (clientYear.Year < clientYear2.Year)
		{
			return -1;
		}
		if (clientYear.Era == YearQuarter.Late && clientYear2.Era == YearQuarter.Mid)
		{
			return 1;
		}
		if (clientYear2.Era == YearQuarter.Late && clientYear.Era == YearQuarter.Mid)
		{
			return -1;
		}
		return x.CompareTo(y);
	}
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlyRetroRobloxHere.Launcher.Extensions;

internal static class ListEx
{
	public static List<T> ListCopy<T>(this List<T> listToClone)
	{
		return listToClone.ToList();
	}

	public static List<T> SortLinq<T>(this List<T> list)
	{
		list.Sort();
		return list;
	}

	public static List<T> SortLinq<T>(this List<T> list, Comparison<T> comparison)
	{
		list.Sort(comparison);
		return list;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlyRetroRobloxHere.Launcher.Extensions;

internal static class IListEx
{
	private static Random _random = new Random();

	public static IList<T> Shuffle<T>(this IList<T> list)
	{
		int num = list.Count;
		while (num > 1)
		{
			num--;
			int index = _random.Next(num + 1);
			T value = list[index];
			list[index] = list[num];
			list[num] = value;
		}
		return list;
	}

	public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
	{
		return listToClone.Select((T item) => (T)item.Clone()).ToList();
	}

	public static IList<T> ListCopy<T>(this IList<T> listToClone)
	{
		return listToClone.ToList();
	}

	public static void LinearAdd<T>(this IList<T> list, T item) where T : IComparable<T>
	{
		for (int i = 0; i < list.Count; i++)
		{
			T other = list[i];
			if (item.CompareTo(other) != 1)
			{
				list.Insert(i, item);
				return;
			}
		}
		list.Add(item);
	}

	public static void LinearAdd<T>(this IList<T> list, T item, IComparer<T> comparer)
	{
		for (int i = 0; i < list.Count; i++)
		{
			T y = list[i];
			if (comparer.Compare(item, y) != 1)
			{
				list.Insert(i, item);
				return;
			}
		}
		list.Add(item);
	}

	public static void LinearAdd<T>(this IList<T> list, T item, Comparison<T> comparer)
	{
		for (int i = 0; i < list.Count; i++)
		{
			T y = list[i];
			if (comparer(item, y) != 1)
			{
				list.Insert(i, item);
				return;
			}
		}
		list.Add(item);
	}

	public static void BinaryAdd<T>(this IList<T> list, T item) where T : IComparable<T>
	{
		int num = list.ToList().BinarySearch(item);
		if (num < 0)
		{
			num = ~num;
		}
		list.Insert(num, item);
	}

	public static void BinaryAdd<T>(this IList<T> list, T item, IComparer<T> comparer)
	{
		int num = list.ToList().BinarySearch(item, comparer);
		if (num < 0)
		{
			num = ~num;
		}
		list.Insert(num, item);
	}
}

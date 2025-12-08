using System;
using System.Collections.Generic;
using System.Linq;
using OnlyRetroRobloxHere.Common.Enums;
using OnlyRetroRobloxHere.Launcher.Extensions;
using OnlyRetroRobloxHere.Launcher.Models;

namespace OnlyRetroRobloxHere.Launcher;

internal static class Catalog
{
	private static Random _random;

	public static Dictionary<AvatarAssetType, List<AvatarItem>> ItemsCategorised { get; }

	public static List<AvatarItem> GetRecommendations(AvatarItem item, int count)
	{
		List<AvatarItem> list = ItemsCategorised[item.Type];
		if (list.Count() - 1 <= count)
		{
			List<AvatarItem> list2 = list.ListCopy();
			list2.Remove(item);
			return list2.Shuffle().ToList();
		}
		List<AvatarItem> list3 = new List<AvatarItem>();
		List<AvatarItem> list4 = list.ListCopy();
		while (list3.Count != count)
		{
			int index = _random.Next(list4.Count());
			AvatarItem avatarItem = list4[index];
			if (!(avatarItem == item) && !list3.Contains(avatarItem))
			{
				list3.Add(avatarItem);
				list4.RemoveAt(index);
			}
		}
		return list3;
	}

	private static int CreateFeaturedSeed()
	{
		int num = DateTime.Now.Year * 10000;
		int num2 = DateTime.Now.Month * 100;
		int day = DateTime.Now.Day;
		return num + num2 + day;
	}

	public static List<AvatarItem> GetFeatured()
	{
		List<AvatarItem> list = new List<AvatarItem>();
		Random random = new Random(CreateFeaturedSeed());
		for (int i = 0; i < 18; i++)
		{
			double num = random.NextDouble();
			AvatarAssetType key = ((num <= 0.6) ? AvatarAssetType.Hat : ((num <= 0.75) ? AvatarAssetType.Face : ((!(num <= 0.9)) ? AvatarAssetType.Gear : AvatarAssetType.Package)));
			AvatarItem item;
			do
			{
				int index = random.Next(ItemsCategorised[key].Count);
				item = ItemsCategorised[key][index];
			}
			while (list.Contains(item));
			list.Add(item);
		}
		return list;
	}

	private static void SortAssetDatabase()
	{
		AvatarAssetType[] values = Enum.GetValues<AvatarAssetType>();
		foreach (AvatarAssetType key in values)
		{
			ItemsCategorised[key] = new List<AvatarItem>();
		}
		foreach (KeyValuePair<ulong, AvatarItem> item in AvatarItems.Database)
		{
			ItemsCategorised[item.Value.Type].Add(item.Value);
		}
	}

	static Catalog()
	{
		_random = new Random();
		ItemsCategorised = new Dictionary<AvatarAssetType, List<AvatarItem>>();
		SortAssetDatabase();
	}
}

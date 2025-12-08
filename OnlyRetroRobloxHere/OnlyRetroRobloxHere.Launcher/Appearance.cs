using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OnlyRetroRobloxHere.Common;
using OnlyRetroRobloxHere.Common.Collection;
using OnlyRetroRobloxHere.Common.Enums;
using OnlyRetroRobloxHere.Launcher.Models;

namespace OnlyRetroRobloxHere.Launcher;

internal static class Appearance
{
	private static object _lock;

	private const bool EnableTeakettleConversion = true;

	public static ObservableCollection<AvatarItem> Inventory { get; }

	public static ObservableDictionary<AvatarSlot, AvatarItem> Equipped { get; }

	public static ObservableCollection<AvatarItem> Unequipped { get; }

	public static void Equip(ulong id)
	{
		lock (_lock)
		{
			AvatarItem byId = AvatarItems.GetById(id);
			if ((object)byId != null)
			{
				Equip(byId);
			}
		}
	}

	public static void Equip(AvatarItem item)
	{
		lock (_lock)
		{
			if (item.Type == AvatarAssetType.Package)
			{
				EquipPackage(item);
			}
			else
			{
				Equip(GetSuitableAvatarSlot(item.Type), item);
			}
		}
	}

	private static void Equip(AvatarSlot slot, ulong id)
	{
		lock (_lock)
		{
			AvatarItem byId = AvatarItems.GetById(id);
			if ((object)byId != null)
			{
				Equip(slot, byId);
			}
		}
	}

	private static void Equip(AvatarSlot slot, AvatarItem item)
	{
		lock (_lock)
		{
			if (OwnsItem(item) || item.Custom)
			{
				Unequip(slot);
				Equipped[slot] = item;
				Unequipped.Remove(item);
			}
		}
	}

	private static bool Unequip(AvatarSlot slot, bool resortHats)
	{
		lock (_lock)
		{
			if (!Equipped.ContainsKey(slot))
			{
				return false;
			}
			AvatarItem avatarItem = Equipped[slot];
			Equipped.Remove(slot);
			if (!avatarItem.Custom)
			{
				Unequipped.Add(avatarItem);
			}
			if (resortHats && slot.IsHat())
			{
				ReorderHatSlots();
			}
			return true;
		}
	}

	public static bool Unequip(AvatarSlot slot)
	{
		return Unequip(slot, resortHats: true);
	}

	public static bool Unequip(AvatarItem item)
	{
		lock (_lock)
		{
			foreach (KeyValuePair<AvatarSlot, AvatarItem> item2 in Equipped)
			{
				if (item2.Value == item)
				{
					return Unequip(item2.Key);
				}
			}
			return false;
		}
	}

	public static bool Unequip(ulong id)
	{
		lock (_lock)
		{
			foreach (KeyValuePair<AvatarSlot, AvatarItem> item in Equipped)
			{
				if (item.Value.Id == id)
				{
					return Unequip(item.Key);
				}
			}
			return false;
		}
	}

	public static bool UnequipAll()
	{
		lock (_lock)
		{
			AvatarSlot[] values = Enum.GetValues<AvatarSlot>();
			for (int i = 0; i < values.Length; i++)
			{
				Unequip(values[i], resortHats: false);
			}
			return true;
		}
	}

	private static void EquipPackage(AvatarItem packageItem)
	{
		lock (_lock)
		{
			if (packageItem.Items == null)
			{
				throw new Exception("Package is missing it's items!");
			}
			IEnumerable<AvatarItem> avatarItemsFromIds = GetAvatarItemsFromIds(packageItem.Items.Except(GetEquippedIds()));
			GiveItemsFromList(avatarItemsFromIds);
			int num = avatarItemsFromIds.Where((AvatarItem x) => x.Type == AvatarAssetType.Hat).Count();
			if (num == 2)
			{
				Unequip(AvatarSlot.Hat3);
				Unequip(AvatarSlot.Hat2);
			}
			else if (num >= 3)
			{
				Unequip(AvatarSlot.Hat3);
				Unequip(AvatarSlot.Hat2);
				Unequip(AvatarSlot.Hat1);
			}
			foreach (AvatarItem item in avatarItemsFromIds)
			{
				Equip(item);
			}
		}
	}

	public static IEnumerable<ulong> GetEquippedIds()
	{
		return Equipped.Values.Select((AvatarItem x) => x.Id);
	}

	private static void GiveItemsFromList(IEnumerable<AvatarItem> items)
	{
		lock (_lock)
		{
			foreach (AvatarItem item in items)
			{
				AwardItem(item);
			}
		}
	}

	private static IEnumerable<AvatarItem> GetAvatarItemsFromIds(IEnumerable<ulong> ids)
	{
		lock (_lock)
		{
			List<AvatarItem> list = new List<AvatarItem>();
			foreach (ulong id in ids)
			{
				AvatarItem byId = AvatarItems.GetById(id);
				if ((object)byId != null)
				{
					list.Add(byId);
				}
			}
			return list;
		}
	}

	private static void ReorderHatSlots()
	{
		lock (_lock)
		{
			if (!Equipped.ContainsKey(AvatarSlot.Hat1) && Equipped.ContainsKey(AvatarSlot.Hat2))
			{
				Equipped[AvatarSlot.Hat1] = Equipped[AvatarSlot.Hat2];
				Equipped.Remove(AvatarSlot.Hat2);
			}
			if (!Equipped.ContainsKey(AvatarSlot.Hat2) && Equipped.ContainsKey(AvatarSlot.Hat3))
			{
				Equipped[AvatarSlot.Hat2] = Equipped[AvatarSlot.Hat3];
				Equipped.Remove(AvatarSlot.Hat3);
			}
		}
	}

	public static bool OwnsItem(ulong id)
	{
		lock (_lock)
		{
			foreach (AvatarItem item in Inventory)
			{
				if (item.Id == id)
				{
					return true;
				}
			}
			return false;
		}
	}

	public static bool OwnsItem(AvatarItem item)
	{
		return Inventory.Contains(item);
	}

	public static bool OwnsItemWithSubItems(ulong id)
	{
		AvatarItem byId = AvatarItems.GetById(id);
		if (byId == null)
		{
			return false;
		}
		return OwnsItemWithSubItems(byId);
	}

	public static bool OwnsItemWithSubItems(AvatarItem item)
	{
		if (!OwnsItem(item))
		{
			return false;
		}
		if (item.Type != AvatarAssetType.Package)
		{
			return true;
		}
		if (item.Items == null)
		{
			throw new Exception("Package is missing it's items!");
		}
		foreach (ulong item2 in item.Items)
		{
			if (!OwnsItem(item2))
			{
				return false;
			}
		}
		return true;
	}

	public static bool TryGetInventoryItem(ulong id, out AvatarItem item)
	{
		lock (_lock)
		{
			foreach (AvatarItem item2 in Inventory)
			{
				if (item2.Id == id)
				{
					item = item2;
					return true;
				}
			}
			item = null;
			return false;
		}
	}

	private static void AddItemToInventory(AvatarItem item)
	{
		lock (_lock)
		{
			Inventory.Add(item);
			Unequipped.Add(item);
		}
	}

	private static bool AwardItemPackage(AvatarItem item)
	{
		lock (_lock)
		{
			bool result = false;
			if (!OwnsItem(item))
			{
				result = true;
				AddItemToInventory(item);
			}
			if (item.Items != null)
			{
				foreach (ulong item2 in item.Items)
				{
					if (AwardItem(item2))
					{
						result = true;
					}
				}
			}
			return result;
		}
	}

	public static bool AwardItem(AvatarItem item)
	{
		lock (_lock)
		{
			if (item.Type == AvatarAssetType.Package)
			{
				return AwardItemPackage(item);
			}
			if (OwnsItem(item))
			{
				return false;
			}
			AddItemToInventory(item);
			return true;
		}
	}

	public static bool AwardItem(ulong id)
	{
		lock (_lock)
		{
			AvatarItem byId = AvatarItems.GetById(id);
			if (byId == null)
			{
				return false;
			}
			return AwardItem(byId);
		}
	}

	public static bool RemoveItem(AvatarItem item)
	{
		lock (_lock)
		{
			Unequip(item);
			Unequipped.Remove(item);
			Inventory.Remove(item);
			return true;
		}
	}

	public static bool RemoveItem(ulong id)
	{
		lock (_lock)
		{
			if (!TryGetInventoryItem(id, out AvatarItem item))
			{
				return false;
			}
			return RemoveItem(item);
		}
	}

	public static AvatarSlot GetSuitableAvatarSlot(AvatarAssetType type)
	{
		lock (_lock)
		{
			switch (type)
			{
			case AvatarAssetType.TShirt:
				return AvatarSlot.TShirt;
			case AvatarAssetType.Shirt:
				return AvatarSlot.Shirt;
			case AvatarAssetType.Pants:
				return AvatarSlot.Pants;
			case AvatarAssetType.Hat:
				if (!Equipped.ContainsKey(AvatarSlot.Hat1))
				{
					return AvatarSlot.Hat1;
				}
				if (!Equipped.ContainsKey(AvatarSlot.Hat2))
				{
					return AvatarSlot.Hat2;
				}
				return AvatarSlot.Hat3;
			case AvatarAssetType.Face:
				return AvatarSlot.Face;
			case AvatarAssetType.Head:
				return AvatarSlot.Head;
			case AvatarAssetType.Torso:
				return AvatarSlot.Torso;
			case AvatarAssetType.LeftArm:
				return AvatarSlot.LeftArm;
			case AvatarAssetType.RightArm:
				return AvatarSlot.RightArm;
			case AvatarAssetType.LeftLeg:
				return AvatarSlot.LeftLeg;
			case AvatarAssetType.RightLeg:
				return AvatarSlot.RightLeg;
			case AvatarAssetType.Gear:
				return AvatarSlot.Gear;
			default:
				throw new Exception("Unknown avatar asset type");
			}
		}
	}

	public static List<AvatarItem> GetUnequippedAvatarItemWithType(AvatarAssetType type)
	{
		List<AvatarItem> list = new List<AvatarItem>();
		foreach (AvatarItem item in Unequipped)
		{
			if (item.Type == type)
			{
				list.Add(item);
			}
		}
		return list;
	}

	private static ulong LoadConvertId(ulong id)
	{
		if (id == 1374320)
		{
			Logger.Instance.Info("Converting Teakettle!");
			return 1374328uL;
		}
		return id;
	}

	private static void LoadInventoryListsFromSettings()
	{
		Dictionary<AvatarSlot, ulong>.ValueCollection values = Settings.Default.Character.Equipped.Values;
		foreach (ulong item in Settings.Default.Inventory.Distinct())
		{
			ulong num = LoadConvertId(item);
			AvatarItem byId = AvatarItems.GetById(num);
			if ((object)byId != null)
			{
				Inventory.Add(byId);
				if (!values.Contains(num))
				{
					Unequipped.Add(byId);
				}
			}
		}
		foreach (KeyValuePair<AvatarSlot, ulong> item2 in Settings.Default.Character.Equipped)
		{
			ulong id = LoadConvertId(item2.Value);
			AvatarItem byId2 = AvatarItems.GetById(id);
			if ((object)byId2 != null)
			{
				if (byId2.Type.IsSlotValid(item2.Key))
				{
					Equipped[item2.Key] = byId2;
					if (!Inventory.Contains(byId2))
					{
						Inventory.Add(byId2);
					}
				}
			}
			else
			{
				AvatarAssetType type = item2.Key.ConvertToAvatarAssetType();
				if (type.CanHaveCustomAssets())
				{
					Equipped[item2.Key] = AvatarItem.GetCustom(id, type);
				}
			}
		}
	}

	private static void OnSettingsPreSerialization(Settings settings)
	{
		settings.Character.Equipped.Clear();
		foreach (KeyValuePair<AvatarSlot, AvatarItem> item in Equipped)
		{
			settings.Character.Equipped[item.Key] = item.Value.Id;
		}
		settings.Inventory.Clear();
		foreach (AvatarItem item2 in Inventory)
		{
			settings.Inventory.Add(item2.Id);
		}
	}

	static Appearance()
	{
		_lock = new object();
		Inventory = new ObservableCollection<AvatarItem>();
		Equipped = new ObservableDictionary<AvatarSlot, AvatarItem>();
		Unequipped = new ObservableCollection<AvatarItem>();
		LoadInventoryListsFromSettings();
		Settings.Default.PreSerialization += OnSettingsPreSerialization;
	}
}

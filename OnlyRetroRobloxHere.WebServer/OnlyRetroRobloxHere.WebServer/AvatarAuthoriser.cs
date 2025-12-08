using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlyRetroRobloxHere.Common.Enums;
using OnlyRetroRobloxHere.WebServer.Models;

namespace OnlyRetroRobloxHere.WebServer;

internal static class AvatarAuthoriser
{
	private static Dictionary<ulong, AvatarAssetType> _whitelistedAssets = new Dictionary<ulong, AvatarAssetType>();

	private static List<ulong> _blacklistedAssets = new List<ulong>();

	private static bool CanAddType(List<AvatarAssetType> usedTypes, AvatarAssetType type)
	{
		if (type == AvatarAssetType.Hat)
		{
			return usedTypes.Where((AvatarAssetType x) => x == AvatarAssetType.Hat).Count() <= 3;
		}
		return !usedTypes.Contains(type);
	}

	private static AvatarAssetType? GetWhitelistedAvatarAssetTypeFromId(int id)
	{
		return id switch
		{
			2 => AvatarAssetType.TShirt, 
			11 => AvatarAssetType.Shirt, 
			12 => AvatarAssetType.Pants, 
			_ => null, 
		};
	}

	private static async Task GetInfoAboutAssets(IEnumerable<ulong> ids)
	{
		foreach (AssetInformation item in await AssetDelivery.BatchRequest(ids))
		{
			AvatarAssetType? whitelistedAvatarAssetTypeFromId = GetWhitelistedAvatarAssetTypeFromId(item.AssetTypeId);
			ulong num = ulong.Parse(item.RequestId);
			if (whitelistedAvatarAssetTypeFromId.HasValue)
			{
				_whitelistedAssets[num] = whitelistedAvatarAssetTypeFromId.Value;
			}
			else
			{
				_blacklistedAssets.Add(num);
			}
		}
	}

	public static IEnumerable<ulong> FilterUnsafeAssets(IEnumerable<ulong> assets)
	{
		List<ulong> list = assets.ToList();
		List<ulong> list2 = new List<ulong>();
		List<AvatarAssetType> list3 = new List<AvatarAssetType>();
		foreach (ulong asset in assets)
		{
			AvatarItem byId = AvatarItems.GetById(asset);
			if (byId != null)
			{
				if (CharacterCompatibility.IsCompatible(byId.Type) && CanAddType(list3, byId.Type))
				{
					list2.Add(asset);
					list3.Add(byId.Type);
				}
				list.Remove(asset);
			}
			else if (_whitelistedAssets.ContainsKey(asset))
			{
				AvatarAssetType avatarAssetType = _whitelistedAssets[asset];
				if (CanAddType(list3, avatarAssetType))
				{
					list2.Add(asset);
					list3.Add(avatarAssetType);
				}
				list.Remove(asset);
			}
			else if (_blacklistedAssets.Contains(asset))
			{
				list.Remove(asset);
			}
		}
		if (!list.Any())
		{
			return list2;
		}
		Task infoAboutAssets = GetInfoAboutAssets(list);
		infoAboutAssets.Wait();
		foreach (ulong item in list)
		{
			if (_whitelistedAssets.ContainsKey(item))
			{
				AvatarAssetType avatarAssetType2 = _whitelistedAssets[item];
				if (CharacterCompatibility.IsCompatible(avatarAssetType2) && CanAddType(list3, avatarAssetType2))
				{
					list2.Add(item);
					list3.Add(avatarAssetType2);
				}
			}
		}
		return list2;
	}
}

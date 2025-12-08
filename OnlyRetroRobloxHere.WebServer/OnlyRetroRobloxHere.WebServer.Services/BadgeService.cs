using System.Collections.Generic;

namespace OnlyRetroRobloxHere.WebServer.Services;

public class BadgeService
{
	public static BadgeService Instance { get; }

	public Dictionary<int, List<int>> AwardedBadges { get; } = new Dictionary<int, List<int>>();

	private List<int> GetPlayerBadges(int id)
	{
		if (!AwardedBadges.ContainsKey(id))
		{
			AwardedBadges[id] = new List<int>();
		}
		return AwardedBadges[id];
	}

	private List<int>? GetPlayerBadgesIfExists(int id)
	{
		if (AwardedBadges.ContainsKey(id))
		{
			return AwardedBadges[id];
		}
		return null;
	}

	public string AwardBadge(int userId, int badgeId)
	{
		lock (AwardedBadges)
		{
			if (!PlaceMetadata.Default.Badges.ContainsKey(badgeId))
			{
				return "0";
			}
			string value = PlaceMetadata.Default.Badges[badgeId];
			if (HasBadge(userId, badgeId))
			{
				return "0";
			}
			List<int> playerBadges = GetPlayerBadges(userId);
			playerBadges.Add(badgeId);
			string value2 = PlayerTrackingService.Default.GetPlayerNameFromIdSafe(userId) ?? "MISSINGNO";
			string creator = PlaceMetadata.Default.Creator;
			return $"{value2} won {creator}'s \"{value}\" award!";
		}
	}

	public bool HasBadge(int userId, int badgeId)
	{
		return GetPlayerBadgesIfExists(userId)?.Contains(badgeId) ?? false;
	}

	static BadgeService()
	{
		Instance = new BadgeService();
	}
}

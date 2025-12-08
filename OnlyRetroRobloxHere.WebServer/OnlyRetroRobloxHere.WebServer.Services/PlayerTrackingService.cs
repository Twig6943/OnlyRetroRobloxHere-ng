using System.Collections.Generic;
using System.Threading;

namespace OnlyRetroRobloxHere.WebServer.Services;

internal class PlayerTrackingService
{
	private const int MaxPlayerNameRetries = 3;

	private Dictionary<int, string> _Players = new Dictionary<int, string>();

	public static PlayerTrackingService Default { get; set; } = new PlayerTrackingService();

	public void RegisterPlayer(int userId, string name)
	{
		_Players[userId] = name;
	}

	public void UnregisterPlayer(int userId)
	{
		if (_Players.ContainsKey(userId))
		{
			_Players.Remove(userId);
		}
	}

	public string? GetPlayerNameFromId(int userId)
	{
		if (!_Players.ContainsKey(userId))
		{
			return null;
		}
		return _Players[userId];
	}

	public string? GetPlayerNameFromIdSafe(int userId)
	{
		string playerNameFromId = GetPlayerNameFromId(userId);
		if (playerNameFromId != null)
		{
			return playerNameFromId;
		}
		for (int i = 0; i < 3; i++)
		{
			Thread.Sleep(100);
			playerNameFromId = GetPlayerNameFromId(userId);
			if (playerNameFromId != null)
			{
				return playerNameFromId;
			}
		}
		return null;
	}

	public int? GetPlayerIdFromName(string userName)
	{
		foreach (KeyValuePair<int, string> player in _Players)
		{
			if (player.Value == userName)
			{
				return player.Key;
			}
		}
		return null;
	}
}

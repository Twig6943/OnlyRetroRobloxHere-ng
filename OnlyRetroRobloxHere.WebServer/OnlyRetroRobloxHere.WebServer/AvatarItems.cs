using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using OnlyRetroRobloxHere.Common;
using OnlyRetroRobloxHere.WebServer.Models;

namespace OnlyRetroRobloxHere.WebServer;

internal static class AvatarItems
{
	internal class AssetDatabase
	{
		[JsonPropertyName("data")]
		public Dictionary<ulong, AvatarItem> Data { get; set; } = new Dictionary<ulong, AvatarItem>();
	}

	public static Dictionary<ulong, AvatarItem> Database { get; private set; }

	private static void ParseDatabase()
	{
		string path = Path.Combine(PathHelper.Data, "assets.json");
		if (!File.Exists(path))
		{
			throw new Exception("Could not find assets database");
		}
		AssetDatabase assetDatabase = JsonSerializer.Deserialize<AssetDatabase>(File.ReadAllText(path));
		if (assetDatabase == null)
		{
			throw new Exception("Parsed asset database is null");
		}
		foreach (KeyValuePair<ulong, AvatarItem> datum in assetDatabase.Data)
		{
			datum.Value.Id = datum.Key;
			Database[datum.Key] = datum.Value;
		}
	}

	public static AvatarItem? GetById(ulong id)
	{
		if (!Database.ContainsKey(id))
		{
			return null;
		}
		return Database[id];
	}

	public static bool TryGetById(ulong id, [NotNullWhen(true)] out AvatarItem item)
	{
		item = GetById(id);
		return item != null;
	}

	static AvatarItems()
	{
		Database = new Dictionary<ulong, AvatarItem>();
		ParseDatabase();
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using OnlyRetroRobloxHere.Common;

namespace OnlyRetroRobloxHere.WebServer;

internal static class AssetVersionIdHelper
{
	public static IReadOnlyDictionary<ulong, ulong> Map { get; }

	static AssetVersionIdHelper()
	{
		string text = Path.Combine(PathHelper.Data, "avid_map.json");
		if (!File.Exists(text))
		{
			throw new Exception("Could not find asset version id map: " + text);
		}
		string json = File.ReadAllText(text);
		Dictionary<ulong, ulong> dictionary = JsonSerializer.Deserialize<Dictionary<ulong, ulong>>(json);
		if (dictionary == null)
		{
			throw new Exception("Deserialised map data is null");
		}
		Map = dictionary;
	}
}

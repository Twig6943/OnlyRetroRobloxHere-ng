using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using OnlyRetroRobloxHere.Common;

namespace OnlyRetroRobloxHere.WebServer;

public class PlaceMetadata
{
	public static PlaceMetadata Default { get; }

	[JsonPropertyName("creator")]
	public string Creator { get; set; } = "Unknown";

	[JsonPropertyName("badges")]
	public Dictionary<int, string> Badges { get; set; } = new Dictionary<int, string>();

	private static string? GetBadgeMetadataPath()
	{
		string selectedMap = Config.Instance.User.Launch.SelectedMap;
		if (string.IsNullOrEmpty(selectedMap))
		{
			return null;
		}
		string text = Path.Combine(Utils.GetMapsDirectory(), selectedMap);
		if (text.EndsWith(".gz"))
		{
			string text2 = text;
			text = text2.Substring(0, text2.Length - 3);
		}
		return text + ".meta.json";
	}

	private static PlaceMetadata GetMetadata()
	{
		string badgeMetadataPath = GetBadgeMetadataPath();
		if (string.IsNullOrEmpty(badgeMetadataPath))
		{
			Logger.Instance.Warn("Failed to get place metadata: No selected map found.");
			return new PlaceMetadata();
		}
		if (!File.Exists(badgeMetadataPath))
		{
			return new PlaceMetadata();
		}
		try
		{
			PlaceMetadata placeMetadata = JsonSerializer.Deserialize<PlaceMetadata>(File.ReadAllText(badgeMetadataPath));
			if (placeMetadata != null)
			{
				Logger.Instance.Info($"Got place metadata! {placeMetadata.Badges.Count} badges.");
				return placeMetadata;
			}
		}
		catch (Exception value)
		{
			Logger.Instance.Warn($"Failed to get place metadata: Exception while parsing: {value}");
			return new PlaceMetadata();
		}
		Logger.Instance.Warn("Failed to get place metadata: parsed metadata is null");
		return new PlaceMetadata();
	}

	static PlaceMetadata()
	{
		Default = GetMetadata();
	}
}

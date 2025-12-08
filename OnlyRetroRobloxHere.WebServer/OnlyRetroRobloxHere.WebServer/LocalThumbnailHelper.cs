using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using OnlyRetroRobloxHere.Common;

namespace OnlyRetroRobloxHere.WebServer;

internal class LocalThumbnailHelper
{
	private Dictionary<ulong, string> _registeredMap = new Dictionary<ulong, string>();

	public static LocalThumbnailHelper Instance { get; }

	private LocalThumbnailHelper()
	{
		ParseClientExclusiveThumbnails();
	}

	public byte[]? GetThumbnailData(ulong id)
	{
		if (_registeredMap.ContainsKey(id))
		{
			return File.ReadAllBytes(_registeredMap[id]);
		}
		return null;
	}

	private void AddFolderFilesToMap(string folder)
	{
		if (!Directory.Exists(folder))
		{
			Logger.Instance.Warn("ThumbnailHandler: folder does not exist: " + folder);
			return;
		}
		string[] files = Directory.GetFiles(folder);
		foreach (string text in files)
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text);
			if (ulong.TryParse(fileNameWithoutExtension, out var result))
			{
				_registeredMap[result] = text;
			}
			else
			{
				Logger.Instance.Warn("ThumbnailHandler: file name is not a valid ID: " + text);
			}
		}
	}

	private void ParseClientExclusiveThumbnails()
	{
		string path = Path.Combine(PathHelper.Thumbnails, "clients");
		string path2 = Path.Combine(path, "metadata.json");
		if (!File.Exists(path2))
		{
			Logger.Instance.Warn("Could not find CEThumbs metadata");
			return;
		}
		Dictionary<string, string[]> dictionary = JsonSerializer.Deserialize<Dictionary<string, string[]>>(File.ReadAllText(path2));
		if (dictionary == null)
		{
			Logger.Instance.Warn("Failed to parse CEThumbs");
			return;
		}
		string value = Config.Instance.Client.ClientYear.ToString().ToUpperInvariant();
		foreach (KeyValuePair<string, string[]> item in dictionary)
		{
			string key = item.Key;
			string[] value2 = item.Value;
			if (value2.Contains("*") || value2.Contains(value))
			{
				string folder = Path.Combine(path, key);
				AddFolderFilesToMap(folder);
			}
		}
	}

	static LocalThumbnailHelper()
	{
		Instance = new LocalThumbnailHelper();
	}
}

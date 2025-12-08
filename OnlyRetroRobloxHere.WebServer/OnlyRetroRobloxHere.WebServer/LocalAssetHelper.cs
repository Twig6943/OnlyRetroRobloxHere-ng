using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using OnlyRetroRobloxHere.Common;

namespace OnlyRetroRobloxHere.WebServer;

internal class LocalAssetHelper
{
	private enum LocalAssetDirectory : byte
	{
		Accessory,
		Core,
		Game,
		Linked,
		Music,
		Sound,
		Tool
	}

	private struct LocalAssetInfo
	{
		public string FileName;

		public LocalAssetDirectory Directory;
	}

	public struct AssetResult
	{
		public bool Success;

		public byte[] File;

		public bool Compressed;

		public bool IsLua;
	}

	private Dictionary<ulong, LocalAssetInfo> _commonMap = new Dictionary<ulong, LocalAssetInfo>();

	private Dictionary<ulong, string> _registeredCEMap = new Dictionary<ulong, string>();

	public static LocalAssetHelper Instance { get; }

	private LocalAssetHelper()
	{
		ParseCommonDirectories();
		ParseClientExclusiveAssets();
		PrintStatistics();
	}

	public AssetResult GetAssetData(ulong id)
	{
		AssetResult cEAsset = GetCEAsset(id);
		if (cEAsset.Success)
		{
			return cEAsset;
		}
		return GetCommonAsset(id);
	}

	public AssetResult GetCommonAsset(ulong id)
	{
		if (_commonMap.ContainsKey(id))
		{
			LocalAssetInfo localAssetInfo = _commonMap[id];
			string commonDirectoryPath = GetCommonDirectoryPath(localAssetInfo.Directory);
			string path = Path.Combine(commonDirectoryPath, localAssetInfo.FileName);
			if (File.Exists(path))
			{
				return new AssetResult
				{
					Success = true,
					File = File.ReadAllBytes(path),
					Compressed = localAssetInfo.FileName.EndsWith(".gz"),
					IsLua = localAssetInfo.FileName.EndsWith(".lua")
				};
			}
		}
		return new AssetResult
		{
			Success = false
		};
	}

	public AssetResult GetCEAsset(ulong id)
	{
		if (_registeredCEMap.ContainsKey(id))
		{
			string text = _registeredCEMap[id];
			if (File.Exists(text))
			{
				return new AssetResult
				{
					Success = true,
					File = File.ReadAllBytes(text),
					Compressed = text.EndsWith(".gz"),
					IsLua = text.EndsWith(".lua")
				};
			}
		}
		return new AssetResult
		{
			Success = false
		};
	}

	private void PrintStatistics()
	{
		int value = _commonMap.Count + _registeredCEMap.Count;
		Logger.Instance.Info($"LocalAssetHelper: Loaded {value} assets");
	}

	private string GetCommonDirectoryName(LocalAssetDirectory dir)
	{
		return dir.ToString().ToLowerInvariant();
	}

	private string GetCommonDirectoryPath(LocalAssetDirectory dir)
	{
		string commonDirectoryName = GetCommonDirectoryName(dir);
		return Path.Combine(PathHelper.Assets, commonDirectoryName);
	}

	private void ParseCommonDirectories()
	{
		ParseCommonDirectory(LocalAssetDirectory.Accessory);
		ParseCommonDirectory(LocalAssetDirectory.Core);
		ParseCommonDirectory(LocalAssetDirectory.Game);
		ParseCommonDirectory(LocalAssetDirectory.Linked);
		ParseCommonDirectory(LocalAssetDirectory.Music);
		ParseCommonDirectory(LocalAssetDirectory.Sound);
		ParseCommonDirectory(LocalAssetDirectory.Tool);
	}

	private void ParseCommonDirectory(LocalAssetDirectory dir)
	{
		string commonDirectoryPath = GetCommonDirectoryPath(dir);
		if (!Directory.Exists(commonDirectoryPath))
		{
			Logger.Instance.Warn("LocalAssetHelper: common dir " + commonDirectoryPath + " not found");
			return;
		}
		string[] files = Directory.GetFiles(commonDirectoryPath);
		foreach (string text in files)
		{
			string text2;
			if (!text.EndsWith(".gz"))
			{
				text2 = text;
			}
			else
			{
				string text3 = text;
				text2 = text3.Substring(0, text3.Length - 3);
			}
			string path = text2;
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
			if (!ulong.TryParse(fileNameWithoutExtension, out var result))
			{
				Logger.Instance.Warn("LocalAssetHelper: file name " + text + " is not a valid id");
				break;
			}
			_commonMap[result] = new LocalAssetInfo
			{
				FileName = Path.GetFileName(text),
				Directory = dir
			};
		}
	}

	private void AddFolderFilesToCEMap(string folder)
	{
		if (!Directory.Exists(folder))
		{
			Logger.Instance.Warn("LocalAssetHelper: folder does not exist: " + folder);
			return;
		}
		string[] files = Directory.GetFiles(folder);
		foreach (string text in files)
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text);
			if (ulong.TryParse(fileNameWithoutExtension, out var result))
			{
				_registeredCEMap[result] = text;
			}
			else
			{
				Logger.Instance.Warn("LocalAssetHelper: file name is not a valid ID: " + text);
			}
		}
	}

	private void ParseClientExclusiveAssets()
	{
		string path = Path.Combine(PathHelper.Assets, "clients");
		string path2 = Path.Combine(path, "metadata.json");
		if (!File.Exists(path2))
		{
			Logger.Instance.Warn("Could not find CEAssets metadata");
			return;
		}
		Dictionary<string, string[]> dictionary = JsonSerializer.Deserialize<Dictionary<string, string[]>>(File.ReadAllText(path2));
		if (dictionary == null)
		{
			Logger.Instance.Warn("Failed to parse CEAssets");
			return;
		}
		string value = Config.Instance.Client.ClientYear.ToString().ToUpperInvariant();
		foreach (KeyValuePair<string, string[]> item in dictionary)
		{
			string key = item.Key;
			string[] value2 = item.Value;
			if (value2.Contains(value))
			{
				string folder = Path.Combine(path, key);
				AddFolderFilesToCEMap(folder);
			}
		}
	}

	static LocalAssetHelper()
	{
		Instance = new LocalAssetHelper();
	}
}

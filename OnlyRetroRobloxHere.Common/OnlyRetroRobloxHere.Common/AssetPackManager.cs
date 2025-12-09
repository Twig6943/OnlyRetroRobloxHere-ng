using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using OnlyRetroRobloxHere.Common.Enums;
using OnlyRetroRobloxHere.Common.Models;

namespace OnlyRetroRobloxHere.Common;

/// <summary>
/// Manages the lifecycle, state, and configuration of asset packs within the application.
/// </summary>
public class AssetPackManager
{
	private ClientYear? _clientYear;

	private bool _disabledInit;

	public static AssetPackManager Instance { get; } = new AssetPackManager();

	public List<AssetPack> AssetPacks { get; set; } = new List<AssetPack>();

	public List<string> DisabledAssetPacks { get; set; } = new List<string>();

	private AssetPackManager()
	{
		ParseAssetPacks();
	}

	public void Reparse()
	{
		AssetPacks.Clear();
		ParseAssetPacks();
		ParseDisabledAssetPacks();
		CheckForDisabledClientYear();
	}

	public void SetDisabledList(List<string> disabled)
	{
		if (_disabledInit)
		{
			throw new Exception("Tried to run AssetPackManager.SetDisabledList twice");
		}
		_disabledInit = true;
		DisabledAssetPacks = disabled.ToList();
		ParseDisabledAssetPacks();
	}

	public void SetClientYear(ClientYear clientYear)
	{
		_clientYear = clientYear;
		CheckForDisabledClientYear();
	}

	public void ToggleAssetPack(AssetPack assetPack)
	{
		if (!assetPack.Disabled)
		{
			assetPack.Disabled = true;
			DisabledAssetPacks.Add(assetPack.DisplayName);
		}
		else
		{
			assetPack.Disabled = false;
			DisabledAssetPacks.Remove(assetPack.DisplayName);
		}
	}

	public List<string> GetEnabledAssetPackDirectories()
	{
		List<string> list = new List<string>();
		foreach (AssetPack assetPack in AssetPacks)
		{
			if (!assetPack.Disabled)
			{
				list.Add(assetPack.Folder);
			}
		}
		return list;
	}

	private static bool IsClientYearCompatible(ClientYear clientYear, List<string> rules)
	{
		if (!rules.Any())
		{
			return true;
		}
		bool flag = true;
		foreach (string rule in rules)
		{
			if (rule == "*")
			{
				flag = true;
			}
			else if (rule.StartsWith('!'))
			{
				string text = rule;
				ClientYear clientYear2 = new ClientYear(text.Substring(1, text.Length - 1));
				flag = clientYear != clientYear2;
				if (!flag)
				{
					break;
				}
			}
			else
			{
				ClientYear clientYear3 = new ClientYear(rule);
				flag = clientYear == clientYear3;
				if (!flag)
				{
					break;
				}
			}
		}
		return flag;
	}

	private void CheckForDisabledClientYear()
	{
		if ((object)_clientYear == null)
		{
			return;
		}
		foreach (AssetPack assetPack in AssetPacks)
		{
			if (!assetPack.Disabled && !IsClientYearCompatible(_clientYear, assetPack.Clients))
			{
				assetPack.Disabled = true;
			}
		}
	}

	private void ParseDisabledAssetPacks()
	{
		if (!DisabledAssetPacks.Any())
		{
			return;
		}
		List<string> list = new List<string>();
		foreach (AssetPack assetPack in AssetPacks)
		{
			if (DisabledAssetPacks.Contains(assetPack.DisplayName))
			{
				assetPack.Disabled = true;
				list.Add(assetPack.DisplayName);
			}
		}
		foreach (string item in DisabledAssetPacks.Except(list).ToList())
		{
			DisabledAssetPacks.Remove(item);
		}
	}

	private void ParseAssetPacks()
	{
		Directory.CreateDirectory(PathHelper.AssetPacks);
		string[] directories = Directory.GetDirectories(PathHelper.AssetPacks);
		foreach (string text in directories)
		{
			string path = Path.Combine(text, "AssetPack.json");
			string path2 = Path.Combine(text, "SodikmAssetPack.ini");
			AssetPack assetPack;
			if (File.Exists(path))
			{
				try
				{
					assetPack = JsonSerializer.Deserialize<AssetPack>(File.ReadAllText(path)) ?? throw new Exception("Deserialised asset pack JSON is null");
					assetPack.Api = AssetPackApi.V1;
				}
				catch (Exception value)
				{
					Logger.Instance.Warn($"AssetPackManager: failed to parse V1 asset pack: {value}");
					continue;
				}
			}
			else if (File.Exists(path2))
			{
				try
				{
					assetPack = IniParser.Parse<AssetPackSodikm>(File.ReadAllText(path2)).Convert();
					assetPack.Api = AssetPackApi.SodikmV1;
				}
				catch (Exception value2)
				{
					Logger.Instance.Warn($"AssetPackManager: failed to parse Sodikm V1 asset pack: {value2}");
					continue;
				}
			}
			else
			{
				assetPack = new AssetPack();
				assetPack.Api = AssetPackApi.None;
			}
			assetPack.Folder = text;
			assetPack.FolderName = Path.GetFileName(text) ?? "errfolder";
			AssetPacks.Add(assetPack);
		}
	}
}

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
/// <remarks>
/// The <see cref="AssetPackManager"/> class allows the usage of some functionality to manage asset packs, including
/// the parsing of the packs, enabling/disabling them, maintaining compatibility with client-specific rules. It is implemented with a
/// singleton, accessible via the <see cref="Instance"/> property. Asset packs are represented as <see
/// cref="AssetPack"/> objects, and their state (enabled/disabled) can be toggled.
/// This class also supports managing a list of disabled asset packs and ensures compatibility with a specified client year, 
/// if provided. Asset packs are loaded from a predefined directory structure, and their metadata is parsed from JSON or INI
/// files according to two different APIs.
/// </remarks>
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
	/// <summary>
	/// Determines whether the specified <paramref name="clientYear"/> is compatible with the given set of <paramref
	/// name="rules"/>.
	/// </summary>
	/// <remarks>The method evaluates the rules in order and stops processing as soon as a definitive compatibility
	/// result is determined.</remarks>
	/// <param name="clientYear">The client year to evaluate against the rules.</param>
	/// <param name="rules">A list of rules that define compatibility. Each rule can be: <list type="bullet"> <item><description>"*" to
	/// indicate universal compatibility.</description></item> <item><description>A version string (e.g., "2023E") to indicate
	/// compatibility with that specific version.</description></item> <item><description>A negated version string (e.g.,
	/// "!2023") to indicate incompatibility with that specific year.</description></item> </list></param>
	/// <returns><see langword="true"/> if the <paramref name="clientYear"/> satisfies the compatibility rules; otherwise, <see
	/// langword="false"/>.</returns>
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
	/// <summary>
	/// Ensures that asset packs incompatible with the current client year are marked as disabled.
	/// </summary>
	/// <remarks>
	/// This method iterates through all asset packs and disables those that are not compatible with the
	/// current client year. Compatibility is determined by the <see cref="IsClientYearCompatible"/> method. If the client
	/// year is not set, the method performs no action.
	/// </remarks>
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
	/// <summary>
	/// Updates the state of asset packs based on the list of disabled asset pack names.
	/// </summary>
	/// <remarks>
	/// This method iterates through the available asset packs and marks those whose display names are
	/// present in the <see cref="DisabledAssetPacks"/> collection as disabled. Any names in <see
	/// cref="DisabledAssetPacks"/> that do not correspond to an existing asset pack are removed from the
	/// collection.
	/// </remarks>
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

	/// <summary>
	/// Parses asset packs from the designated asset packs directory and adds them to the collection of loaded asset packs.
	/// </summary>
	/// <remarks>
	/// This method scans the asset packs directory for subdirectories containing asset pack configuration
	/// files. It supports two formats: JSON-based asset packs (V1) and INI-based asset packs (Sodikm V1). If a valid
	/// configuration  file is found, the asset pack is deserialized and added to the collection. If no configuration file
	/// is found, a default asset pack with no API version is created for the directory. 
	/// <para> Any errors encountered during deserialization are logged, and the corresponding asset pack is skipped. </para>
	/// </remarks>
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

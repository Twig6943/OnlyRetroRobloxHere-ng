using System.Collections.Generic;
using Microsoft.Win32;
using OnlyRetroRobloxHere.Launcher.Enums;

namespace OnlyRetroRobloxHere.Launcher;

internal class VCPPRedistInstallationDetector
{
	private enum RedistKeyLocation
	{
		Products,
		Dependencies
	}

	private struct RedistInformation
	{
		public RedistKeyLocation Location { get; }

		public string[] Keys { get; }

		public RedistInformation(RedistKeyLocation location, string[] keys)
		{
			Location = location;
			Keys = keys;
		}
	}

	private static Dictionary<VCPPRedist, RedistInformation> _VCRedistToRedistKeysMap;

	private static Dictionary<VCPPRedist, bool> _VCRedistResults;

	private static bool CheckIfInstallerKeyExists(RedistInformation information)
	{
		string text = information.Location.ToString();
		string[] keys = information.Keys;
		foreach (string text2 in keys)
		{
			using RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Classes\\Installer\\" + text + "\\" + text2);
			if (registryKey != null)
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsInstalled(VCPPRedist redist)
	{
		return _VCRedistResults[redist];
	}

	public static List<VCPPRedist> GetUninstalledRedists()
	{
		List<VCPPRedist> list = new List<VCPPRedist>();
		foreach (KeyValuePair<VCPPRedist, bool> vCRedistResult in _VCRedistResults)
		{
			if (!vCRedistResult.Value)
			{
				list.Add(vCRedistResult.Key);
			}
		}
		return list;
	}

	private static void CheckAllKeys()
	{
		foreach (KeyValuePair<VCPPRedist, RedistInformation> item in _VCRedistToRedistKeysMap)
		{
			VCPPRedist key = item.Key;
			bool value = CheckIfInstallerKeyExists(item.Value);
			_VCRedistResults[key] = value;
		}
	}

	static VCPPRedistInstallationDetector()
	{
		_VCRedistToRedistKeysMap = new Dictionary<VCPPRedist, RedistInformation>
		{
			[VCPPRedist.VCPP2005] = new RedistInformation(RedistKeyLocation.Products, new string[2] { "c1c4f01781cc94c4c8fb1542c0981a2a", "b25099274a207264182f8181add555d0" }),
			[VCPPRedist.VCPP2008] = new RedistInformation(RedistKeyLocation.Products, new string[3] { "6E815EB96CCE9A53884E7857C57002F0", "6F9E66FF7E38E3A3FA41D89E8A906A4A", "D20352A90C039D93DBF6126ECE614057" })
		};
		_VCRedistResults = new Dictionary<VCPPRedist, bool> { [VCPPRedist.None] = true };
		CheckAllKeys();
	}
}

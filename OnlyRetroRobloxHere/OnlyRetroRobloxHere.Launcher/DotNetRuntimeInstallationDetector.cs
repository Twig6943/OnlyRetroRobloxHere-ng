using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using OnlyRetroRobloxHere.Common;
using OnlyRetroRobloxHere.Launcher.Enums;

namespace OnlyRetroRobloxHere.Launcher;

internal class DotNetRuntimeInstallationDetector
{
	private static bool _initialised;

	private static Dictionary<DotNetRuntime, List<string>> _installedVersions;

	private static string? GetRuntimesListRaw()
	{
		try
		{
			using Process process = new Process();
			process.StartInfo.FileName = "dotnet";
			process.StartInfo.Arguments = "--list-runtimes";
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.RedirectStandardOutput = true;
			process.Start();
			string result = process.StandardOutput.ReadToEnd();
			process.WaitForExit();
			return result;
		}
		catch (Exception value)
		{
			Logger.Instance.Error($"Failed to get .NET runtimes: {value}");
			return null;
		}
	}

	private static DotNetRuntime GetDotNetRuntimeFromName(string name)
	{
		return name switch
		{
			"Microsoft.NETCore.App" => DotNetRuntime.Core, 
			"Microsoft.WindowsDesktop.App" => DotNetRuntime.Desktop, 
			"Microsoft.AspNetCore.App" => DotNetRuntime.AspNet, 
			"Microsoft.AspNetCore.All" => DotNetRuntime.AspNet, 
			_ => throw new Exception("Unknown .net namespace: " + name), 
		};
	}

	private static void CheckInstalledVersions()
	{
		string runtimesListRaw = GetRuntimesListRaw();
		if (runtimesListRaw == null)
		{
			return;
		}
		DotNetRuntime[] values = Enum.GetValues<DotNetRuntime>();
		foreach (DotNetRuntime key in values)
		{
			_installedVersions[key] = new List<string>();
		}
		string[] array = runtimesListRaw.Split('\n');
		foreach (string text in array)
		{
			if (!string.IsNullOrEmpty(text))
			{
				string[] array2 = text.Split(' ');
				if (array2.Length < 3)
				{
					Logger.Instance.Warn("Weird line: " + text);
					continue;
				}
				string name = array2[0];
				string item = array2[1];
				DotNetRuntime dotNetRuntimeFromName = GetDotNetRuntimeFromName(name);
				_installedVersions[dotNetRuntimeFromName].Add(item);
			}
		}
		_initialised = true;
	}

	public static List<DotNetRuntime> GetUninstalledRuntimes()
	{
		List<DotNetRuntime> list = new List<DotNetRuntime>();
		if (!_initialised)
		{
			return list;
		}
		foreach (KeyValuePair<DotNetRuntime, List<string>> installedVersion in _installedVersions)
		{
			if (!installedVersion.Value.Any((string x) => x.StartsWith("6.")))
			{
				list.Add(installedVersion.Key);
			}
		}
		return list;
	}

	static DotNetRuntimeInstallationDetector()
	{
		_initialised = false;
		_installedVersions = new Dictionary<DotNetRuntime, List<string>>();
		try
		{
			CheckInstalledVersions();
		}
		catch (Exception value)
		{
			Logger.Instance.Error($"DotNetRuntimeInstallationDetector::CheckInstalledVersions failed: {value}");
		}
	}
}

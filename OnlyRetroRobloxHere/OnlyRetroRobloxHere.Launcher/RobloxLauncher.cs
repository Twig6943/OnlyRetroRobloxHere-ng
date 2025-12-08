using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Windows;
using OnlyRetroRobloxHere.Common;
using OnlyRetroRobloxHere.Common.Enums;
using OnlyRetroRobloxHere.Common.Models;
using OnlyRetroRobloxHere.Launcher.Enums;
using OnlyRetroRobloxHere.Launcher.Models;
using OnlyRetroRobloxHere.Launcher.UI;
using OnlyRetroRobloxHere.Launcher.UI.Dialogs;

namespace OnlyRetroRobloxHere.Launcher;

internal static class RobloxLauncher
{
	private static ClientConfig? GetClientConfig()
	{
		if (ClientConfigManager.SelectedClientConfig == null)
		{
			Utils.ShowMessageBox("No client selected", MessageBoxButton.OK, MessageBoxImage.Hand);
			return null;
		}
		return ClientConfigManager.SelectedClientConfig;
	}

	private static string GetStudioExecutableName(ClientConfig config)
	{
		switch (config.Studio.Type)
		{
		case StudioType.App:
		case StudioType.MFC:
		case StudioType.Qt:
			return config.Studio.ExecutableName;
		case StudioType.MFCnQt:
			if (Settings.Default.Launch.PreferredStudioType != PreferredStudioType.MFC)
			{
				return config.Studio.QtExecutableName;
			}
			return config.Studio.MFCExecutableName;
		default:
			throw new NotImplementedException(config.Studio.Type.ToString());
		}
	}

	private static string GetStudioDirectory(ClientConfig config)
	{
		switch (config.Studio.Type)
		{
		case StudioType.App:
			return LaunchDirectory.Player.ToString();
		case StudioType.MFC:
		case StudioType.Qt:
			return "Studio";
		case StudioType.MFCnQt:
			return Settings.Default.Launch.PreferredStudioType.ToString() + "Studio";
		default:
			throw new NotImplementedException(config.Studio.Type.ToString());
		}
	}

	private static string GetExecutableName(ClientConfig config, LaunchType launchType)
	{
		return launchType switch
		{
			LaunchType.Play => config.Player.ExecutableName, 
			LaunchType.Studio => GetStudioExecutableName(config), 
			LaunchType.Host => config.Server.ExecutableName, 
			LaunchType.Render => GetStudioExecutableName(config), 
			_ => throw new NotImplementedException(launchType.ToString()), 
		};
	}

	private static string GetCharacterAppearanceFetchUrl()
	{
		Character character = Settings.Default.Character;
		IEnumerable<ulong> values = Appearance.Equipped.Select<KeyValuePair<AvatarSlot, AvatarItem>, ulong>((KeyValuePair<AvatarSlot, AvatarItem> x) => x.Value.Id);
		string arg = string.Join(',', values);
		string arg2 = $"{character.Head},{character.LeftArm},{character.RightArm},{character.LeftLeg},{character.RightLeg},{character.Torso}";
		return $"http://www.roblox.com/asset/characterfetchlist.ashx?items={arg}&colors={arg2}";
	}

	private static string GetLaunchArgumentsFormat(ClientConfig config, LaunchType launchType)
	{
		return launchType switch
		{
			LaunchType.Play => config.Player.LaunchArguments, 
			LaunchType.Studio => "", 
			LaunchType.Host => config.Server.LaunchArguments, 
			LaunchType.Render => "-rm " + $"-script \"loadfile('http://www.roblox.com/Game/RenderCharacter.ashx?charapp={HttpUtility.UrlEncode(GetCharacterAppearanceFetchUrl())}&outputDir={HttpUtility.UrlEncode(Path.Combine(PathHelper.UserAppData, "render.png"))}')()\"", 
			_ => throw new NotImplementedException(launchType.ToString()), 
		};
	}

	private static string GetLaunchArgumentsUrl(LaunchType launchType, bool authServer)
	{
		return launchType switch
		{
			LaunchType.Play => UrlConstructor.CreateJoinUrl(SharedViewModels.Play.SelectedIp, SharedViewModels.Play.SelectedPort, SharedViewModels.Play.SelectedChatStyle), 
			LaunchType.Studio => "", 
			LaunchType.Host => UrlConstructor.CreateHostUrl((ushort)(SharedViewModels.Host.SelectedPort + (authServer ? 1 : 0))), 
			LaunchType.Render => "", 
			_ => throw new NotImplementedException(launchType.ToString()), 
		};
	}

	private static string GetLaunchDirectoryName(ClientConfig config, LaunchType launchType)
	{
		return launchType switch
		{
			LaunchType.Play => LaunchDirectory.Player.ToString(), 
			LaunchType.Studio => GetStudioDirectory(config), 
			LaunchType.Host => config.Server.Directory.ToString(), 
			LaunchType.Render => GetStudioDirectory(config), 
			_ => throw new NotImplementedException(launchType.ToString()), 
		};
	}

	private static string GetExtraArgument(LaunchType launchType)
	{
		return launchType switch
		{
			LaunchType.Play => "-ip", 
			LaunchType.Studio => "-is", 
			LaunchType.Host => "-ih", 
			LaunchType.Render => "", 
			_ => throw new NotImplementedException(launchType.ToString()), 
		};
	}

	private static bool SafeCopyAddonFiles(string executableDir, string addonDir, string addonFileName)
	{
		string text = Path.Combine(addonDir, addonFileName);
		if (!File.Exists(text))
		{
			Utils.ShowMessageBox("Could not find addon file: " + text, MessageBoxButton.OK, MessageBoxImage.Hand);
			return false;
		}
		string destFileName = Path.Combine(executableDir, addonFileName);
		try
		{
			File.Copy(text, destFileName);
		}
		catch (Exception ex)
		{
			Logger.Instance.Error($"Failed to copy addon file: {ex}");
			Utils.ShowMessageBox("Failed to copy addon file: " + ex.Message, MessageBoxButton.OK, MessageBoxImage.Hand);
			return false;
		}
		return true;
	}

	private static void AddClientAddons(string executableDir)
	{
		if (Settings.Default.Launch.CustomGraphicsApi == CustomGraphicsApi.DXVK)
		{
			SafeCopyAddonFiles(executableDir, PathHelper.ClientAddonsDXVK, "d3d9.dll");
		}
		else if (Settings.Default.Launch.CustomGraphicsApi == CustomGraphicsApi.DgVoodoo && SafeCopyAddonFiles(executableDir, PathHelper.ClientAddonsDgVoodoo, "d3d9.dll"))
		{
			SafeCopyAddonFiles(executableDir, PathHelper.ClientAddonsDgVoodoo, "dgVoodoo.conf");
		}
	}

	private static bool DeleteOldClientAddons(string executableDir)
	{
		try
		{
			string path = Path.Combine(executableDir, "d3d9.dll");
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			string path2 = Path.Combine(executableDir, "dgVoodoo.conf");
			if (File.Exists(path2))
			{
				File.Delete(path2);
			}
			return true;
		}
		catch
		{
			return false;
		}
	}

	private static void SetupClientAddons(string executableDir)
	{
		Directory.CreateDirectory(PathHelper.ClientAddons);
		Directory.CreateDirectory(PathHelper.ClientAddonsCache);
		Utils.ClearClientAddonsCache();
		if (DeleteOldClientAddons(executableDir))
		{
			AddClientAddons(executableDir);
		}
	}

	private static Process LaunchProcessInstance(string executablePath, string executableDir, string launchArguments, bool hide)
	{
		Process process = new Process();
		process.StartInfo.UseShellExecute = true;
		process.StartInfo.FileName = executablePath;
		process.StartInfo.WorkingDirectory = executableDir;
		process.StartInfo.Arguments = launchArguments;
		if (hide)
		{
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
		}
		Logger.Instance.Info("Launching Roblox: " + executablePath + " " + launchArguments);
		process.Start();
		return process;
	}

	public static Process? LaunchProcess(LaunchType launchType, bool authLaunch = false, bool hide = false)
	{
		if (launchType == LaunchType.Host)
		{
			if (!authLaunch)
			{
				if (!Utils.IsUdpPortAvailable(SharedViewModels.Host.SelectedPort))
				{
					Utils.ShowMessageBox("Selected UDP port is already being used. Please select another port.", MessageBoxButton.OK, MessageBoxImage.Hand);
					return null;
				}
			}
			else if (!Utils.IsPortSafeForAuthServers(SharedViewModels.Host.SelectedPort))
			{
				Utils.ShowMessageBox("Selected port can not be used for authorisation servers. Check if any other applications are using the required ports or select another port.", MessageBoxButton.OK, MessageBoxImage.Hand);
				return null;
			}
			if (!File.Exists(Path.Combine(Utils.GetMapsDirectory(), SharedViewModels.Host.SelectedMap?.GetPath() ?? "")))
			{
				Utils.ShowMessageBox("Select a map before hosting.", MessageBoxButton.OK, MessageBoxImage.Hand);
				return null;
			}
		}
		ClientConfig clientConfig = ((launchType == LaunchType.Render) ? ClientConfigManager.Get("2012M") : GetClientConfig());
		if (clientConfig == null)
		{
			Logger.Instance.Warn("Client config is missing!");
			return null;
		}
		if (launchType == LaunchType.Studio && clientConfig.Studio.Type == StudioType.None)
		{
			Utils.ShowMessageBox("Selected client has no studio.", MessageBoxButton.OK, MessageBoxImage.Hand);
			return null;
		}
		string executableName = GetExecutableName(clientConfig, launchType);
		string launchArgumentsFormat = GetLaunchArgumentsFormat(clientConfig, launchType);
		string launchDirectoryName = GetLaunchDirectoryName(clientConfig, launchType);
		if (executableName.Contains('\\') || executableName.Contains('/'))
		{
			Utils.ShowMessageBox("Security check failed!", MessageBoxButton.OK, MessageBoxImage.Hand);
			return null;
		}
		string text = Path.Combine(PathHelper.Clients, (launchType != LaunchType.Render) ? ClientConfigManager.SelectedClientName : "2012M", launchDirectoryName);
		string text2 = Path.Combine(text, executableName);
		if (!File.Exists(text2))
		{
			Utils.ShowMessageBox("Could not find executable path: " + text2, MessageBoxButton.OK, MessageBoxImage.Hand);
			return null;
		}
		string launchArgumentsUrl = GetLaunchArgumentsUrl(launchType, authLaunch);
		string text3 = string.Format(launchArgumentsFormat, launchArgumentsUrl);
		if (launchType != LaunchType.Play || (launchType == LaunchType.Play && !clientConfig.Player.Sensitive))
		{
			string extraArgument = GetExtraArgument(launchType);
			text3 = text3 + " " + extraArgument;
			if (launchType == LaunchType.Host && authLaunch)
			{
				text3 += $" -ssp:{SharedViewModels.Host.SelectedPort}";
			}
		}
		Settings.Default.Serialize();
		SecureSettings.Save();
		SetupClientAddons(text);
		return LaunchProcessInstance(text2, text, text3, hide);
	}

	public static void Launch(LaunchType launchType, bool authLaunch = false)
	{
		Process process = LaunchProcess(launchType, authLaunch);
		if (process != null)
		{
			BootstrapperWindowShared bootstrapperWindow = Settings.Default.Launch.BootstrapperLaunchType.GetBootstrapperWindow(process);
			if (bootstrapperWindow != null)
			{
				bootstrapperWindow.Show();
			}
			else
			{
				process.Dispose();
			}
		}
	}
}

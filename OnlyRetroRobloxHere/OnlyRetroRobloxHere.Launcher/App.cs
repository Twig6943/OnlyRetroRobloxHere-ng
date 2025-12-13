using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using OnlyRetroRobloxHere.Common;
using OnlyRetroRobloxHere.Launcher.Enums;
using OnlyRetroRobloxHere.Launcher.Extensions;

namespace OnlyRetroRobloxHere.Launcher;

public partial class App : Application
{
	private InterProcessLock? _lock;

	protected override void OnStartup(StartupEventArgs e)
	{
		AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler.Handle;
		Logger.Instance.Info("Only Retro Roblox Here Version: " + Utils.Version);
		_lock = new InterProcessLock("Launcher", TimeSpan.FromSeconds(1.0));
		Utils.ThemeManager.ApplyTheme();
        if (!_lock.IsAcquired)
		{
			Logger.Instance.Error("ORRH is already running. Quitting.");
			Utils.ShowMessageBox("Another instance of Only Retro Roblox Here is already running.", MessageBoxButton.OK, MessageBoxImage.Hand);
			Shutdown();
			return;
		}
		if (!PathHelper.Base.All(char.IsAscii))
		{
			Logger.Instance.Warn("ORRH launch path has non-ASCII characters.");
			Utils.ShowMessageBox("Your path has non-ASCII characters. This WILL cause problems with Roblox. Please move your ORRH location.", MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}
		List<VCPPRedist> uninstalledRedists = VCPPRedistInstallationDetector.GetUninstalledRedists();
		if (uninstalledRedists.Any())
		{
			string text = string.Join(" and ", uninstalledRedists.Select((VCPPRedist x) => x.GetDescription()));
			Logger.Instance.Warn(text + " redists are missing!");
			Utils.ShowMessageBox("ORRH detected that " + text + " redist are missing. Redistributables are required to play the clients.\nRedist installers can be found inside the redist/MSVC folder.", MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}
		List<DotNetRuntime> uninstalledRuntimes = DotNetRuntimeInstallationDetector.GetUninstalledRuntimes();
		if (uninstalledRuntimes.Any())
		{
			string text2 = string.Join(" and ", uninstalledRuntimes.Select((DotNetRuntime x) => x.GetDescription()));
			Logger.Instance.Warn(text2 + " are missing!");
			if (Utils.ShowMessageBox("ORRH detected that " + text2 + " are missing. .NET runtimes are required for ORRH to function properly.\nThe runtimes can be found at https://dotnet.microsoft.com/en-us/download/dotnet/6.0. Would you like to open this URL in your browser?", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
			{
				Utils.OpenUrl("https://dotnet.microsoft.com/en-us/download/dotnet/6.0");
			}
		}
		Directory.CreateDirectory(PathHelper.UserAppData);
		Utils.ClearClientAddonsCache();
		base.OnStartup(e);
	}

	protected override void OnExit(ExitEventArgs e)
	{
		_lock?.Dispose();
		base.OnExit(e);
	}
}

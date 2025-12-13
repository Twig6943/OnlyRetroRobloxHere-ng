using System;
using System.Collections.Generic;
using System.Windows;
using OnlyRetroRobloxHere.Common.Enums;

namespace OnlyRetroRobloxHere.Launcher.UI.ViewModels.Pages.SettingsPages;

internal class GraphicsSettingsViewModel : ViewModelBase
{
	public static IReadOnlyCollection<CustomGraphicsApi> CustomGraphicsApis => (IReadOnlyCollection<CustomGraphicsApi>)(object)Enum.GetValues<CustomGraphicsApi>();

	public bool ExperimentalPlayerlistEnabled
	{
		get
		{
			return Settings.Default.Launch.ExperimentalPlayerlistEnabled;
		}
		set
		{
			if (Settings.Default.Launch.ExperimentalPlayerlistEnabled != value)
			{
				Settings.Default.Launch.ExperimentalPlayerlistEnabled = value;
				OnPropertyChanged("ExperimentalPlayerlistEnabled");
			}
		}
	}

	public bool ClientLargerPlayerListNameLength
	{
		get
		{
			return Settings.Default.Launch.ClientLargerPlayerListNameLength;
		}
		set
		{
			if (Settings.Default.Launch.ClientLargerPlayerListNameLength != value)
			{
				Settings.Default.Launch.ClientLargerPlayerListNameLength = value;
				OnPropertyChanged("ClientLargerPlayerListNameLength");
			}
		}
	}

	public bool ClientLegacyUIDarkMode
	{
		get
		{
			return Settings.Default.Launch.ClientLegacyUIDarkMode;
		}
		set
		{
			if (Settings.Default.Launch.ClientLegacyUIDarkMode != value)
			{
				Settings.Default.Launch.ClientLegacyUIDarkMode = value;
				OnPropertyChanged("ClientLegacyUIDarkMode");
			}
		}
	}

	public bool ClientHigherQualityScreenshots
	{
		get
		{
			return Settings.Default.Launch.ClientHigherQualityScreenshots;
		}
		set
		{
			if (Settings.Default.Launch.ClientHigherQualityScreenshots != value)
			{
				Settings.Default.Launch.ClientHigherQualityScreenshots = value;
				OnPropertyChanged("ClientHigherQualityScreenshots");
			}
		}
	}

	public CustomGraphicsApi CustomGraphicsApi
	{
		get
		{
			return Settings.Default.Launch.CustomGraphicsApi;
		}
		set
		{
			if (Settings.Default.Launch.CustomGraphicsApi != value)
			{
				Settings.Default.Launch.CustomGraphicsApi = value;
				OnPropertyChanged("CustomGraphicsApi");
			}
		}
	}

	public bool DebugShowWebServerWindow
	{
		get
		{
			return Settings.Default.Launch.DebugShowWebServerWindow;
		}
		set
		{
			if (Settings.Default.Launch.DebugShowWebServerWindow != value)
			{
				Settings.Default.Launch.DebugShowWebServerWindow = value;
				OnPropertyChanged("DebugShowWebServerWindow");
			}
		}
	}

	public uint FPSCap
	{
		get
		{
			return Settings.Default.Launch.FPSCap;
		}
		set
		{
			if (Settings.Default.Launch.FPSCap != value)
			{
				Settings.Default.Launch.FPSCap = value;
				OnPropertyChanged("FPSCap");
			}
		}
	}

	public uint FPSCapOlder
	{
		get
		{
			return Settings.Default.Launch.FPSCapOlder;
		}
		set
		{
			if (Settings.Default.Launch.FPSCapOlder != value)
			{
				Settings.Default.Launch.FPSCapOlder = value;
				OnPropertyChanged("FPSCapOlder");
			}
		}
	}

	public bool OptimisedClientStartup
	{
		get
		{
			return Settings.Default.Launch.OptimisedClientStartup;
		}
		set
		{
			if (Settings.Default.Launch.OptimisedClientStartup != value)
			{
				Settings.Default.Launch.OptimisedClientStartup = value;
				OnPropertyChanged("OptimisedClientStartup");
			}
		}
	}

    public static Visibility DebugShowWebServerWindowVisibility =>
    Utils.IsDebug ? Visibility.Visible : Visibility.Collapsed;

    public static Visibility FPSVisibility =>
	Utils.IsDebug ? Visibility.Visible : Visibility.Collapsed;
}

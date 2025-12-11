using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using OnlyRetroRobloxHere.Common;
using OnlyRetroRobloxHere.Launcher.Enums;

namespace OnlyRetroRobloxHere.Launcher.UI.ViewModels.Pages.SettingsPages;

internal class LauncherSettingsViewModel : ViewModelBase
{
	public static IReadOnlyCollection<BootstrapperLaunchType> BootstrapperLaunchTypes => (IReadOnlyCollection<BootstrapperLaunchType>)(object)Enum.GetValues<BootstrapperLaunchType>();

	public string CustomMapsDirectory
	{
		get
		{
			return Settings.Default.Launch.CustomMapsDirectory;
		}
		set
		{
			if (Settings.Default.Launch.CustomMapsDirectory != value && !string.IsNullOrWhiteSpace(value))
			{
				if (!string.IsNullOrWhiteSpace(value) && !Directory.Exists(value))
				{
					Utils.ShowMessageBox("Custom maps directory does not exist.", MessageBoxButton.OK, MessageBoxImage.Hand);
					return;
				}
				Settings.Default.Launch.CustomMapsDirectory = value;
				OnPropertyChanged("CustomMapsDirectory");
				SharedViewModels.Host.OnPropertyChanged("MapsPath");
			}
		}
	}

	public BootstrapperLaunchType BootstrapperLaunchType
	{
		get
		{
			return Settings.Default.Launch.BootstrapperLaunchType;
		}
		set
		{
			if (Settings.Default.Launch.BootstrapperLaunchType != value)
			{
				Settings.Default.Launch.BootstrapperLaunchType = value;
				OnPropertyChanged("BootstrapperLaunchType");
			}
		}
	}

	public string RobloxCookie
	{
		get
		{
			return SecureSettings.Default.RobloxCookie;
		}
		set
		{
			if (SecureSettings.Default.RobloxCookie != value)
			{
				SecureSettings.Default.RobloxCookie = value;
				OnPropertyChanged("RobloxCookie");
			}
		}
	}
    public bool HackCustomHats
    {
        get
        {
            return Settings.Default.Launch.HackCustomHats;
        }
        set
        {
            if (Settings.Default.Launch.HackCustomHats != value)
            {
                Settings.Default.Launch.HackCustomHats = value;
                OnPropertyChanged("HackCustomHats");
            }
        }
    }
}

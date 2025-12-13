using System.Windows;

namespace OnlyRetroRobloxHere.Launcher.UI.ViewModels.Pages.SettingsPages;

internal class ServerSettingsViewModel : ViewModelBase
{
	public bool EggHuntMode
	{
		get
		{
			return Settings.Default.Launch.EggHuntMode;
		}
		set
		{
			if (Settings.Default.Launch.EggHuntMode != value)
			{
				Settings.Default.Launch.EggHuntMode = value;
				OnPropertyChanged("EggHuntMode");
			}
		}
	}

	public bool CustomInGameApisEnabled
	{
		get
		{
			return Settings.Default.Launch.CustomInGameApisEnabled;
		}
		set
		{
			if (Settings.Default.Launch.CustomInGameApisEnabled != value)
			{
				Settings.Default.Launch.CustomInGameApisEnabled = value;
				OnPropertyChanged("CustomInGameApisEnabled");
			}
		}
	}

	public bool AutoSaveDebug
	{
		get
		{
			return Settings.Default.Launch.AutoSaveDebug;
		}
		set
		{
			if (Settings.Default.Launch.AutoSaveDebug != value)
			{
				Settings.Default.Launch.AutoSaveDebug = value;
				OnPropertyChanged("AutoSaveDebug");
			}
		}
	}

    public static Visibility AutoSaveDebugVisibility =>
    Utils.IsDebug ? Visibility.Visible : Visibility.Collapsed;

    public uint AutoSaveChangesPerPlayer
	{
		get
		{
			return Settings.Default.Launch.AutoSaveChangesPerPlayer;
		}
		set
		{
			if (Settings.Default.Launch.AutoSaveChangesPerPlayer != value)
			{
				Settings.Default.Launch.AutoSaveChangesPerPlayer = value;
				OnPropertyChanged("AutoSaveChangesPerPlayer");
			}
		}
	}

	public uint AutoSaveCheckInterval
	{
		get
		{
			return Settings.Default.Launch.AutoSaveCheckInterval;
		}
		set
		{
			if (Settings.Default.Launch.AutoSaveCheckInterval != value)
			{
				Settings.Default.Launch.AutoSaveCheckInterval = value;
				OnPropertyChanged("AutoSaveCheckInterval");
			}
		}
	}

	public uint AutoSaveCooldown
	{
		get
		{
			return Settings.Default.Launch.AutoSaveCooldown;
		}
		set
		{
			if (Settings.Default.Launch.AutoSaveCooldown != value)
			{
				Settings.Default.Launch.AutoSaveCooldown = value;
				OnPropertyChanged("AutoSaveCooldown");
			}
		}
	}
}

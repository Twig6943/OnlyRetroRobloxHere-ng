using System.Collections.ObjectModel;
using System.Windows;
using OnlyRetroRobloxHere.Launcher.Models;

namespace OnlyRetroRobloxHere.Launcher.UI.ViewModels.Pages;

internal class HostViewModel : ViewModelBase
{
	private TreePathItem? _selectedMap;

	private ushort _selectedPort = 53640;

	private bool _authEnabled;

	private string _serverDetailsText = "";

	private string _copyServerDetailsButtonText = "Copy to Clipboard";

	public ObservableCollection<TreePathItem> Maps { get; } = new ObservableCollection<TreePathItem>();

	public TreePathItem? SelectedMap
	{
		get
		{
			return _selectedMap;
		}
		set
		{
			SetProperty(ref _selectedMap, value, "SelectedMap");
		}
	}

	public ushort SelectedPort
	{
		get
		{
			return _selectedPort;
		}
		set
		{
			SetProperty(ref _selectedPort, value, "SelectedPort");
		}
	}

	public string MapsPath
	{
		get
		{
			return Settings.Default.Launch.CustomMapsDirectory;
		}
		set
		{
			if (Settings.Default.Launch.CustomMapsDirectory != value)
			{
				Settings.Default.Launch.CustomMapsDirectory = value;
				OnPropertyChanged("MapsPath");
			}
		}
	}

	public bool AutoSaveEnabled
	{
		get
		{
			return Settings.Default.Launch.AutoSaveEnabled;
		}
		set
		{
			if (Settings.Default.Launch.AutoSaveEnabled != value)
			{
				Settings.Default.Launch.AutoSaveEnabled = value;
				OnPropertyChanged("AutoSaveEnabled");
			}
		}
	}

	public bool AuthEnabled
	{
		get
		{
			return _authEnabled;
		}
		set
		{
			SetProperty(ref _authEnabled, value, "AuthEnabled");
		}
	}

	public string ServerDetailsText
	{
		get
		{
			return _serverDetailsText;
		}
		set
		{
			SetProperty(ref _serverDetailsText, value, "ServerDetailsText");
		}
	}

	public string DisplayIP
	{
		get
		{
			return Settings.Default.Launch.DisplayIP;
		}
		set
		{
			if (Settings.Default.Launch.DisplayIP != value)
			{
				Settings.Default.Launch.DisplayIP = value;
				OnPropertyChanged("DisplayIP");
			}
		}
	}

	public int DisplayPort
	{
		get
		{
			return Settings.Default.Launch.DisplayPort;
		}
		set
		{
			if (Settings.Default.Launch.DisplayPort != value)
			{
				Settings.Default.Launch.DisplayPort = value;
				OnPropertyChanged("DisplayPort");
			}
		}
	}

	public string DisplayPortString
	{
		get
		{
			if (DisplayPort != -1)
			{
				return DisplayPort.ToString();
			}
			return "";
		}
		set
		{
			ushort result;
			if (string.IsNullOrEmpty(value))
			{
				DisplayPort = -1;
				OnPropertyChanged("DisplayPortString");
			}
			else if (!ushort.TryParse(value, out result))
			{
				Utils.ShowMessageBox("Display port must be a value from 0-65535 or empty.", MessageBoxButton.OK, MessageBoxImage.Hand);
			}
			else
			{
				DisplayPort = result;
				OnPropertyChanged("DisplayPortString");
			}
		}
	}

	public string CopyServerDetailsButtonText
	{
		get
		{
			return _copyServerDetailsButtonText;
		}
		set
		{
			SetProperty(ref _copyServerDetailsButtonText, value, "CopyServerDetailsButtonText");
		}
	}

	public bool ServerNo3D
	{
		get
		{
			return Settings.Default.Launch.ServerNo3D;
		}
		set
		{
			if (Settings.Default.Launch.ServerNo3D != value)
			{
				Settings.Default.Launch.ServerNo3D = value;
				OnPropertyChanged("ServerNo3D");
			}
		}
	}
}

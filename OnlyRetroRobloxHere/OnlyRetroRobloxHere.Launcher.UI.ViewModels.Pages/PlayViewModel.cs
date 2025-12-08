using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using OnlyRetroRobloxHere.Common.Enums;

namespace OnlyRetroRobloxHere.Launcher.UI.ViewModels.Pages;

internal class PlayViewModel : ViewModelBase
{
	private string _selectedAddress = "localhost:53640";

	private string? _selectedClient;

	private ChatStyle _selectedChatStyle = ChatStyle.ClassicAndBubble;

	private Uri _showcaseImageTopSource = new Uri("pack://application:,,,/OnlyRetroRobloxHere;component/Resources/Blank.png");

	private double _showcaseImageTopOpacity;

	private Uri _showcaseImageBottomSource = new Uri("pack://application:,,,/OnlyRetroRobloxHere;component/Resources/Blank.png");

	private double _showcaseImageBottomOpacity;

	private string _clientName = "December 2010";

	private string _clientDescription = "Introduces the long awaited interface overhaul! It would not last long...";

	public static IReadOnlyCollection<ChatStyle> ChatStyles => (IReadOnlyCollection<ChatStyle>)(object)new ChatStyle[3]
	{
		ChatStyle.Classic,
		ChatStyle.Bubble,
		ChatStyle.ClassicAndBubble
	};

	public ObservableCollection<string> Clients => OnlyRetroRobloxHere.Launcher.Clients.List;

	public string SelectedAddress
	{
		get
		{
			return _selectedAddress;
		}
		set
		{
			if (!(_selectedAddress != value))
			{
				return;
			}
			if (!value.Contains(':'))
			{
				value += ":53640";
			}
			string[] array = value.Split(':');
			if (array.Length != 2)
			{
				Utils.ShowMessageBox("Address must be in the format of IP:Port", MessageBoxButton.OK, MessageBoxImage.Hand);
				return;
			}
			string selectedIp = array[0];
			if (!ushort.TryParse(array[1], out var result))
			{
				Utils.ShowMessageBox("Port must be a number between 0-65535", MessageBoxButton.OK, MessageBoxImage.Hand);
				return;
			}
			SelectedIp = selectedIp;
			SelectedPort = result;
			_selectedAddress = value;
			OnPropertyChanged("SelectedAddress");
		}
	}

	public string? SelectedClient
	{
		get
		{
			return _selectedClient;
		}
		set
		{
			SetProperty(ref _selectedClient, value, "SelectedClient");
		}
	}

	public ChatStyle SelectedChatStyle
	{
		get
		{
			return _selectedChatStyle;
		}
		set
		{
			SetProperty(ref _selectedChatStyle, value, "SelectedChatStyle");
		}
	}

	public string SelectedIp { get; set; } = "localhost";

	public ushort SelectedPort { get; set; } = 53640;

	public Uri ShowcaseImageTopSource
	{
		get
		{
			return _showcaseImageTopSource;
		}
		set
		{
			SetProperty(ref _showcaseImageTopSource, value, "ShowcaseImageTopSource");
		}
	}

	public double ShowcaseImageTopOpacity
	{
		get
		{
			return _showcaseImageTopOpacity;
		}
		set
		{
			SetProperty(ref _showcaseImageTopOpacity, value, "ShowcaseImageTopOpacity");
		}
	}

	public Uri ShowcaseImageBottomSource
	{
		get
		{
			return _showcaseImageBottomSource;
		}
		set
		{
			SetProperty(ref _showcaseImageBottomSource, value, "ShowcaseImageBottomSource");
		}
	}

	public double ShowcaseImageBottomOpacity
	{
		get
		{
			return _showcaseImageBottomOpacity;
		}
		set
		{
			SetProperty(ref _showcaseImageBottomOpacity, value, "ShowcaseImageBottomOpacity");
		}
	}

	public string ClientName
	{
		get
		{
			return _clientName;
		}
		set
		{
			SetProperty(ref _clientName, value, "ClientName");
		}
	}

	public string ClientDescription
	{
		get
		{
			return _clientDescription;
		}
		set
		{
			SetProperty(ref _clientDescription, value, "ClientDescription");
		}
	}

	public bool AllowClientLaunch()
	{
		if (SelectedClient != null && !Müllschutz.IsWhitelisted(SelectedClient))
		{
			Utils.OpenUrl("https://www.youtube.com/watch?v=OYVn34aN6F4");
			return false;
		}
		return true;
	}
}

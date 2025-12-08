using System;
using System.Windows;

namespace OnlyRetroRobloxHere.Launcher.UI.ViewModels.Pages;

internal class SettingsViewModel : ViewModelBase
{
	private string _headerText = "Launcher Settings";

	private Uri _adSource = new Uri("pack://application:,,,/OnlyRetroRobloxHere;component/Resources/Ads/Vertical/1.jpg");

	private Visibility _trollModalVisibility = Visibility.Collapsed;

	public string HeaderText
	{
		get
		{
			return _headerText;
		}
		set
		{
			SetProperty(ref _headerText, value, "HeaderText");
		}
	}

	public Uri AdSource
	{
		get
		{
			return _adSource;
		}
		set
		{
			SetProperty(ref _adSource, value, "AdSource");
		}
	}

	public Visibility TrollModalVisibility
	{
		get
		{
			return _trollModalVisibility;
		}
		set
		{
			SetProperty(ref _trollModalVisibility, value, "TrollModalVisibility");
		}
	}
}

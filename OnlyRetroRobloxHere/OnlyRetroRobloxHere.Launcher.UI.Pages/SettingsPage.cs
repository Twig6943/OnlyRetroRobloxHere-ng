using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;
using OnlyRetroRobloxHere.Launcher.UI.Controls;
using OnlyRetroRobloxHere.Launcher.UI.Dialogs;
using OnlyRetroRobloxHere.Launcher.UI.Pages.SettingsPages;
using OnlyRetroRobloxHere.Launcher.UI.ViewModels.Pages;

namespace OnlyRetroRobloxHere.Launcher.UI.Pages;

public partial class SettingsPage : BasePage, IComponentConnector
{
	private const int NumOfAds = 68;

	private SettingsViewModel _viewModel;

	private SelectableButton? _selectedButton;

	private Dictionary<string, BasePage> _pages = new Dictionary<string, BasePage>();

	private BasePage? _currentPage;

	private Random _random = new Random();

	public SettingsPage()
	{
		InitializeComponent();
		_viewModel = new SettingsViewModel();
		base.DataContext = _viewModel;
		OnNavigationButtonClick(LauncherNavivationButton, null);
	}

	public override void OnPageOpening()
	{
		SelectRandomAd();
		base.OnPageOpening();
	}

	private BasePage GetPageFromName(string name)
	{
		return name switch
		{
			"Launcher" => new LauncherSettingsPage(), 
			"Graphics" => new GraphicsSettingsPage(), 
			"Server" => new ServerSettingsPage(), 
			"TTS" => new TTSSettingsPage(), 
			_ => throw new Exception("Unrecognised settings page name"), 
		};
	}

	private BasePage GetPage(string name)
	{
		if (!_pages.ContainsKey(name))
		{
			BasePage pageFromName = GetPageFromName(name);
			_pages[name] = pageFromName;
			return pageFromName;
		}
		return _pages[name];
	}

	private void OnNavigationButtonClick(object sender, RoutedEventArgs e)
	{
		SelectableButton selectableButton = (SelectableButton)sender;
		if (!selectableButton.IsSelected)
		{
			if (!(selectableButton.Tag is string name))
			{
				throw new Exception("Page tag is not a string");
			}
			BasePage page = GetPage(name);
			_currentPage?.OnPageClosing();
			_currentPage = page;
			page.OnPageOpening();
			NavigationFrame.Navigate(page);
			NavigationFrame.NavigationService.RemoveBackEntry();
			selectableButton.IsSelected = true;
			if (_selectedButton != null)
			{
				_selectedButton.IsSelected = false;
			}
			_selectedButton = selectableButton;
			_viewModel.HeaderText = $"{selectableButton.Content} Settings";
		}
	}

	private void SelectRandomAd()
	{
		int value = _random.Next(1, 69);
		_viewModel.AdSource = new Uri($"pack://application:,,,/OnlyRetroRobloxHere;component/Resources/Ads/Vertical/{value}.jpg");
	}

	private void OnReportButtonClicked(object sender, RoutedEventArgs e)
	{
		_viewModel.TrollModalVisibility = Visibility.Visible;
	}

	private void OnTrollYesButtonClicked(object sender, RoutedEventArgs e)
	{
		_viewModel.TrollModalVisibility = Visibility.Collapsed;
		((MainWindow)Application.Current.MainWindow).Navigate(new CatalogItemPage(64446449uL));
		((MainWindow)Application.Current.MainWindow).SetActiveButton("Catalog");
	}

}

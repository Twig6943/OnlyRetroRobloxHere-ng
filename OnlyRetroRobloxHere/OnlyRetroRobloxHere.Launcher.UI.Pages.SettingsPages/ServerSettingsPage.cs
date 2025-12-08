using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;
using OnlyRetroRobloxHere.Launcher.UI.ViewModels.Pages.SettingsPages;

namespace OnlyRetroRobloxHere.Launcher.UI.Pages.SettingsPages;

public partial class ServerSettingsPage : BasePage, IComponentConnector
{
	private ServerSettingsViewModel _viewModel;

	public ServerSettingsPage()
	{
		InitializeComponent();
		_viewModel = new ServerSettingsViewModel();
		base.DataContext = _viewModel;
	}

	private void OnAutoSaveResetClicked(object sender, RoutedEventArgs e)
	{
		_viewModel.AutoSaveDebug = false;
		_viewModel.AutoSaveChangesPerPlayer = 100u;
		_viewModel.AutoSaveCheckInterval = 1800u;
		_viewModel.AutoSaveCooldown = 900u;
	}

}

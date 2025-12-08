using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows.Markup;
using OnlyRetroRobloxHere.Launcher.UI.ViewModels.Pages.SettingsPages;

namespace OnlyRetroRobloxHere.Launcher.UI.Pages.SettingsPages;

public partial class GraphicsSettingsPage : BasePage, IComponentConnector
{
	private GraphicsSettingsViewModel _viewModel;

	public GraphicsSettingsPage()
	{
		InitializeComponent();
		_viewModel = new GraphicsSettingsViewModel();
		base.DataContext = _viewModel;
	}

}

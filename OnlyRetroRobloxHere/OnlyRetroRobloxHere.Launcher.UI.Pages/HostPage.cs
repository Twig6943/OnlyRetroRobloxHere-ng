using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using OnlyRetroRobloxHere.Common;
using OnlyRetroRobloxHere.Launcher.Enums;
using OnlyRetroRobloxHere.Launcher.Models;
using OnlyRetroRobloxHere.Launcher.UI.Helpers;
using OnlyRetroRobloxHere.Launcher.UI.ViewModels.Pages;

namespace OnlyRetroRobloxHere.Launcher.UI.Pages;

public partial class HostPage : BasePage, IComponentConnector
{
	private HostViewModel _viewModel;

	private Random _random;

	private string? _cachedIp;

	private bool _initialised;

	private object _isFetchingServerDetailsLock = new object();

	private bool _isFetchingServerDetails;

	public HostPage()
	{
		InitializeComponent();
		_random = new Random();
		_viewModel = SharedViewModels.Host;
		base.DataContext = _viewModel;
		_viewModel.PropertyChanged += OnViewModelPropertyChanged;
		SharedViewModels.Play.PropertyChanged += OnPlayViewModelPropertyChanged;
		Settings.Default.PreSerialization += OnSettingsPreSerialization;
	}

	private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "MapsPath")
		{
			LoadMapList(init: false);
		}
		if (e.PropertyName == "SelectedMap" || e.PropertyName == "SelectedPort" || e.PropertyName == "DisplayIP" || e.PropertyName == "DisplayPort")
		{
			UpdateServerDetailsTextBox();
		}
	}

	private void OnPlayViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "SelectedClient")
		{
			UpdateServerDetailsTextBox();
		}
	}

	private void OnPageLoaded(object sender, RoutedEventArgs e)
	{
		if (!_initialised)
		{
			LoadMapList(init: true);
			UpdateServerDetailsTextBox();
			_initialised = true;
		}
	}

	private void FindAndSetSelectedTreePathItem(List<string> paths)
	{
		IEnumerable<TreePathItem> enumerable = _viewModel.Maps;
		int count = paths.Count;
		int num = count - 1;
		for (int i = 0; i < count; i++)
		{
			string text = paths[i];
			using IEnumerator<TreePathItem> enumerator = enumerable.GetEnumerator();
			TreePathDirectoryItem treePathDirectoryItem;
			while (true)
			{
				if (!enumerator.MoveNext())
				{
					return;
				}
				TreePathItem current = enumerator.Current;
				if (!(current.Name == text))
				{
					continue;
				}
				if (i != num)
				{
					treePathDirectoryItem = current as TreePathDirectoryItem;
					if (treePathDirectoryItem != null)
					{
						break;
					}
				}
				else if (current != null)
				{
					TreeViewItem treeViewItem = TreeViewHelper.GetTreeViewItem(MapTreeView, current);
					if (treeViewItem != null)
					{
						treeViewItem.IsSelected = true;
					}
					return;
				}
			}
			enumerable = treePathDirectoryItem.Items;
		}
	}

	private void LoadMapList(bool init)
	{
		List<string> list = (init ? Utils.SplitPath(Settings.Default.Launch.SelectedMap ?? "").ToList() : _viewModel.SelectedMap?.GetPaths());
		_viewModel.Maps.Clear();
		bool flag = !string.IsNullOrWhiteSpace(_viewModel.MapsPath);
		string path = (flag ? _viewModel.MapsPath : PathHelper.Maps);
		if (!Directory.Exists(path))
		{
			if (flag)
			{
				Utils.ShowMessageBox("Custom maps directory no longer exists. Defaulting to the default path.", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				_viewModel.MapsPath = "";
				SharedViewModels.LauncherSettings.OnPropertyChanged("CustomMapsDirectory");
				return;
			}
			Utils.ShowMessageBox("The maps directory is missing! Creating a new one.", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			Directory.CreateDirectory(path);
		}
		TreePathItemProvider.AddItems(_viewModel.Maps, path);
		if (list != null)
		{
			FindAndSetSelectedTreePathItem(list);
		}
	}

	private void SelectRandomMap()
	{
		if (_viewModel.Maps.Any())
		{
			TreePathItem treePathItem = _viewModel.Maps[_random.Next(_viewModel.Maps.Count)];
			while (treePathItem is TreePathDirectoryItem treePathDirectoryItem && treePathDirectoryItem.Items.Any())
			{
				treePathItem = treePathDirectoryItem.Items[_random.Next(treePathDirectoryItem.Items.Count)];
			}
			TreeViewItem treeViewItem = TreeViewHelper.GetTreeViewItem(MapTreeView, treePathItem);
			if (treeViewItem != null)
			{
				treeViewItem.IsSelected = true;
			}
		}
	}

	private void OnMapReloadClicked(object sender, RoutedEventArgs e)
	{
		LoadMapList(init: false);
	}

	private void OnMapRandomClicked(object sender, RoutedEventArgs e)
	{
		SelectRandomMap();
	}

	private void OnMapOpenFolderClicked(object sender, RoutedEventArgs e)
	{
		Process.Start("explorer.exe", (!string.IsNullOrWhiteSpace(_viewModel.MapsPath)) ? _viewModel.MapsPath : PathHelper.Maps);
	}

	private void OnHostButtonClicked(object sender, RoutedEventArgs e)
	{
		if (SharedViewModels.Play.AllowClientLaunch())
		{
			RobloxLauncher.Launch(LaunchType.Host, _viewModel.AuthEnabled);
		}
	}

	private void OnSettingsPreSerialization(Settings settings)
	{
		settings.Launch.SelectedMap = _viewModel.SelectedMap?.GetPath();
	}

	private void UpdateServerDetailsTextBox()
	{
		_viewModel.ServerDetailsText = CreateServerDetails("[copy to see ip]").Result;
	}

	private async void OnCopyServerDetailsButtonClicked(object sender, RoutedEventArgs e)
	{
		lock (_isFetchingServerDetailsLock)
		{
			if (_isFetchingServerDetails)
			{
				return;
			}
			_isFetchingServerDetails = true;
		}
		_viewModel.CopyServerDetailsButtonText = "Copying...";
		string text;
		try
		{
			text = await CreateServerDetails();
		}
		catch (HttpRequestException value)
		{
			Logger.Instance.Error($"Failed to copy server details: {value}");
			_viewModel.CopyServerDetailsButtonText = "Failed to get your public IP";
			_isFetchingServerDetails = false;
			return;
		}
		catch (Exception value2)
		{
			Logger.Instance.Error($"Failed to copy server details: {value2}");
			_viewModel.CopyServerDetailsButtonText = "Failed to copy server details";
			_isFetchingServerDetails = false;
			return;
		}
		try
		{
			Clipboard.SetText(text);
		}
		catch (COMException value3)
		{
			Logger.Instance.Error($"Failed to set clipboard: {value3}");
			_viewModel.CopyServerDetailsButtonText = "Failed to set clipboard";
			_isFetchingServerDetails = false;
			return;
		}
		_viewModel.CopyServerDetailsButtonText = "Copied!";
		_isFetchingServerDetails = false;
	}

	private async Task<string> CreateServerDetails(string? ipOverwrite = null)
	{
		StringBuilder builder = new StringBuilder();
		StringBuilder stringBuilder = builder;
		StringBuilder stringBuilder2 = stringBuilder;
		StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(8, 1, stringBuilder);
		handler.AppendLiteral("Client: ");
		handler.AppendFormatted(SharedViewModels.Play.SelectedClient);
		stringBuilder2.AppendLine(ref handler);
		string text = Path.Combine(Utils.GetMapsDirectory(), SharedViewModels.Host.SelectedMap?.GetPath() ?? "");
		string value = ((!File.Exists(text)) ? "None!" : ((!text.EndsWith(".gz")) ? Path.GetFileName(text) : Path.GetFileNameWithoutExtension(text)));
		stringBuilder = builder;
		StringBuilder stringBuilder3 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(5, 1, stringBuilder);
		handler.AppendLiteral("Map: ");
		handler.AppendFormatted(value);
		stringBuilder3.AppendLine(ref handler);
		string value2;
		if (!string.IsNullOrEmpty(_viewModel.DisplayIP))
		{
			value2 = _viewModel.DisplayIP;
		}
		else if (ipOverwrite != null)
		{
			value2 = ipOverwrite;
		}
		else
		{
			if (string.IsNullOrEmpty(_cachedIp))
			{
				_cachedIp = await Http.Client.GetStringAsync("https://ipv4.icanhazip.com/");
				_cachedIp = _cachedIp.TrimEnd();
			}
			value2 = _cachedIp;
		}
		int value3 = ((_viewModel.DisplayPort == -1) ? _viewModel.SelectedPort : _viewModel.DisplayPort);
		stringBuilder = builder;
		StringBuilder stringBuilder4 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(13, 2, stringBuilder);
		handler.AppendLiteral("IP Address: ");
		handler.AppendFormatted(value2);
		handler.AppendLiteral(":");
		handler.AppendFormatted(value3);
		stringBuilder4.AppendLine(ref handler);
		stringBuilder = builder;
		StringBuilder stringBuilder5 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(33, 1, stringBuilder);
		handler.AppendLiteral("Version: Only Retro Roblox Here v");
		handler.AppendFormatted(Utils.Version);
		stringBuilder5.AppendLine(ref handler);
		return builder.ToString();
	}

}

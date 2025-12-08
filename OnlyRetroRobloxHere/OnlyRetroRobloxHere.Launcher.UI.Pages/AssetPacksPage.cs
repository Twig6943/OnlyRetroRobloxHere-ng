using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using OnlyRetroRobloxHere.Common;
using OnlyRetroRobloxHere.Common.Enums;
using OnlyRetroRobloxHere.Common.Models;
using OnlyRetroRobloxHere.Launcher.UI.ViewModels.Pages;

namespace OnlyRetroRobloxHere.Launcher.UI.Pages;

public partial class AssetPacksPage : BasePage, IComponentConnector
{
	private AssetPacksViewModel _viewModel;

	private AssetPack? _selectedAssetPack;

	public AssetPacksPage()
	{
		InitializeComponent();
		AssetPackManager.Instance.SetDisabledList(Settings.Default.Launch.DisabledAssetPacks);
		_viewModel = new AssetPacksViewModel();
		_viewModel.InfoVisibility = Visibility.Hidden;
		base.DataContext = _viewModel;
		Settings.Default.PreSerialization += OnSettingsPreSerialization;
	}

	private string CreateClientsString(List<string> clients)
	{
		List<string> list = new List<string>();
		foreach (string client in clients)
		{
			if (client == "*")
			{
				list.Add("All");
			}
			else if (client.StartsWith('!'))
			{
				string text = client;
				string text2 = text.Substring(1, text.Length - 1);
				list.Add("Not " + text2);
			}
			else
			{
				list.Add(client);
			}
		}
		if (!list.Any())
		{
			list.Add("All");
		}
		return string.Join(", ", list);
	}

	private string CreateApiText(AssetPackApi api)
	{
		return api switch
		{
			AssetPackApi.None => "Using no metadata", 
			AssetPackApi.V1 => "Using V1 metadata", 
			AssetPackApi.SodikmV1 => "Using Sodikm V1 metadata", 
			_ => throw new Exception($"Unknown asset pack metadata version: {api}"), 
		};
	}

	private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (AssetPackListBox.SelectedItem == null)
		{
			_viewModel.InfoVisibility = Visibility.Hidden;
			_viewModel.NothingSelectedVisibility = Visibility.Visible;
			return;
		}
		_selectedAssetPack = (AssetPack)AssetPackListBox.SelectedItem;
		_viewModel.Name = _selectedAssetPack.DisplayName;
		_viewModel.Version = _selectedAssetPack.Version;
		_viewModel.VersionVisibility = (string.IsNullOrEmpty(_selectedAssetPack.Version) ? Visibility.Collapsed : Visibility.Visible);
		_viewModel.Authors = _selectedAssetPack.Author;
		_viewModel.AuthorsVisibility = (string.IsNullOrEmpty(_selectedAssetPack.Author) ? Visibility.Collapsed : Visibility.Visible);
		_viewModel.Description = _selectedAssetPack.Description;
		_viewModel.DescriptionVisibility = (string.IsNullOrEmpty(_selectedAssetPack.Description) ? Visibility.Collapsed : Visibility.Visible);
		_viewModel.Clients = CreateClientsString(_selectedAssetPack.Clients);
		_viewModel.DisabledVisibility = ((!_selectedAssetPack.Disabled) ? Visibility.Collapsed : Visibility.Visible);
		_viewModel.ApiText = CreateApiText(_selectedAssetPack.Api);
		_viewModel.FolderText = "Directory name: " + _selectedAssetPack.FolderName;
		_viewModel.InfoVisibility = Visibility.Visible;
		_viewModel.NothingSelectedVisibility = Visibility.Collapsed;
	}

	private void OnRefresh(object sender, RoutedEventArgs e)
	{
		string text = _selectedAssetPack?.DisplayName;
		AssetPackManager.Instance.Reparse();
		AssetPackListBox.Items.Refresh();
		if (text == null)
		{
			return;
		}
		foreach (AssetPack assetPack in AssetPackManager.Instance.AssetPacks)
		{
			if (assetPack.DisplayName == text)
			{
				AssetPackListBox.SelectedItem = assetPack;
				break;
			}
		}
	}

	private void OnSettingsPreSerialization(Settings settings)
	{
		settings.Launch.DisabledAssetPacks = AssetPackManager.Instance.DisabledAssetPacks;
	}

	private void OnOpenFolder(object sender, RoutedEventArgs e)
	{
		if (_selectedAssetPack != null)
		{
			Process.Start("explorer.exe", _selectedAssetPack.Folder);
		}
	}

	private void OnToggle(object sender, RoutedEventArgs e)
	{
		if (_selectedAssetPack != null)
		{
			AssetPackManager.Instance.ToggleAssetPack(_selectedAssetPack);
			AssetPackListBox.Items.Refresh();
			_viewModel.DisabledVisibility = ((!_selectedAssetPack.Disabled) ? Visibility.Collapsed : Visibility.Visible);
		}
	}

}

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using OnlyRetroRobloxHere.Common.Enums;
using OnlyRetroRobloxHere.Launcher.Models;
using OnlyRetroRobloxHere.Launcher.UI.Controls;
using OnlyRetroRobloxHere.Launcher.UI.Dialogs;
using OnlyRetroRobloxHere.Launcher.UI.ViewModels.Pages;

namespace OnlyRetroRobloxHere.Launcher.UI.Pages;

public partial class CatalogPage : BasePage, IComponentConnector, IStyleConnector
{
	private const int ItemsPerPage = 18;

	private const AvatarAssetType AllType = (AvatarAssetType)200;

	private const AvatarAssetType FeaturedType = (AvatarAssetType)201;

	private CatalogViewModel _viewModel;

	private SelectableButton? _selectedButton;

	private AvatarAssetType _selectedCategory;

	private Dictionary<AvatarAssetType, IEnumerable<AvatarItem>> _cache = new Dictionary<AvatarAssetType, IEnumerable<AvatarItem>>();

	public CatalogPage()
	{
		InitializeComponent();
		_viewModel = new CatalogViewModel();
		_cache[(AvatarAssetType)201] = Catalog.GetFeatured();
		OnCategoryClicked(FeaturedCategoryButton, null);
		base.DataContext = _viewModel;
		_viewModel.PropertyChanged += OnViewModelPropertyChanged;
	}

	private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "SearchQuery")
		{
			OnSearchQueryUpdated();
		}
	}

	private void OnItemClicked(object sender, RoutedEventArgs e)
	{
		if (!(((Button)sender).Tag is ulong id))
		{
			throw new Exception("Catalog item tag is not a ulong");
		}
		CatalogItemPage page = new CatalogItemPage(id);
		((MainWindow)Application.Current.MainWindow).Navigate(page);
	}

	private IEnumerable<AvatarItem> GetCategory(AvatarAssetType type)
	{
		if (_cache.ContainsKey(type))
		{
			return _cache[type];
		}
		List<AvatarItem> list = ((type != (AvatarAssetType)200) ? Catalog.ItemsCategorised[type].ToList() : AvatarItems.Database.Values.Where((AvatarItem x) => x.Type.IsShownOnCatalog()).ToList());
		list.Sort();
		_cache[type] = list;
		return list;
	}

	private AvatarAssetType GetCategoryFromName(string category)
	{
		if (!(category == "All"))
		{
			if (category == "Featured")
			{
				return (AvatarAssetType)201;
			}
			return Enum.Parse<AvatarAssetType>(category);
		}
		return (AvatarAssetType)200;
	}

	private void AwardAllAccessories()
	{
		foreach (AvatarItem item in GetCategory((AvatarAssetType)200))
		{
			Appearance.AwardItem(item);
		}
	}

	private void OnSearchQueryUpdated()
	{
		if (_viewModel.SearchQuery == "givmeallplz")
		{
			AwardAllAccessories();
			Sounds.SecretActivated.Play();
			_viewModel.SearchQuery = "";
		}
        if (_viewModel.SearchQuery == "backtothesummerof1999")
        {
			Settings.Default.Launch.SecretEventOverride = true;
			Settings.Default.Launch.SecretEvent = "summer";
            Sounds.SecretActivated.Play();
            _viewModel.SearchQuery = "";
        }
        if (_viewModel.SearchQuery == "inasnowboundland")
        {
			Settings.Default.Launch.SecretEventOverride = true;
			Settings.Default.Launch.SecretEvent = "winter";
            Sounds.SecretActivated.Play();
            _viewModel.SearchQuery = "";
        }
        if (_viewModel.SearchQuery == "millionsofgays.killthem")
        {
			Settings.Default.Launch.SecretEventOverride = true;
			Settings.Default.Launch.SecretEvent = "pride";
            Sounds.SecretActivated.Play();
            _viewModel.SearchQuery = "";
        }
        if (_viewModel.SearchQuery == "flowersandgrass")
        {
			Settings.Default.Launch.SecretEventOverride = true;
			Settings.Default.Launch.SecretEvent = "spring";
            Sounds.SecretActivated.Play();
            _viewModel.SearchQuery = "";
        }
        if (_viewModel.SearchQuery == "inthedeadofautumn")
        {
			Settings.Default.Launch.SecretEventOverride = true;
			Settings.Default.Launch.SecretEvent = "fall";
            Sounds.SecretActivated.Play();
            _viewModel.SearchQuery = "";
        }
        if (_viewModel.SearchQuery == "onlyretrothemeshere")
        {
			Settings.Default.Launch.SecretEventOverride = true;
            Settings.Default.Launch.SecretEvent = "none";
            Sounds.SecretActivated.Play();
            _viewModel.SearchQuery = "";
        }
        if (_viewModel.SearchQuery == "hiddenstarinfourseasons")
        {
			Settings.Default.Launch.SecretEventOverride = false;
            Sounds.SecretActivated.Play();
            _viewModel.SearchQuery = "";
        }
        else if (!string.IsNullOrEmpty(_viewModel.SearchQuery) && _selectedCategory == (AvatarAssetType)201)
		{
			OnCategoryClicked(AllCategoryButton, null);
		}
		else
		{
			UpdateItems();
		}
	}

	private void UpdateItems()
	{
		IEnumerable<AvatarItem> enumerable = GetCategory(_selectedCategory);
		if (!string.IsNullOrWhiteSpace(_viewModel.SearchQuery) && _selectedCategory != (AvatarAssetType)201)
		{
			string searchQueryLower = _viewModel.SearchQuery.ToLowerInvariant();
			enumerable = enumerable.Where((AvatarItem x) => x.Name.ToLowerInvariant().Contains(searchQueryLower));
		}
		CatalogItemsControl.ResetPageIndex(updateItems: false);
		_viewModel.Items = enumerable;
		_viewModel.NavigationVisibility = ((enumerable.Count() <= 18) ? Visibility.Collapsed : Visibility.Visible);
	}

	private void OnCategoryClicked(object sender, RoutedEventArgs e)
	{
		SelectableButton selectableButton = (SelectableButton)sender;
		if (_selectedButton != selectableButton)
		{
			if (!(selectableButton.Tag is string category))
			{
				throw new Exception("Category button tag is not a string");
			}
			_selectedCategory = GetCategoryFromName(category);
			UpdateItems();
			if (_selectedButton != null)
			{
				_selectedButton.IsSelected = false;
			}
			selectableButton.IsSelected = true;
			_selectedButton = selectableButton;
		}
	}

}

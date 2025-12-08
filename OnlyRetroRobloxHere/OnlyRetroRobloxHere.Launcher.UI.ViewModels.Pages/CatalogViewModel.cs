using System.Collections.Generic;
using System.Linq;
using System.Windows;
using OnlyRetroRobloxHere.Launcher.Extensions;
using OnlyRetroRobloxHere.Launcher.Models;

namespace OnlyRetroRobloxHere.Launcher.UI.ViewModels.Pages;

internal class CatalogViewModel : ViewModelBase
{
	private IEnumerable<AvatarItem> _items = AvatarItems.Database.Values.ToList().SortLinq();

	private string _searchQuery = "";

	private Visibility _navigationVisibility;

	public IEnumerable<AvatarItem> Items
	{
		get
		{
			return _items;
		}
		set
		{
			SetProperty(ref _items, value, "Items");
		}
	}

	public string SearchQuery
	{
		get
		{
			return _searchQuery;
		}
		set
		{
			SetProperty(ref _searchQuery, value, "SearchQuery");
		}
	}

	public Visibility NavigationVisibility
	{
		get
		{
			return _navigationVisibility;
		}
		set
		{
			SetProperty(ref _navigationVisibility, value, "NavigationVisibility");
		}
	}
}

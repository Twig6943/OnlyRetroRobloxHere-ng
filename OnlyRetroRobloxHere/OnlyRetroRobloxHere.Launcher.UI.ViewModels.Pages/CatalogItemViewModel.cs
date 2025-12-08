using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using OnlyRetroRobloxHere.Launcher.Models;

namespace OnlyRetroRobloxHere.Launcher.UI.ViewModels.Pages;

internal class CatalogItemViewModel : ViewModelBase
{
	private const int RecommendationsCount = 16;

	private AvatarItem? _item;

	private bool _doesNotOwn = true;

	private bool _showModal;

	private bool _showSuccessInModal;

	private Visibility _modalVisibility = Visibility.Collapsed;

	private Visibility _modalPurchaseGridVisibility;

	private Visibility _modalSuccessGridVisibility = Visibility.Collapsed;

	private Visibility _modalProcessingGridVisibility = Visibility.Collapsed;

	private Visibility _boomBoxVisibility = Visibility.Collapsed;

	private string? _musicSelectedItem;

	private Visibility _musicConfigVisibility = Visibility.Collapsed;

	public AvatarItem Item
	{
		get
		{
			return _item ?? (_item = AvatarItems.Database.First().Value);
		}
		set
		{
			SetProperty(ref _item, value, "Item");
		}
	}

	public bool DoesNotOwn
	{
		get
		{
			return _doesNotOwn;
		}
		set
		{
			SetProperty(ref _doesNotOwn, value, "DoesNotOwn");
		}
	}

	public bool ShowModal
	{
		get
		{
			return _showModal;
		}
		set
		{
			SetProperty(ref _showModal, value, "ShowModal");
		}
	}

	public bool ShowSuccessInModal
	{
		get
		{
			return _showSuccessInModal;
		}
		set
		{
			SetProperty(ref _showSuccessInModal, value, "ShowSuccessInModal");
		}
	}

	public Visibility ModalVisibility
	{
		get
		{
			return _modalVisibility;
		}
		set
		{
			SetProperty(ref _modalVisibility, value, "ModalVisibility");
		}
	}

	public Visibility ModalPurchaseGridVisibility
	{
		get
		{
			return _modalPurchaseGridVisibility;
		}
		set
		{
			SetProperty(ref _modalPurchaseGridVisibility, value, "ModalPurchaseGridVisibility");
		}
	}

	public Visibility ModalSuccessGridVisibility
	{
		get
		{
			return _modalSuccessGridVisibility;
		}
		set
		{
			SetProperty(ref _modalSuccessGridVisibility, value, "ModalSuccessGridVisibility");
		}
	}

	public Visibility ModalProcessingGridVisibility
	{
		get
		{
			return _modalProcessingGridVisibility;
		}
		set
		{
			SetProperty(ref _modalProcessingGridVisibility, value, "ModalProcessingGridVisibility");
		}
	}

	public List<AvatarItem> Recommendations => Catalog.GetRecommendations(Item, 16);

	public bool ShowCreatorName => !string.IsNullOrEmpty(Item.Creator);

	public bool ShowCreationDate => Item.CreationDate != DateTime.MinValue;

	public bool ShowDescription => !string.IsNullOrEmpty(Item.Description);

	public bool ShowSeperator
	{
		get
		{
			if (!ShowCreatorName && !ShowCreationDate)
			{
				return ShowDescription;
			}
			return true;
		}
	}

	public string CreationDate => Item.CreationDate.ToString("MM/dd/yyyy");

	public static string[] MusicOptions { get; } = new string[4] { "Rock", "Funk", "Jazz", "Electronic" };

	public Visibility BoomBoxVisibility
	{
		get
		{
			return _boomBoxVisibility;
		}
		set
		{
			SetProperty(ref _boomBoxVisibility, value, "BoomBoxVisibility");
		}
	}

	public string? MusicSelectedItem
	{
		get
		{
			return _musicSelectedItem;
		}
		set
		{
			SetProperty(ref _musicSelectedItem, value, "MusicSelectedItem");
		}
	}

	public Visibility MusicConfigVisibility
	{
		get
		{
			return _musicConfigVisibility;
		}
		set
		{
			SetProperty(ref _musicConfigVisibility, value, "MusicConfigVisibility");
		}
	}
}

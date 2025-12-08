using System.Collections.ObjectModel;
using System.Windows.Input;
using OnlyRetroRobloxHere.Launcher.Models;

namespace OnlyRetroRobloxHere.Launcher.UI.ViewModels.Pages;

internal class InventoryViewModel : ViewModelBase
{
	private Cursor _regularCursor = Cursors.Arrow;

	private Cursor _hoverCursor = Cursors.Arrow;

	private string _hammerTimeText = "[ delete ]";

	public static ObservableCollection<AvatarItem> Inventory => Appearance.Inventory;

	public Cursor RegularCursor
	{
		get
		{
			return _regularCursor;
		}
		set
		{
			SetProperty(ref _regularCursor, value, "RegularCursor");
		}
	}

	public Cursor HoverCursor
	{
		get
		{
			return _hoverCursor;
		}
		set
		{
			SetProperty(ref _hoverCursor, value, "HoverCursor");
		}
	}

	public string HammerTimeText
	{
		get
		{
			return _hammerTimeText;
		}
		set
		{
			SetProperty(ref _hammerTimeText, value, "HammerTimeText");
		}
	}
}

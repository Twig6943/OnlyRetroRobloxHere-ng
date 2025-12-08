using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using OnlyRetroRobloxHere.Launcher.Models;
using OnlyRetroRobloxHere.Launcher.UI.ViewModels.Pages;

namespace OnlyRetroRobloxHere.Launcher.UI.Pages;

public partial class InventoryPage : BasePage, IComponentConnector, IStyleConnector
{
	private static Cursor _hammerCursor = new Cursor(Utils.GetStreamFromUri(new Uri("pack://application:,,,/OnlyRetroRobloxHere;component/Resources/Cursors/HammerCursor.cur")));

	private static Cursor _hammerHoverCursor = new Cursor(Utils.GetStreamFromUri(new Uri("pack://application:,,,/OnlyRetroRobloxHere;component/Resources/Cursors/HammerOverCursor.cur")));

	private bool _hammerModeOn;

	private InventoryViewModel _viewModel;

	public InventoryPage()
	{
		InitializeComponent();
		_viewModel = new InventoryViewModel();
		base.DataContext = _viewModel;
	}

	private static async void PlayExplosionSound()
	{
		SimpleAudioPlayer sap = new SimpleAudioPlayer("pack://application:,,,/OnlyRetroRobloxHere;component/Resources/Sounds/Explosion.ogg");
		sap.Play();
		await Task.Delay(1300);
		sap.Dispose();
	}

	private void OnDeleteClicked(object sender, RoutedEventArgs e)
	{
		if (!_hammerModeOn)
		{
			return;
		}
		if (!(((Button)sender).Tag is ulong num))
		{
			throw new Exception("Inventory item tag is not a ulong");
		}
		AvatarItem byId = AvatarItems.GetById(num);
		if (byId == null)
		{
			throw new Exception($"Item {num} does not exist.");
		}
		Appearance.RemoveItem(byId);
		if (byId.Items != null)
		{
			foreach (ulong item in byId.Items)
			{
				Appearance.RemoveItem(item);
			}
		}
		PlayExplosionSound();
	}

	private void OnWipeClicked(object sender, RoutedEventArgs e)
	{
		if (Utils.ShowMessageBox("Are you sure you want to delete ALL your items?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
		{
			return;
		}
		foreach (AvatarItem item in Appearance.Inventory.ToList())
		{
			Appearance.RemoveItem(item);
		}
		Sounds.PageTurn.Play();
	}

	private void Update()
	{
		_viewModel.RegularCursor = (_hammerModeOn ? _hammerCursor : Cursors.Arrow);
		_viewModel.HoverCursor = (_hammerModeOn ? _hammerHoverCursor : Cursors.Arrow);
		_viewModel.HammerTimeText = (_hammerModeOn ? "[ stop deleting ]" : "[ delete ]");
	}

	private void OnHammerTimeClicked(object sender, RoutedEventArgs e)
	{
		_hammerModeOn = !_hammerModeOn;
		Update();
		Sounds.Ping.Play();
	}

	public override void OnPageClosing()
	{
		_hammerModeOn = false;
		Update();
	}

}

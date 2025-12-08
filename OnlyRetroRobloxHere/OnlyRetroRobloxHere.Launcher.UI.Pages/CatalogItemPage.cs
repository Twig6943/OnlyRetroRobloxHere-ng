using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using OnlyRetroRobloxHere.Launcher.UI.Dialogs;
using OnlyRetroRobloxHere.Launcher.UI.ViewModels.Pages;

namespace OnlyRetroRobloxHere.Launcher.UI.Pages;

public partial class CatalogItemPage : BasePage, IComponentConnector, IStyleConnector
{
	private const int ProcessingTimeMinMs = 500;

	private const int ProcessingTimeMaxMs = 1250;

	private CatalogItemViewModel _viewModel;

	private SimpleAudioPlayer? _songPlayer;

	public CatalogItemPage(ulong id)
	{
		InitializeComponent();
		_viewModel = new CatalogItemViewModel();
		_viewModel.Item = AvatarItems.GetById(id);
		_viewModel.DoesNotOwn = !Appearance.OwnsItemWithSubItems(id);
		_viewModel.BoomBoxVisibility = ((id != 16975388) ? Visibility.Collapsed : Visibility.Visible);
		_viewModel.PropertyChanged += OnPropertyChanged;
		base.DataContext = _viewModel;
	}

	private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "MusicSelectedItem")
		{
			UpdateMusic();
		}
	}

	private void OnItemRecommendationClicked(object sender, RoutedEventArgs e)
	{
		if (!(((Button)sender).Tag is ulong id))
		{
			throw new Exception("Recommendation item tag is not a ulong");
		}
		((MainWindow)Application.Current.MainWindow).Navigate(new CatalogItemPage(id));
	}

	private void OnPurchaseClicked(object sender, RoutedEventArgs e)
	{
		_viewModel.ModalVisibility = Visibility.Visible;
	}

	private void CloseModalButton_Click(object sender, RoutedEventArgs e)
	{
		_viewModel.ModalVisibility = Visibility.Collapsed;
	}

	private async void DoSomeProcessing()
	{
		_viewModel.ModalPurchaseGridVisibility = Visibility.Collapsed;
		_viewModel.ModalProcessingGridVisibility = Visibility.Visible;
		await Task.Delay(new Random().Next(500, 1250));
		_viewModel.DoesNotOwn = false;
		_viewModel.ModalProcessingGridVisibility = Visibility.Collapsed;
		_viewModel.ModalSuccessGridVisibility = Visibility.Visible;
	}

	private void ModalBuyButton_Click(object sender, RoutedEventArgs e)
	{
		Appearance.AwardItem(_viewModel.Item.Id);
		DoSomeProcessing();
	}

	private void ModalGoToCatalogButton_Click(object sender, RoutedEventArgs e)
	{
		((MainWindow)Application.Current.MainWindow).Navigate("Catalog");
	}

	private void ModalGoToCharacterButton_Click(object sender, RoutedEventArgs e)
	{
		((MainWindow)Application.Current.MainWindow).NavigateByButton("Character");
	}

	private void UpdateMusic()
	{
		_songPlayer?.Dispose();
		_songPlayer = new SimpleAudioPlayer("pack://application:,,,/OnlyRetroRobloxHere;component/Resources/Sounds/Songs/" + _viewModel.MusicSelectedItem + ".ogg", loop: true);
		_songPlayer.Play();
	}

	private void BoomBoxButton_Click(object sender, RoutedEventArgs e)
	{
		_viewModel.MusicConfigVisibility = ((_viewModel.MusicConfigVisibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible);
	}

	public override void OnPageClosing()
	{
		_songPlayer?.Dispose();
		base.OnPageClosing();
	}

}

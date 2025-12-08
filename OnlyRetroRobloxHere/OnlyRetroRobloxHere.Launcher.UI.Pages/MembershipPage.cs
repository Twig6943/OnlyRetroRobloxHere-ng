using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using OnlyRetroRobloxHere.Common.Enums;
using OnlyRetroRobloxHere.Launcher.Enums;
using OnlyRetroRobloxHere.Launcher.UI.ViewModels.Pages;

namespace OnlyRetroRobloxHere.Launcher.UI.Pages;

public partial class MembershipPage : BasePage, IComponentConnector
{
	private const int NumOfAds = 3;

	private static readonly Dictionary<MembershipButtonType, double> _prices = new Dictionary<MembershipButtonType, double>
	{
		[MembershipButtonType.None] = 0.0,
		[MembershipButtonType.BCMonthly] = 5.95,
		[MembershipButtonType.BC6Months] = 29.95,
		[MembershipButtonType.BC12Months] = 57.95,
		[MembershipButtonType.BCLifetime] = 199.95,
		[MembershipButtonType.TBCMonthly] = 11.95,
		[MembershipButtonType.TBC6Months] = 44.95,
		[MembershipButtonType.TBC12Months] = 85.95,
		[MembershipButtonType.TBCLifetime] = 299.95,
		[MembershipButtonType.OBCMonthly] = 19.95
	};

	private Random _random = new Random();

	private MembershipViewModel _viewModel;

	private Storyboard _fadeIn;

	private Storyboard _fadeOut;

	private bool _animationPlaying;

	private Dictionary<MembershipButtonType, Action<bool>> _setters;

	private MembershipButtonType _selectedButton;

	public MembershipPage()
	{
		InitializeComponent();
		_fadeIn = (Storyboard)FindResource("OBCFadeIn");
		_fadeOut = (Storyboard)FindResource("OBCFadeOut");
		_fadeIn.Completed += OnStoryboardCompleted;
		_fadeOut.Completed += OnStoryboardCompleted;
		_viewModel = new MembershipViewModel();
		_setters = new Dictionary<MembershipButtonType, Action<bool>>
		{
			[MembershipButtonType.None] = delegate
			{
			},
			[MembershipButtonType.BCMonthly] = delegate(bool value)
			{
				_viewModel.BCMonthlyEnabled = value;
			},
			[MembershipButtonType.BC6Months] = delegate(bool value)
			{
				_viewModel.BC6MonthsEnabled = value;
			},
			[MembershipButtonType.BC12Months] = delegate(bool value)
			{
				_viewModel.BC12MonthsEnabled = value;
			},
			[MembershipButtonType.BCLifetime] = delegate(bool value)
			{
				_viewModel.BCLifetimeEnabled = value;
			},
			[MembershipButtonType.TBCMonthly] = delegate(bool value)
			{
				_viewModel.TBCMonthlyEnabled = value;
			},
			[MembershipButtonType.TBC6Months] = delegate(bool value)
			{
				_viewModel.TBC6MonthsEnabled = value;
			},
			[MembershipButtonType.TBC12Months] = delegate(bool value)
			{
				_viewModel.TBC12MonthsEnabled = value;
			},
			[MembershipButtonType.TBCLifetime] = delegate(bool value)
			{
				_viewModel.TBCLifetimeEnabled = value;
			},
			[MembershipButtonType.OBCMonthly] = delegate(bool value)
			{
				_viewModel.OBCMonthlyEnabled = value;
			}
		};
		ValidateSettings();
		SelectButton(Settings.Default.Launch.SelectedMembershipButton);
		base.DataContext = _viewModel;
	}

	private void SelectButton(MembershipButtonType button)
	{
		_setters[_selectedButton](obj: true);
		_setters[button](obj: false);
		_selectedButton = button;
	}

	private void ValidateSettings()
	{
		if (Settings.Default.Player.Membership == MembershipType.BuildersClub && !Settings.Default.Launch.SelectedMembershipButton.IsBC())
		{
			Settings.Default.Launch.SelectedMembershipButton = MembershipButtonType.BCMonthly;
			Settings.Default.Launch.MembershipDebt += _prices[MembershipButtonType.BCMonthly];
		}
		else if (Settings.Default.Player.Membership == MembershipType.TurboBuildersClub && !Settings.Default.Launch.SelectedMembershipButton.IsTBC())
		{
			Settings.Default.Launch.SelectedMembershipButton = MembershipButtonType.TBCMonthly;
			Settings.Default.Launch.MembershipDebt += _prices[MembershipButtonType.TBCMonthly];
		}
		else if (Settings.Default.Player.Membership == MembershipType.OutrageousBuildersClub && !Settings.Default.Launch.SelectedMembershipButton.IsOBC())
		{
			Settings.Default.Launch.SelectedMembershipButton = MembershipButtonType.OBCMonthly;
			Settings.Default.Launch.MembershipDebt += _prices[MembershipButtonType.OBCMonthly];
		}
	}

	private void OnStoryboardCompleted(object? sender, EventArgs e)
	{
		_animationPlaying = false;
	}

	private void OBCOpenButton_Click(object sender, RoutedEventArgs e)
	{
		if (!_animationPlaying)
		{
			_animationPlaying = true;
			_fadeIn.Begin();
		}
	}

	private void OBCCloseButton_Click(object sender, RoutedEventArgs e)
	{
		if (!_animationPlaying)
		{
			_animationPlaying = true;
			_fadeOut.Begin();
		}
	}

	private void MembershipButton_Click(object sender, RoutedEventArgs e)
	{
		MembershipButtonType membershipButtonType = Enum.Parse<MembershipButtonType>((string)((Button)sender).Tag);
		if (_selectedButton != membershipButtonType)
		{
			SelectButton(membershipButtonType);
			Settings.Default.Player.Membership = membershipButtonType.GetMembershipType();
			Settings.Default.Launch.SelectedMembershipButton = membershipButtonType;
			if (membershipButtonType != MembershipButtonType.None)
			{
				Sounds.CashRegister.Play();
			}
			else
			{
				Sounds.Ping.Play();
			}
			_viewModel.Debt += _prices[membershipButtonType];
		}
	}

	private void SelectRandomAd()
	{
		int value = _random.Next(1, 4);
		_viewModel.AdSource = new Uri($"pack://application:,,,/OnlyRetroRobloxHere;component/Resources/Ads/BC/{value}.jpg");
	}

	public override void OnPageOpening()
	{
		SelectRandomAd();
		base.OnPageOpening();
	}

	public override void OnPageClosing()
	{
		if (_animationPlaying)
		{
			_fadeIn.Stop();
			_fadeOut.Stop();
			_animationPlaying = false;
			RegularMembershipGrid.Visibility = Visibility.Visible;
			RegularMembershipGrid.Opacity = 1.0;
			OutrageousMembershipGrid.Visibility = Visibility.Collapsed;
		}
		base.OnPageClosing();
	}

}

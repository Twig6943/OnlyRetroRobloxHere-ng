using System;

namespace OnlyRetroRobloxHere.Launcher.UI.ViewModels.Pages;

internal class MembershipViewModel : ViewModelBase
{
	private bool _bcMonthlyEnabled = true;

	private bool _bc6MonthsEnabled = true;

	private bool _bc12MonthsEnabled = true;

	private bool _bcLifetimeEnabled = true;

	private bool _tbcMonthlyEnabled = true;

	private bool _tbc6MonthsEnabled = true;

	private bool _tbc12MonthsEnabled = true;

	private bool _tbcLifetimeEnabled = true;

	private bool _obcMonthlyEnabled = true;

	private Uri _adSource = new Uri("pack://application:,,,/OnlyRetroRobloxHere;component/Resources/Ads/BC/1.jpg");

	public bool BCMonthlyEnabled
	{
		get
		{
			return _bcMonthlyEnabled;
		}
		set
		{
			SetProperty(ref _bcMonthlyEnabled, value, "BCMonthlyEnabled");
		}
	}

	public bool BC6MonthsEnabled
	{
		get
		{
			return _bc6MonthsEnabled;
		}
		set
		{
			SetProperty(ref _bc6MonthsEnabled, value, "BC6MonthsEnabled");
		}
	}

	public bool BC12MonthsEnabled
	{
		get
		{
			return _bc12MonthsEnabled;
		}
		set
		{
			SetProperty(ref _bc12MonthsEnabled, value, "BC12MonthsEnabled");
		}
	}

	public bool BCLifetimeEnabled
	{
		get
		{
			return _bcLifetimeEnabled;
		}
		set
		{
			SetProperty(ref _bcLifetimeEnabled, value, "BCLifetimeEnabled");
		}
	}

	public bool TBCMonthlyEnabled
	{
		get
		{
			return _tbcMonthlyEnabled;
		}
		set
		{
			SetProperty(ref _tbcMonthlyEnabled, value, "TBCMonthlyEnabled");
		}
	}

	public bool TBC6MonthsEnabled
	{
		get
		{
			return _tbc6MonthsEnabled;
		}
		set
		{
			SetProperty(ref _tbc6MonthsEnabled, value, "TBC6MonthsEnabled");
		}
	}

	public bool TBC12MonthsEnabled
	{
		get
		{
			return _tbc12MonthsEnabled;
		}
		set
		{
			SetProperty(ref _tbc12MonthsEnabled, value, "TBC12MonthsEnabled");
		}
	}

	public bool TBCLifetimeEnabled
	{
		get
		{
			return _tbcLifetimeEnabled;
		}
		set
		{
			SetProperty(ref _tbcLifetimeEnabled, value, "TBCLifetimeEnabled");
		}
	}

	public bool OBCMonthlyEnabled
	{
		get
		{
			return _obcMonthlyEnabled;
		}
		set
		{
			SetProperty(ref _obcMonthlyEnabled, value, "OBCMonthlyEnabled");
		}
	}

	public string DebtText
	{
		get
		{
			if (Debt != 0.0)
			{
				return "Debt: $" + Debt.ToString("#,##.00");
			}
			return string.Empty;
		}
	}

	public double Debt
	{
		get
		{
			return Settings.Default.Launch.MembershipDebt;
		}
		set
		{
			if (Settings.Default.Launch.MembershipDebt != value)
			{
				Settings.Default.Launch.MembershipDebt = value;
				OnPropertyChanged("Debt");
				OnPropertyChanged("DebtText");
			}
		}
	}

	public Uri AdSource
	{
		get
		{
			return _adSource;
		}
		set
		{
			SetProperty(ref _adSource, value, "AdSource");
		}
	}
}

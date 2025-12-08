using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media.Imaging;
using OnlyRetroRobloxHere.Launcher.Models.Attributes;

namespace OnlyRetroRobloxHere.Launcher.UI.ViewModels.Pages;

internal class AboutViewModel : ViewModelBase
{
	private int _mattHeadSize = 63;

	public static string Version => Utils.Version;

	public static BuildMetadataAttribute BuildMetadata => Utils.BuildMetadata;

	public static string BuildTime => BuildMetadata.Timestamp.ToString("dddd, d MMMM yyyy 'at' h:mm:ss tt", CultureInfo.InvariantCulture);

	public Visibility PrivateVersionWarningVisibility { get; set; }

	public int MattHeadSize
	{
		get
		{
			return _mattHeadSize;
		}
		set
		{
			SetProperty(ref _mattHeadSize, value, "MattHeadSize");
		}
	}

	public BitmapImage BannerSource => new BitmapImage(new Uri("pack://application:,,,/OnlyRetroRobloxHere;component/Resources/Launcher/BannerLong" + (DateEvents.Winter ? "Winter" : "") + ".png"));

	public Visibility SnowfallVisibility
	{
		get
		{
			if (!DateEvents.Winter)
			{
				return Visibility.Collapsed;
			}
			return Visibility.Visible;
		}
	}
}

using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace OnlyRetroRobloxHere.Launcher.UI.ViewModels.Dialogs;

internal class MainWindowViewModel : ViewModelBase
{
	public static string Version => "v" + Utils.Version;

    public static Visibility DebugTextVisibility =>
    Utils.IsDebug ? Visibility.Visible : Visibility.Collapsed;

    public static string DebugText => "DEBUG";

	public BitmapImage BannerSource => new BitmapImage(new Uri("pack://application:,,,/OnlyRetroRobloxHere;component/Resources/Banner" + (DateEvents.Fall ? "Fall" : "") + (DateEvents.Spring ? "Spring" : "") + (DateEvents.Summer ? "Summer" : "") + (DateEvents.Pride ? "Pride" : "") + (DateEvents.Winter ? "Winter" : "") + ".png"));
}

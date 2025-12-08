using OnlyRetroRobloxHere.Launcher.UI.ViewModels.Pages;
using OnlyRetroRobloxHere.Launcher.UI.ViewModels.Pages.SettingsPages;

namespace OnlyRetroRobloxHere.Launcher.UI;

internal static class SharedViewModels
{
	public static PlayViewModel Play { get; }

	public static HostViewModel Host { get; }

	public static LauncherSettingsViewModel LauncherSettings { get; }

	static SharedViewModels()
	{
		Play = new PlayViewModel();
		Host = new HostViewModel();
		LauncherSettings = new LauncherSettingsViewModel();
	}
}

using System;
using System.Diagnostics;
using OnlyRetroRobloxHere.Launcher.UI.Dialogs;

namespace OnlyRetroRobloxHere.Launcher.Enums;

internal static class BootstrapperLaunchTypeEx
{
	public static BootstrapperWindowShared? GetBootstrapperWindow(this BootstrapperLaunchType type, Process process)
	{
		return type switch
		{
			BootstrapperLaunchType.Disabled => null, 
			BootstrapperLaunchType.Enabled => new BootstrapperWindow(process), 
			BootstrapperLaunchType.EnabledRetro => new BootstrapperRetroWindow(process), 
			_ => throw new Exception($"Unhandled BootstrapperLaunchType: {type}"), 
		};
	}
}

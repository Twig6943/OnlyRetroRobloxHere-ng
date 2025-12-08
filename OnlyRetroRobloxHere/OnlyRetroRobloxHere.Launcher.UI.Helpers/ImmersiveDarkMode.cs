using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace OnlyRetroRobloxHere.Launcher.UI.Helpers;

internal class ImmersiveDarkMode
{
	[DllImport("dwmapi.dll")]
	private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

	public static bool Apply(IntPtr handle)
	{
		try
		{
			if (IsWindows10OrGreater(17763))
			{
				int attr = (IsWindows10OrGreater(18985) ? 20 : 19);
				int attrValue = 1;
				return DwmSetWindowAttribute(handle, attr, ref attrValue, 4) == 0;
			}
		}
		catch
		{
		}
		return false;
	}

	public static bool ApplyWindow(Window window)
	{
		return Apply(new WindowInteropHelper(window).EnsureHandle());
	}

	private static bool IsWindows10OrGreater(int build = -1)
	{
		OperatingSystem oSVersion = Environment.OSVersion;
		if (oSVersion.Platform == PlatformID.Win32NT && oSVersion.Version.Major >= 6)
		{
			if (build != -1)
			{
				if (oSVersion.Version.Major <= 6)
				{
					if (oSVersion.Version.Major == 6)
					{
						return oSVersion.Version.Build >= build;
					}
					return false;
				}
				return true;
			}
			return oSVersion.Version.Major > 6;
		}
		return false;
	}
}

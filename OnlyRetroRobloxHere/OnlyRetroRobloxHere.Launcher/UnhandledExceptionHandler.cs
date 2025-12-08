using System;
using System.Windows;
using OnlyRetroRobloxHere.Common;

namespace OnlyRetroRobloxHere.Launcher;

internal static class UnhandledExceptionHandler
{
	public static void Handle(object sender, UnhandledExceptionEventArgs args)
	{
		Exception ex = (Exception)args.ExceptionObject;
		string text = ex.Message;
		try
		{
			Logger.Instance.Error("An unexpected error occured");
			Logger.Instance.Error(ex.ToString());
		}
		catch
		{
			text = ex.ToString();
		}
		if (!args.IsTerminating)
		{
			return;
		}
		try
		{
			Logger.Instance.Warn("We're closing! Saving everything!");
		}
		catch
		{
		}
		try
		{
			if (Settings.Initalized)
			{
				Settings.Default.Serialize();
			}
			SecureSettings.Save();
		}
		catch
		{
		}
		Utils.ShowMessageBox("An error has occured!\nPlease send your latest log file to a developer.\nException: " + text, MessageBoxButton.OK, MessageBoxImage.Hand);
	}
}

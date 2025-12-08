using System;
using OnlyRetroRobloxHere.Common;
using TextCopy;

namespace OnlyRetroRobloxHere.ServerAuthInterface;

internal static class InputMonitor
{
	public static void DisplayHelpMenu()
	{
		lock (Logger.Instance)
		{
			Logger.Instance.Info("Commands:");
			Logger.Instance.Info("C - Display the commands menu");
			Logger.Instance.Info("K - Generate a one use key");
			Logger.Instance.Info("I - Generate an infinite use key");
			Logger.Instance.Info("Key generation commands copy the key to the clipboard.");
		}
	}

	public static void Start()
	{
		while (true)
		{
			switch (Console.ReadKey(intercept: true).Key)
			{
			case ConsoleKey.C:
				DisplayHelpMenu();
				break;
			case ConsoleKey.K:
			{
				string text2 = KeyService.Instance.GenerateKey(infinite: false);
				Logger.Instance.Info("One use key generated: " + text2 + ". Copied to clipboard.");
				ClipboardService.SetTextAsync(text2);
				break;
			}
			case ConsoleKey.I:
			{
				string text = KeyService.Instance.GenerateKey(infinite: true);
				Logger.Instance.Info("Infinite use key generated: " + text + ". Copied to clipboard.");
				ClipboardService.SetTextAsync(text);
				break;
			}
			}
		}
	}
}

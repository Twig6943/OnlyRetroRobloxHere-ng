using System;

namespace OnlyRetroRobloxHere.Launcher;

internal static class DateEvents
{
	private const bool WinterOverride = false;

	private const bool AprilFoolsOverride = false;

	public static bool Winter => DateTime.Now.Month == 12;

	public static bool AprilFools
	{
		get
		{
			if (DateTime.Now.Month == 4)
			{
				return DateTime.Now.Day == 1;
			}
			return false;
		}
	}
}

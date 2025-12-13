using System;
using System.Net.NetworkInformation;

namespace OnlyRetroRobloxHere.Launcher;

internal static class DateEvents
{
    private const bool WinterOverride = false;

    private const bool AprilFoolsOverride = false;

#if DEBUG
    public static bool Pride = true;
    public static bool Winter = false;
    public static bool Summer = false;
    public static bool Spring = false;
    public static bool Fall = false;
#else
    public static bool Winter => DateTime.Now.Month == 12;
    public static bool Summer => DateTime.Now.Month == 7;
    public static bool Pride => DateTime.Now.Month == 6;
    public static bool Spring => DateTime.Now.Month == 4;
    public static bool Fall => DateTime.Now.Month == 9;
#endif

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
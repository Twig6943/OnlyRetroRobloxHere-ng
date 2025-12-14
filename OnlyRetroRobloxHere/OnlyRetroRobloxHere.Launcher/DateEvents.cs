using System;
using System.Net.NetworkInformation;

namespace OnlyRetroRobloxHere.Launcher;

/// <summary>
/// Class returns booleans for whether certain date-based events are active or not
/// </summary>
internal static class DateEvents
{

    /// <summary>
    /// Internal method to determine if an event is active based on Settings or Date.
    /// This allows for overriding events via the secret.
    /// </summary>
    /// <param name="eventName">The string key to check against Settings, like "winter"</param>
    /// <param name="dateCondition">The original date-based boolean logic</param>
    private static bool CheckEvent(string eventName, bool dateCondition)
    {
        if (Settings.Default.Launch.SecretEventOverride)
        {
            return string.Equals(Settings.Default.Launch.SecretEvent, eventName, StringComparison.OrdinalIgnoreCase);
        }

        return dateCondition;
    }

    public static bool Winter => CheckEvent("winter", DateTime.Now.Month == 12);
    public static bool Summer => CheckEvent("summer", DateTime.Now.Month == 7);
    // This theme isnt done at all yet
    public static bool Pride => CheckEvent("pride", DateTime.Now.Month == 0);
    public static bool Spring => CheckEvent("spring", DateTime.Now.Month == 4);
    public static bool Fall => CheckEvent("fall", DateTime.Now.Month == 9);

    public static bool AprilFools => CheckEvent("aprilfools", DateTime.Now.Month == 4 && DateTime.Now.Day == 1);
}
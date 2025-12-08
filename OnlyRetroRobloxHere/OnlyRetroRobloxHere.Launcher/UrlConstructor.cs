using System.Collections.Specialized;
using System.Web;
using OnlyRetroRobloxHere.Common.Enums;

namespace OnlyRetroRobloxHere.Launcher;

internal static class UrlConstructor
{
	private static string? GetQueryString(params (string, object)[] values)
	{
		NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(string.Empty);
		for (int i = 0; i < values.Length; i++)
		{
			var (name, obj) = values[i];
			nameValueCollection.Add(name, obj.ToString());
		}
		return nameValueCollection.ToString();
	}

	public static string CreateJoinUrl(string ip, ushort port, ChatStyle chatStyle)
	{
		string text = GetQueryString(("UserName", Settings.Default.Player.Name), ("serverIP", ip), ("serverPort", port), ("UserID", Settings.Default.Player.Id), ("chatStyle", chatStyle), ("testMode", false))?.ToString();
		return "http://www.roblox.com/game/join.ashx?" + text;
	}

	public static string CreateHostUrl(ushort port)
	{
		string text = GetQueryString(("serverPort", port))?.ToString();
		return "http://www.roblox.com/game/host.ashx?" + text;
	}
}

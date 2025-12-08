using System.Text.Json.Serialization;

namespace OnlyRetroRobloxHere.Launcher.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
internal enum PreferredStudioType
{
	MFC,
	Qt
}

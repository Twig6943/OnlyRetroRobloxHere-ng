using System.Text.Json.Serialization;

namespace OnlyRetroRobloxHere.Launcher.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
internal enum LaunchDirectory
{
	Player,
	Studio,
	MFCStudio,
	QtStudio,
	Server
}

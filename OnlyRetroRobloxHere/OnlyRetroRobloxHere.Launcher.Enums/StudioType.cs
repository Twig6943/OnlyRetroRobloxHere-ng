using System.Text.Json.Serialization;

namespace OnlyRetroRobloxHere.Launcher.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
internal enum StudioType
{
	None,
	App,
	MFC,
	Qt,
	MFCnQt
}

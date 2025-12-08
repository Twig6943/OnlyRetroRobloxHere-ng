using System.Text.Json.Serialization;

namespace OnlyRetroRobloxHere.Launcher.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
internal enum TTSEngine
{
	SAPI4,
	SAPI5
}

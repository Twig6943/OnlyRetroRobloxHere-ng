using System.Text.Json.Serialization;

namespace OnlyRetroRobloxHere.Launcher.Models;

internal class TTSVoices
{
	[JsonPropertyName("error")]
	public int Error { get; set; }

	[JsonPropertyName("sapi4")]
	public TTSEngineVoices SAPI4 { get; set; }

	[JsonPropertyName("sapi5")]
	public TTSEngineVoices SAPI5 { get; set; }
}

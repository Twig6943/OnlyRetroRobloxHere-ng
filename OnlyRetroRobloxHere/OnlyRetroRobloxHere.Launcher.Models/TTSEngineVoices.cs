using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OnlyRetroRobloxHere.Launcher.Models;

internal class TTSEngineVoices
{
	[JsonPropertyName("error")]
	public int Error { get; set; }

	[JsonPropertyName("voices")]
	public List<string>? Voices { get; set; }
}

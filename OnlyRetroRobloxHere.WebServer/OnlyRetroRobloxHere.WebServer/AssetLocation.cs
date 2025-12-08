using System.Text.Json.Serialization;

namespace OnlyRetroRobloxHere.WebServer;

internal class AssetLocation
{
	[JsonPropertyName("location")]
	public string? Location { get; set; }
}

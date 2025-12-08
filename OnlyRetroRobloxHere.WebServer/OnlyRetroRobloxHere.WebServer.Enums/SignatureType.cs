using System.Text.Json.Serialization;

namespace OnlyRetroRobloxHere.WebServer.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
internal enum SignatureType
{
	None,
	Legacy,
	RbxSig
}

using System.ComponentModel;
using System.Text.Json.Serialization;

namespace OnlyRetroRobloxHere.Launcher.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
internal enum VCPPRedist
{
	None,
	[Description("VC++ 2005")]
	VCPP2005,
	[Description("VC++ 2008")]
	VCPP2008,
	[Description("VC++ 2012")]
	VCPP2012
}

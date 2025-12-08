using System.Text.Json.Serialization;

namespace OnlyRetroRobloxHere.Launcher.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
internal enum MembershipButtonType
{
	None,
	BCMonthly,
	BC6Months,
	BC12Months,
	BCLifetime,
	TBCMonthly,
	TBC6Months,
	TBC12Months,
	TBCLifetime,
	OBCMonthly
}

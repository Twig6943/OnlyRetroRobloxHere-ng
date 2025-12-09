using System.ComponentModel;

namespace OnlyRetroRobloxHere.Common.Enums;
/// <summary>
/// Represents the different types of membership available.
/// </summary>
/// <remarks>
/// This enumeration defines the membership tiers, including standard and premium options.
/// The values include <see cref="None"/>, <see cref="BuildersClub"/>, <see cref="TurboBuildersClub"/>, and <see cref="OutrageousBuildersClub"/>.
/// </remarks>
public enum MembershipType
{
	None,
	[Description("Builders Club")]
	BuildersClub,
	[Description("Turbo Builders Club")]
	TurboBuildersClub,
	[Description("Outrageous Builders Club")]
	OutrageousBuildersClub
}

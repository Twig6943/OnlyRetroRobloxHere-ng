using System.ComponentModel;
using System.Text.Json.Serialization;

namespace OnlyRetroRobloxHere.Common.Enums;

/// <summary>
/// Represents the type of an avatar asset in Roblox.
/// </summary>
/// <remarks>
/// Has 14 different values, each representing a specific type of avatar asset.
/// The values include <see cref="TShirt"/>, <see cref="Shirt"/>, <see cref="Pants"/>, <see cref="Hat"/>, <see cref="Face"/>, <see cref="Head"/>, <see cref="Torso"/>,
/// <see cref="LeftArm"/>, <see cref="RightArm"/>, <see cref="LeftLeg"/>, <see cref="RightLeg"/>, <see cref="Package"/>, and <see cref="Gear"/>.
/// </remarks>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AvatarAssetType
{
	[Description("T-Shirt")]
	TShirt,
	Shirt,
	Pants,
	Hat,
	Face,
	Head,
	Torso,
	[Description("Left Arm")]
	LeftArm,
	[Description("Right Arm")]
	RightArm,
	[Description("Left Leg")]
	LeftLeg,
	[Description("Right Leg")]
	RightLeg,
	Package,
	Gear
}

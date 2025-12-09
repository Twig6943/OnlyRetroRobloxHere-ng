namespace OnlyRetroRobloxHere.Common.Enums;

/// <summary>
/// Represents the versions of the Asset Pack API.
/// </summary>
/// <remarks>
/// <see cref="AssetPackApi"/> is an enumeration that defines the different versions of the Asset Pack API.
/// It has three values: <see cref="None"/>, <see cref="V1"/>, and <see cref="SodikmV1"/>. V1 metadata uses JSON, while SodikmV1 metadata uses INI.
/// </remarks>
public enum AssetPackApi
{
	None,
	V1,
	SodikmV1
}

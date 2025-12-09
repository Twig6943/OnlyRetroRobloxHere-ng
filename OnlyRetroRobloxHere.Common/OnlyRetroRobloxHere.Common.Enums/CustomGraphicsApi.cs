using System.ComponentModel;

namespace OnlyRetroRobloxHere.Common.Enums;

/// <summary>
/// Represents the custom graphics API options available for Roblox.
/// </summary>
/// <remarks>
/// The values include <see cref="None"/>, <see cref="DXVK"/>, and <see cref="DgVoodoo"/>.
/// </remarks>
public enum CustomGraphicsApi
{
	None,
	[Description("DXVK (Vulkan)")]
	DXVK,
	[Description("dgVoodoo (DX11/12)")]
	DgVoodoo
}

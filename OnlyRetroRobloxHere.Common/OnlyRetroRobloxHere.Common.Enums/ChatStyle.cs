using System.ComponentModel;
using System.Text.Json.Serialization;

namespace OnlyRetroRobloxHere.Common.Enums;

/// <summary>
/// Represents the available styles for chat display.
/// </summary>
/// <remarks>
/// This enumeration defines the visual presentation styles for chat messages.
/// The values include <see cref="Classic"/>, <see cref="Bubble"/>, and <see cref="ClassicAndBubble"/>.
/// </remarks>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ChatStyle
{
	Classic,
	Bubble,
	[Description("Classic and Bubble")]
	ClassicAndBubble
}

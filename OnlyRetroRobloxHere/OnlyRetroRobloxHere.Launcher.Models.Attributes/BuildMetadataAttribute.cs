using System;

namespace OnlyRetroRobloxHere.Launcher.Models.Attributes;

[AttributeUsage(AttributeTargets.Assembly)]
internal class BuildMetadataAttribute : Attribute
{
	public DateTime Timestamp { get; set; }

	public string Configuration { get; set; }

	public BuildMetadataAttribute(string timestamp)
	{
		Timestamp = DateTime.Parse(timestamp).ToLocalTime();
		Configuration = "Release";
	}
}

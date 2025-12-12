using System;

namespace OnlyRetroRobloxHere.Launcher.Models.Attributes;

[AttributeUsage(AttributeTargets.Assembly)]
internal class BuildMetadataAttribute : Attribute
{
    public DateTime Timestamp { get; set; }

    public string Configuration { get; set; }

    // Now without hardcoded configuration
    public BuildMetadataAttribute(string timestamp, string configuration)
    {
        Timestamp = DateTime.Parse(timestamp).ToLocalTime();
        Configuration = configuration;
    }
}

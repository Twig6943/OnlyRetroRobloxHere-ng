using System;
using System.Linq;
using OnlyRetroRobloxHere.Launcher.Enums;

namespace OnlyRetroRobloxHere.Launcher.Models;

internal class ClientConfig
{
	public class _Player
	{
		public string ExecutableName { get; set; } = "";

		public string LaunchArguments { get; set; } = "";

		public bool Sensitive { get; set; }
	}

	public class _Studio
	{
		public StudioType Type { get; set; }

		public string ExecutableName { get; set; } = "";

		public string MFCExecutableName { get; set; } = "";

		public string QtExecutableName { get; set; } = "";
	}

	public class _Server
	{
		public string ExecutableName { get; set; } = "";

		public string LaunchArguments { get; set; } = "";

		public LaunchDirectory Directory { get; set; }
	}

	public class _CharacterCompatibility
	{
		public bool FigureBodyColours { get; set; }

		public bool TShirts { get; set; }

		public bool Hats { get; set; }

		public bool ShirtsAndPants { get; set; }

		public bool Faces { get; set; }

		public bool Heads { get; set; }

		public bool ExtendedColours { get; set; }

		public bool BodyParts { get; set; }
	}

	public string Name { get; set; } = "";

	public string Description { get; set; } = "";

	public _Player Player { get; set; } = new _Player();

	public _Studio Studio { get; set; } = new _Studio();

	public _Server Server { get; set; } = new _Server();

	public _CharacterCompatibility CharacterCompatibility { get; set; } = new _CharacterCompatibility();

	public string[] Tags { get; set; } = Array.Empty<string>();

	public bool HasTag(string tag)
	{
		return Tags.Contains(tag);
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using OnlyRetroRobloxHere.Common;
using OnlyRetroRobloxHere.Common.Enums;
using OnlyRetroRobloxHere.Common.Models;
using OnlyRetroRobloxHere.WebServer.Enums;

namespace OnlyRetroRobloxHere.WebServer;

internal class Config
{
	internal class _Client
	{
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

		private ClientYear? _clientYear;

		[JsonIgnore]
		public string ClientName { get; set; } = "";

		[JsonIgnore]
		public ClientYear ClientYear => _clientYear ?? (_clientYear = new ClientYear(ClientName));

		public SignatureType Signature { get; set; }

		public CharacterLoadType CharacterLoadType { get; set; }

		public bool SignAssetScripts { get; set; } = true;

		public bool ClientWillDieIfAHttpRedirectHappens { get; set; }

		public _CharacterCompatibility CharacterCompatibility { get; set; } = new _CharacterCompatibility();
	}

	internal class _User
	{
		internal class _Launch
		{
			public string? SelectedMap { get; set; }

			public bool AutoSaveEnabled { get; set; }

			public bool ExperimentalPlayerlistEnabled { get; set; }

			public bool EggHuntMode { get; set; }

			public bool AutoSaveDebug { get; set; }

			public uint AutoSaveChangesPerPlayer { get; set; } = 100u;

			public uint AutoSaveCheckInterval { get; set; } = 1800u;

			public uint AutoSaveCooldown { get; set; } = 900u;

			public string CustomMapsDirectory { get; set; } = "";

			public List<string> DisabledAssetPacks { get; set; } = new List<string>();
		}

		internal class _Player
		{
			public int Id { get; set; }

			public string Name { get; set; } = "";

			public MembershipType Membership { get; set; }
		}

		public _Launch Launch { get; set; } = new _Launch();

		public _Player Player { get; set; } = new _Player();

		public Character Character { get; set; } = new Character();

		public Dictionary<string, string> OutfitPreferences { get; set; } = new Dictionary<string, string>();
	}

	public static Config Instance { get; private set; }

	public _Client Client { get; private set; } = new _Client();

	public _User User { get; private set; } = new _User();

	public bool IsRenderMode { get; set; }

	public static void Init(string client)
	{
		Instance = new Config();
		ClientPaths.SetClientName(client);
		if (!File.Exists(ClientPaths.Config))
		{
			throw new Exception("Config for requested client " + client + " does not exist");
		}
		_Client client2 = JsonSerializer.Deserialize<_Client>(File.ReadAllText(ClientPaths.Config));
		if (client2 == null)
		{
			throw new Exception("Failed to deserialize client config for " + client);
		}
		Instance.Client = client2;
		Instance.Client.ClientName = client;
		if (!File.Exists(PathHelper.Settings))
		{
			throw new Exception("No user settings found");
		}
		_User user = JsonSerializer.Deserialize<_User>(File.ReadAllText(PathHelper.Settings));
		if (user == null)
		{
			throw new Exception("Failed to deserialize user settings");
		}
		Instance.User = user;
	}
}

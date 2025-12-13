using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using OnlyRetroRobloxHere.Common;
using OnlyRetroRobloxHere.Common.Enums;
using OnlyRetroRobloxHere.Common.Models;
using OnlyRetroRobloxHere.Launcher.Enums;

namespace OnlyRetroRobloxHere.Launcher;

internal class Settings
{
	public delegate void PreSerializationHandler(Settings settings);

	public class _Launch
	{
		private uint _fpsCap = 60u;

		private uint _fpsCapOlder = 30u;

		public string? SelectedMap { get; set; }

		public string? SelectedClient { get; set; }

		public PreferredStudioType PreferredStudioType { get; set; }

		public BootstrapperLaunchType BootstrapperLaunchType { get; set; } = BootstrapperLaunchType.EnabledRetro;

		public bool AutoSaveEnabled { get; set; }

		public bool ExperimentalPlayerlistEnabled { get; set; }

		public CustomGraphicsApi CustomGraphicsApi { get; set; }

		public bool EggHuntMode { get; set; }

        public bool HackCustomHats { get; set; }

        public string? SecretEvent { get; set; }

		public bool SecretEventOverride { get; set; } = false;

        public bool AutoSaveDebug { get; set; }

		public uint AutoSaveChangesPerPlayer { get; set; } = 100u;

		public uint AutoSaveCheckInterval { get; set; } = 1800u;

		public uint AutoSaveCooldown { get; set; } = 900u;

		public bool ClientLargerPlayerListNameLength { get; set; } = true;

		public bool ClientLegacyUIDarkMode { get; set; }

		public bool ClientHigherQualityScreenshots { get; set; } = true;

		public bool DebugShowWebServerWindow { get; set; }

		public bool CustomInGameApisEnabled { get; set; }

		public uint FPSCap
		{
			get
			{
				return _fpsCap;
			}
			set
			{
				_fpsCap = Math.Clamp(value, 60u, 240u);
			}
		}

		public uint FPSCapOlder
		{
			get
			{
				return _fpsCapOlder;
			}
			set
			{
				_fpsCapOlder = Math.Clamp(value, 30u, 240u);
			}
		}

		public bool TTSEnabled { get; set; }

		public TTSEngine TTSType { get; set; } = TTSEngine.SAPI5;

		public string TTSVoice { get; set; } = "";

		public string CustomMapsDirectory { get; set; } = "";

		public int DisplayPort { get; set; } = -1;

		public string DisplayIP { get; set; } = "";

		public MembershipButtonType SelectedMembershipButton { get; set; }

		public double MembershipDebt { get; set; }

		public bool ServerNo3D { get; set; }

		public bool OptimisedClientStartup { get; set; } = true;

		public bool RemoveUploadDialogs { get; set; } = true;

		public List<string> DisabledAssetPacks { get; set; } = new List<string>();
	}

	public class _Player
	{
		private int? _id;

		private string? _name;

		public int Id
		{
			get
			{
				int valueOrDefault = _id.GetValueOrDefault();
				if (!_id.HasValue)
				{
					valueOrDefault = PlayerRandomiser.GenerateId();
					_id = valueOrDefault;
					return valueOrDefault;
				}
				return valueOrDefault;
			}
			set
			{
				_id = value;
			}
		}

		public string Name
		{
			get
			{
				return _name ?? (_name = PlayerRandomiser.GenerateUsername());
			}
			set
			{
				_name = value;
			}
		}

		public MembershipType Membership { get; set; }
	}

	private const bool DebugDisableSaving = false;

	internal const int MinFPS = 60;

	internal const int MaxFPS = 240;

	internal const int MinFPSOlder = 30;

	internal const int MaxFPSOlder = 240;

	internal const int AutoSaveChangesPerPlayerDefault = 100;

	internal const int AutoSaveCheckIntervalDefault = 1800;

	internal const int AutoSaveCooldownDefault = 900;

	private static Settings? _default;

	public static Settings Default => _default ?? (_default = Get());

	public static bool Initalized => _default != null;

	public _Launch Launch { get; set; } = new _Launch();

	public _Player Player { get; set; } = new _Player();

	public Character Character { get; set; } = new Character();

	public List<ulong> Inventory { get; set; } = new List<ulong>();

	public Dictionary<string, string> OutfitPreferences { get; set; } = new Dictionary<string, string>();

	public event PreSerializationHandler? PreSerialization;

	private static Settings Deserialize()
	{
		if (File.Exists(PathHelper.SettingsLegacy))
		{
			if (!File.Exists(PathHelper.Settings))
			{
				Logger.Instance.Info("Found settings file in old path. Migrating!");
				File.Move(PathHelper.SettingsLegacy, PathHelper.Settings);
			}
			else
			{
				Logger.Instance.Info("Found settings file in old path. Settings already exist in new path. Deleting!");
				File.Delete(PathHelper.SettingsLegacy);
			}
		}
		if (!File.Exists(PathHelper.Settings))
		{
			Logger.Instance.Info("Settings file does not exist, creating a new one.");
			return GetNew();
		}
		try
		{
			return JsonSerializer.Deserialize<Settings>(File.ReadAllText(PathHelper.Settings)) ?? throw new Exception("Deserialised JSON is null");
		}
		catch (Exception value)
		{
			Logger.Instance.Error($"Failed to deserialise settings: {value}");
			return GetNew();
		}
	}

	private static Settings GetNew()
	{
		return new Settings();
	}

	private static Settings Get()
	{
		if (File.Exists(PathHelper.Settings) || File.Exists(PathHelper.SettingsLegacy))
		{
			return Deserialize();
		}
		return GetNew();
	}

	public void Serialize()
	{
		this.PreSerialization?.Invoke(this);
		File.WriteAllText(PathHelper.Settings, JsonSerializer.Serialize(this));
	}
}

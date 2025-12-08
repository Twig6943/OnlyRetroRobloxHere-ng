using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using OnlyRetroRobloxHere.Common.Collection;
using OnlyRetroRobloxHere.Common.Models;

namespace OnlyRetroRobloxHere.Common;

public class OutfitManager
{
	public struct Result
	{
		public bool Success;

		public string Message;

		internal Result(bool success)
		{
			Success = success;
			Message = null;
		}

		internal Result(bool success, string message)
		{
			Success = success;
			Message = message;
		}
	}

	private const int MinNameLength = 1;

	private const int MaxNameLength = 50;

	private Dictionary<string, string>? _preferredOutfitsList;

	public ObservableDictionary<string, Outfit> Outfits { get; } = new ObservableDictionary<string, Outfit>();

	public OutfitManager(Dictionary<string, string>? preferredOutfitsList = null)
	{
		_preferredOutfitsList = preferredOutfitsList;
		Load();
	}

	private Outfit? GetOutfitByPath(string path)
	{
		try
		{
			Character character = JsonSerializer.Deserialize<Character>(File.ReadAllText(path));
			if (character == null)
			{
				throw new Exception("Deserialised character JSON is NULL!");
			}
			return new Outfit
			{
				Name = Path.GetFileNameWithoutExtension(path),
				Character = character
			};
		}
		catch (Exception value)
		{
			Logger.Instance.Error($"Failed to get outfit at path {path}: {value}");
			return null;
		}
	}

	private void Load()
	{
		MigrateOutfits();
		string[] outfitPaths = GetOutfitPaths();
		Dictionary<string, string> dictionary;
		if (_preferredOutfitsList != null)
		{
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			foreach (KeyValuePair<string, string> preferredOutfits in _preferredOutfitsList)
			{
				if (!list.Contains(preferredOutfits.Value))
				{
					list.Add(preferredOutfits.Value);
				}
				else
				{
					list2.Add(preferredOutfits.Key);
				}
			}
			foreach (string item in list2)
			{
				_preferredOutfitsList.Remove(item);
			}
			dictionary = _preferredOutfitsList.ToDictionary<KeyValuePair<string, string>, string, string>((KeyValuePair<string, string> e) => e.Value, (KeyValuePair<string, string> e) => e.Key);
		}
		else
		{
			dictionary = new Dictionary<string, string>();
		}
		string[] array = outfitPaths;
		foreach (string path in array)
		{
			Outfit outfitByPath = GetOutfitByPath(path);
			if (outfitByPath != null)
			{
				if (dictionary.ContainsKey(outfitByPath.Name))
				{
					outfitByPath.Client = dictionary[outfitByPath.Name];
				}
				Outfits[outfitByPath.Name.ToLowerInvariant()] = outfitByPath;
			}
		}
		RemoveNonExistentOutfitsFromPreferredOutfits();
	}

	private static Dictionary<T, U> CopyDictionary<T, U>(Dictionary<T, U> dictionary) where T : notnull
	{
		return dictionary.ToDictionary<KeyValuePair<T, U>, T, U>((KeyValuePair<T, U> entry) => entry.Key, (KeyValuePair<T, U> entry) => entry.Value);
	}

	private void MigrateOutfits()
	{
		if (!Directory.Exists(PathHelper.UserOutfitsLegacy))
		{
			return;
		}
		Logger.Instance.Info("Found user outfits in old path. Migrating.");
		Directory.CreateDirectory(PathHelper.UserOutfits);
		string[] files = Directory.GetFiles(PathHelper.UserOutfitsLegacy);
		foreach (string text in files)
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text);
			string outfitPath = GetOutfitPath(fileNameWithoutExtension);
			if (File.Exists(outfitPath))
			{
				Logger.Instance.Info("MigrateOutfits: Outfit with name " + fileNameWithoutExtension + " already exists.");
				continue;
			}
			Logger.Instance.Info("MigrateOutfits: Migrating " + fileNameWithoutExtension + ".");
			File.Move(text, outfitPath);
		}
		try
		{
			Directory.Delete(PathHelper.UserOutfitsLegacy, recursive: true);
		}
		catch (Exception value)
		{
			Logger.Instance.Info($"Failed to delete old user outfits path: {value}");
		}
		Logger.Instance.Info("Finished migration.");
	}

	private void RemoveNonExistentOutfitsFromPreferredOutfits()
	{
		if (_preferredOutfitsList == null)
		{
			return;
		}
		foreach (KeyValuePair<string, string> item in CopyDictionary(_preferredOutfitsList))
		{
			string key = item.Value.ToLowerInvariant();
			if (!Outfits.ContainsKey(key))
			{
				_preferredOutfitsList.Remove(item.Key);
			}
		}
	}

	private string GetOutfitPath(string outfitName)
	{
		Directory.CreateDirectory(PathHelper.UserOutfits);
		return Path.Combine(PathHelper.UserOutfits, outfitName + ".json");
	}

	private string[] GetOutfitPaths()
	{
		Directory.CreateDirectory(PathHelper.UserOutfits);
		return Directory.GetFiles(PathHelper.UserOutfits);
	}

	public Outfit? GetOutfit(string outfitName)
	{
		string key = outfitName.ToLowerInvariant();
		if (Outfits.ContainsKey(key))
		{
			return Outfits[key];
		}
		return null;
	}

	public IEnumerable<Outfit> GetOutfits()
	{
		return Outfits.Select<KeyValuePair<string, Outfit>, Outfit>((KeyValuePair<string, Outfit> x) => x.Value);
	}

	private void SaveOutfit(string name, Character character)
	{
		string outfitPath = GetOutfitPath(name);
		string contents = JsonSerializer.Serialize(character);
		File.WriteAllText(outfitPath, contents);
	}

	public void SaveOutfit(Outfit outfit, Character character)
	{
		SaveOutfit(outfit.Name, character);
		Outfits[outfit.Name.ToLowerInvariant()] = outfit;
	}

	public Result CreateOutfit(string name, Character character)
	{
		string key = name.ToLowerInvariant();
		if (Outfits.ContainsKey(key))
		{
			return new Result(success: false, "Name is already taken.");
		}
		if (name.Length < 1)
		{
			return new Result(success: false, "Name is too short.");
		}
		if (name.Length > 50)
		{
			return new Result(success: false, "Name is too long.");
		}
		if (!PathHelper.IsFileNameValid(name))
		{
			return new Result(success: false, "Name is not valid.");
		}
		try
		{
			SaveOutfit(name, character);
			Character character2 = (Character)character.Clone();
			Outfit outfit = new Outfit();
			outfit.Name = name;
			outfit.Character = character2;
			Outfits[key] = outfit;
		}
		catch (Exception value)
		{
			Logger.Instance.Error($"An exception occured while creating outfit {name}: {value}");
			return new Result(success: false, "An exception occured. View logs for more details.");
		}
		return new Result(success: true);
	}

	public Result DeleteOutfit(string name)
	{
		string key = name.ToLowerInvariant();
		if (!Outfits.ContainsKey(key))
		{
			return new Result(success: false, "Outfit not found.");
		}
		string outfitPath = GetOutfitPath(name);
		if (!File.Exists(outfitPath))
		{
			return new Result(success: false, "Outfit file does not exist.");
		}
		try
		{
			File.Delete(outfitPath);
		}
		catch (Exception value)
		{
			Logger.Instance.Error($"An exception occured while deleting outfit {name}: {value}");
			return new Result(success: false, "An exception occured. View logs for more details.");
		}
		if (_preferredOutfitsList != null)
		{
			IEnumerable<KeyValuePair<string, string>> source = _preferredOutfitsList.Where<KeyValuePair<string, string>>((KeyValuePair<string, string> x) => x.Value == name);
			if (source.Any())
			{
				_preferredOutfitsList.Remove(source.First().Key);
			}
		}
		Outfits.Remove(key);
		return new Result(success: true);
	}

	public Result DeleteOutfit(Outfit outfit)
	{
		return DeleteOutfit(outfit.Name);
	}

	public Result RenameOutfit(string name, string newName)
	{
		Outfit outfit = GetOutfit(name);
		if (outfit == null)
		{
			return new Result(success: false, "Outfit not found.");
		}
		return RenameOutfit(outfit, newName);
	}

	public Result RenameOutfit(Outfit outfit, string newName)
	{
		string key = newName.ToLowerInvariant();
		if (Outfits.ContainsKey(key))
		{
			return new Result(success: false, "New name is already taken.");
		}
		string outfitPath = GetOutfitPath(outfit.Name);
		if (!File.Exists(outfitPath))
		{
			return new Result(success: false, "Outfit file not found.");
		}
		if (newName.Length < 1)
		{
			return new Result(success: false, "New name is too short.");
		}
		if (newName.Length > 50)
		{
			return new Result(success: false, "New name is too long.");
		}
		if (!PathHelper.IsFileNameValid(newName))
		{
			return new Result(success: false, "New name is not valid.");
		}
		string outfitPath2 = GetOutfitPath(newName);
		if (File.Exists(outfitPath2))
		{
			return new Result(success: false, "Outfit file for new name already exists.");
		}
		try
		{
			File.Move(outfitPath, outfitPath2);
		}
		catch (Exception value)
		{
			Logger.Instance.Error($"An exception occured while renaming outfit {outfit.Name} to {newName}: {value}");
			return new Result(success: false, "An exception occured. View logs for more details.");
		}
		string key2 = outfit.Name.ToLowerInvariant();
		Outfits.Remove(key2);
		outfit.Name = newName;
		Outfits[key] = outfit;
		return new Result(success: true);
	}

	public void SetPreferredOutfitForClient(Outfit outfit, string? client)
	{
		if (_preferredOutfitsList == null)
		{
			throw new NullReferenceException("_preferredOutfitsList");
		}
		if (client == "None")
		{
			client = null;
		}
		outfit.Client = client;
		IEnumerable<KeyValuePair<string, string>> source = _preferredOutfitsList.Where<KeyValuePair<string, string>>((KeyValuePair<string, string> x) => x.Value == outfit.Name);
		if (source.Any())
		{
			_preferredOutfitsList.Remove(source.First().Key);
		}
		if (client != null)
		{
			_preferredOutfitsList[client] = outfit.Name;
		}
	}

	public Outfit? GetPreferredOutfitForClient(string client)
	{
		if (_preferredOutfitsList == null)
		{
			throw new NullReferenceException("_preferredOutfitsList");
		}
		if (_preferredOutfitsList.ContainsKey(client))
		{
			string key = _preferredOutfitsList[client].ToLowerInvariant();
			return Outfits[key];
		}
		return null;
	}

	public List<string> GetClientsWithPreferred()
	{
		if (_preferredOutfitsList == null)
		{
			throw new NullReferenceException("_preferredOutfitsList");
		}
		return _preferredOutfitsList.Select<KeyValuePair<string, string>, string>((KeyValuePair<string, string> x) => x.Key).ToList();
	}
}

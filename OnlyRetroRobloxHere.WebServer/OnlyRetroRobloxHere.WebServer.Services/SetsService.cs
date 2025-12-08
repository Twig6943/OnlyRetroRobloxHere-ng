using System.Collections.Generic;
using System.IO;
using OnlyRetroRobloxHere.Common;
using OnlyRetroRobloxHere.Common.Models;

namespace OnlyRetroRobloxHere.WebServer.Services;

internal class SetsService
{
	private Dictionary<int, string> _sets = new Dictionary<int, string>();

	private Dictionary<int, string> _users = new Dictionary<int, string>();

	public static SetsService Instance { get; }

	public SetsService()
	{
		string path = Path.Combine(PathHelper.Data, "sets");
		ParseData(_sets, Path.Combine(path, "set"));
		ParseData(_users, Path.Combine(path, "user"));
	}

	public string GetSet(int setId)
	{
		if (_sets.ContainsKey(setId))
		{
			return File.ReadAllText(_sets[setId]);
		}
		return "<List></List>";
	}

	public string GetUserSets(int userId)
	{
		if (_users.ContainsKey(userId))
		{
			return File.ReadAllText(_users[userId]);
		}
		return "<List></List>";
	}

	public string GetBaseSet()
	{
		return File.ReadAllText(Path.Combine(PathHelper.Data, "sets", "base.xml"));
	}

	private void ParseData(Dictionary<int, string> map, string directory)
	{
		Dictionary<int, (string, ClientYear)> dictionary = new Dictionary<int, (string, ClientYear)>();
		string[] files = Directory.GetFiles(directory);
		foreach (string text in files)
		{
			if (!text.EndsWith(".xml"))
			{
				continue;
			}
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text);
			int result2;
			if (fileNameWithoutExtension.Contains('_'))
			{
				string[] array = fileNameWithoutExtension.Split('_');
				if (int.TryParse(array[0], out var result))
				{
					string year = array[1];
					ClientYear clientYear = new ClientYear(year);
					if (!dictionary.ContainsKey(result))
					{
						dictionary[result] = (text, clientYear);
					}
					else if (dictionary[result].Item2 > Config.Instance.Client.ClientYear && dictionary[result].Item2 < Config.Instance.Client.ClientYear)
					{
						dictionary[result] = (text, clientYear);
					}
					else if (dictionary[result].Item2 < clientYear && clientYear <= Config.Instance.Client.ClientYear)
					{
						dictionary[result] = (text, clientYear);
					}
				}
			}
			else if (int.TryParse(fileNameWithoutExtension, out result2))
			{
				dictionary[result2] = (text, ClientYear.Blank);
			}
		}
		foreach (KeyValuePair<int, (string, ClientYear)> item in dictionary)
		{
			map[item.Key] = item.Value.Item1;
		}
	}

	public void Test()
	{
	}

	static SetsService()
	{
		Instance = new SetsService();
	}
}

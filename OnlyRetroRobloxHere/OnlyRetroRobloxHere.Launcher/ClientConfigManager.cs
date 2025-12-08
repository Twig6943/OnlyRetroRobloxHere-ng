using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using OnlyRetroRobloxHere.Common;
using OnlyRetroRobloxHere.Launcher.Models;

namespace OnlyRetroRobloxHere.Launcher;

internal static class ClientConfigManager
{
	private static Dictionary<string, ClientConfig> _map = new Dictionary<string, ClientConfig>();

	public static string? SelectedClientName { get; private set; } = null;

	public static ClientConfig? SelectedClientConfig { get; private set; } = null;

	public static string GetClientConfigPath(string client)
	{
		return Path.Combine(PathHelper.Clients, client, "OnlyRetroRobloxHere_Config.json");
	}

	public static ClientConfig? Get(string client)
	{
		lock (_map)
		{
			if (!_map.ContainsKey(client))
			{
				string clientConfigPath = GetClientConfigPath(client);
				if (!File.Exists(clientConfigPath))
				{
					Utils.ShowMessageBox("Client config is missing", MessageBoxButton.OK, MessageBoxImage.Hand);
					return null;
				}
				try
				{
					ClientConfig clientConfig = JsonSerializer.Deserialize<ClientConfig>(File.ReadAllText(clientConfigPath));
					if (clientConfig == null)
					{
						throw new Exception("Deserialised JSON is NULL");
					}
					_map[client] = clientConfig;
				}
				catch (Exception ex)
				{
					Logger.Instance.Error($"An error occured while parsing the client config: {ex}");
					Utils.ShowMessageBox("An error occured while parsing the client config: " + ex.Message, MessageBoxButton.OK, MessageBoxImage.Hand);
					return null;
				}
			}
			return _map[client];
		}
	}

	public static void Select(string client)
	{
		if (Get(client) != null)
		{
			SelectedClientName = client;
			SelectedClientConfig = _map[client];
		}
	}

	public static void ClearCache()
	{
		lock (_map)
		{
			SelectedClientName = null;
			SelectedClientConfig = null;
			_map.Clear();
		}
	}
}

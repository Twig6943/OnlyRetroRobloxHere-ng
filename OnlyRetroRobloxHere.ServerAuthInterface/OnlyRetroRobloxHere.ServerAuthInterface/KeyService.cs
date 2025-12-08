using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace OnlyRetroRobloxHere.ServerAuthInterface;

internal class KeyService
{
	private class KeyData
	{
		public string Key { get; set; } = "";

		public bool Infinite { get; set; }
	}

	private List<KeyData> _Keys = new List<KeyData>();

	public static KeyService Instance { get; } = new KeyService();

	private string RandomString(int length)
	{
		return new string((from _ in Enumerable.Range(1, length)
			select "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"[RandomNumberGenerator.GetInt32("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".Length)]).ToArray());
	}

	public string GenerateKey(bool infinite)
	{
		string text = "ORRHServerAuthKey-";
		text += RandomString(64);
		KeyData keyData = new KeyData();
		keyData.Key = text;
		keyData.Infinite = infinite;
		_Keys.Add(keyData);
		return text;
	}

	public bool ValidateThenInvalidateKey(string key)
	{
		lock (_Keys)
		{
			KeyData keyData = _Keys.Find((KeyData x) => x.Key == key);
			if (keyData == null)
			{
				return false;
			}
			if (!keyData.Infinite)
			{
				_Keys.Remove(keyData);
			}
			return true;
		}
	}
}

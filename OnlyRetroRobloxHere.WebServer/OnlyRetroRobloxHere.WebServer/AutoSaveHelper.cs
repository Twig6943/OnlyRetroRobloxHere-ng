using System;
using System.IO;
using System.Threading.Tasks;
using Ionic.Zlib;
using OnlyRetroRobloxHere.Common;

namespace OnlyRetroRobloxHere.WebServer;

internal static class AutoSaveHelper
{
	private static string CreateSavePathInternal(long time, int suffix)
	{
		string text = $"OnlyRetroRobloxHere-Save-{time}";
		if (suffix != 0)
		{
			text += $"-{suffix}";
		}
		return text + ".rbxl.gz";
	}

	private static string CreateSavePath()
	{
		long time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
		for (int i = 0; i <= 10; i++)
		{
			string path = CreateSavePathInternal(time, i);
			string text = Path.Combine(PathHelper.AutoSaves, path);
			if (!File.Exists(text))
			{
				return text;
			}
		}
		throw new ApplicationException("Could not generate a valid save path");
	}

	public static void Save(byte[] map, bool shouldCompress)
	{
		Directory.CreateDirectory(PathHelper.AutoSaves);
		string path = CreateSavePath();
		File.WriteAllBytes(path, shouldCompress ? GZipStream.CompressBuffer(map) : map);
	}

	public static async Task Save(Stream stream, bool shouldCompress)
	{
		using MemoryStream ms = new MemoryStream();
		await stream.CopyToAsync(ms);
		Save(ms.ToArray(), shouldCompress);
	}
}

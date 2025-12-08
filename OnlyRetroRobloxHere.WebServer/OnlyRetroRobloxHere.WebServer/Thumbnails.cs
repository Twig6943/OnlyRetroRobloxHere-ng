using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using OnlyRetroRobloxHere.Common;

namespace OnlyRetroRobloxHere.WebServer;

internal static class Thumbnails
{
	private class ThumbnailsAssetsData
	{
		[JsonPropertyName("targetId")]
		public long TargetId { get; set; }

		[JsonPropertyName("state")]
		public string? State { get; set; }

		[JsonPropertyName("imageUrl")]
		public string? ImageUrl { get; set; }
	}

	private class ThumbnailsAssets
	{
		[JsonPropertyName("data")]
		public ThumbnailsAssetsData[]? Data { get; set; }
	}

	private static List<(int, int)> _validThumbnailSize;

	private static readonly byte[] _deletedImageBytes;

	public static string GetClosestValidSize(int x, int y)
	{
		var (value, value2) = _validThumbnailSize.OrderBy(((int, int) e) => Math.Abs(e.Item1 - x) + Math.Abs(e.Item2 - y)).FirstOrDefault();
		return $"{value}x{value2}";
	}

	public static async Task<byte[]> GetThumbnail(ulong assetId, int x, int y, string format)
	{
		string closestValidSize = GetClosestValidSize(x, y);
		string content = JsonSerializer.Serialize(new[]
		{
			new
			{
				targetId = assetId,
				type = "Asset",
				size = closestValidSize,
				format = format,
				isCircular = false
			}
		});
		StringContent bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
		for (int i = 1; i <= 3; i++)
		{
			try
			{
				HttpResponseMessage httpResponseMessage = await Common.HttpClient.PostAsync("https://thumbnails.roblox.com/v1/batch", bodyContent);
				if (!httpResponseMessage.IsSuccessStatusCode)
				{
					if (httpResponseMessage.StatusCode != HttpStatusCode.TooManyRequests)
					{
						break;
					}
					await Task.Delay(500);
					continue;
				}
				ThumbnailsAssets thumbnailsAssets = JsonSerializer.Deserialize<ThumbnailsAssets>(await httpResponseMessage.Content.ReadAsStringAsync());
				object obj;
				if (thumbnailsAssets == null)
				{
					obj = null;
				}
				else
				{
					ThumbnailsAssetsData[]? data = thumbnailsAssets.Data;
					obj = ((data != null) ? data[0] : null);
				}
				ThumbnailsAssetsData thumbnailsAssetsData = (ThumbnailsAssetsData)obj;
				string requestUri = thumbnailsAssetsData?.ImageUrl;
				string text = thumbnailsAssetsData?.State;
				if (text != "Completed")
				{
					continue;
				}
				return await Common.HttpClient.GetByteArrayAsync(requestUri);
			}
			catch
			{
			}
		}
		return _deletedImageBytes;
	}

	static Thumbnails()
	{
		_validThumbnailSize = new List<(int, int)>
		{
			(30, 30),
			(42, 42),
			(50, 50),
			(75, 75),
			(110, 110),
			(140, 140),
			(150, 150),
			(160, 100),
			(160, 600),
			(250, 250),
			(256, 144),
			(300, 250),
			(384, 216),
			(420, 420),
			(480, 270),
			(512, 512),
			(576, 324),
			(700, 700),
			(728, 90),
			(768, 432)
		};
		string path = Path.Combine(PathHelper.ThumbnailsDeprecated, "deleted.png");
		_deletedImageBytes = File.ReadAllBytes(path);
	}
}

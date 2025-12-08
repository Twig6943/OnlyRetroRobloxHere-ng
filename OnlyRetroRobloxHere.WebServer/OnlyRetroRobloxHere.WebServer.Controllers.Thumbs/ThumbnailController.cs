using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using OnlyRetroRobloxHere.Common;
using OnlyRetroRobloxHere.WebServer.Enums;

namespace OnlyRetroRobloxHere.WebServer.Controllers.Thumbs;

public class ThumbnailController : ControllerBase
{
	private const int MinWidth = 1;

	private const int MinHeight = 1;

	private const int MaxWidth = 1000;

	private const int MaxHeight = 1000;

	private static SemaphoreSlim _semaphore = new SemaphoreSlim(20, 20);

	protected virtual bool PreserveHeight => true;

	private byte[] ResizeThumbnailInternal(byte[] imageBytes, int width, int height, ThumbnailFormat format)
	{
		using MemoryStream stream = new MemoryStream(imageBytes);
		using Image image = Image.FromStream(stream);
		float num = (float)width / (float)image.Width;
		float num2 = (float)height / (float)image.Height;
		float num3 = ((!PreserveHeight) ? ((num2 < num) ? num2 : num) : num2);
		int num4 = (int)((float)image.Width * num3);
		int num5 = (int)((float)image.Height * num3);
		int x = 0;
		int y = 0;
		if (num2 < num || PreserveHeight)
		{
			x = (width - num4) / 2;
		}
		else if (!PreserveHeight)
		{
			y = (height - num5) / 2;
		}
		using Image image2 = new Bitmap(width, height);
		using (Graphics graphics = Graphics.FromImage(image2))
		{
			if (format == ThumbnailFormat.Jpeg)
			{
				graphics.Clear(Color.White);
			}
			graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			graphics.DrawImage(image, x, y, num4, num5);
		}
		using MemoryStream memoryStream = new MemoryStream();
		image2.Save(memoryStream, (format == ThumbnailFormat.Png) ? ImageFormat.Png : ImageFormat.Jpeg);
		memoryStream.Position = 0L;
		return memoryStream.ToArray();
	}

	protected bool ResizeThumbnail(byte[] imageBytes, int width, int height, ThumbnailFormat format, out byte[] resizedThumbnail)
	{
		_semaphore.Wait();
		try
		{
			resizedThumbnail = ResizeThumbnailInternal(imageBytes, width, height, format);
			return true;
		}
		catch (Exception value)
		{
			Logger.Instance.Warn($"Failed to resize thumbnail: {value}");
			resizedThumbnail = null;
			return false;
		}
		finally
		{
			_semaphore.Release();
		}
	}

	protected IActionResult Thumbnail(byte[] imageBytes, int width, int height, ThumbnailFormat format)
	{
		if (ResizeThumbnail(imageBytes, width, height, format, out byte[] resizedThumbnail))
		{
			string contentType = ((format == ThumbnailFormat.Png) ? "image/png" : "image/jpeg");
			return File(resizedThumbnail, contentType);
		}
		return File(imageBytes, "image/png");
	}

	protected static bool IsValidSize(int width, int height)
	{
		if (width < 1 || width > 1000)
		{
			return false;
		}
		if (height < 1 || height > 1000)
		{
			return false;
		}
		return true;
	}
}

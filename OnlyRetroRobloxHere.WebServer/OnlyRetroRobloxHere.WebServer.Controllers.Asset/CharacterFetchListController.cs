using System.Collections.Generic;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlyRetroRobloxHere.Common.Enums;
using OnlyRetroRobloxHere.WebServer.Models;

namespace OnlyRetroRobloxHere.WebServer.Controllers.Asset;

[ApiController]
[Route("Asset/CharacterFetchList.ashx")]
public class CharacterFetchListController : ControllerBase
{
	private readonly ILogger<CharacterFetchListController> _logger;

	public CharacterFetchListController(ILogger<CharacterFetchListController> logger)
	{
		_logger = logger;
	}

	[HttpGet]
	public IActionResult Handle([FromQuery] string? items, [FromQuery] string? colors)
	{
		if (items == null)
		{
			items = "";
		}
		if (colors == null)
		{
			colors = "";
		}
		if (Config.Instance.Client.CharacterCompatibility.FigureBodyColours)
		{
			return Redirect("/Asset/CharacterFetchFigure.ashx?figureType=1");
		}
		string text = "";
		List<ulong> list = new List<ulong>();
		string[] array = items.Split(',');
		if (Config.Instance.User.Launch.HackCustomHats == true)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (ulong.TryParse(array[i], out var result))
				{
					list.Add(result);
				}
			}
			foreach (ulong item in AvatarAuthoriser.FilterUnsafeAssets(list))
			{
				bool flag = false;
				AvatarItem byId = AvatarItems.GetById(item);
				if (byId != null)
				{
					_ = byId.AssetVersion;
					if (byId.Type == AvatarAssetType.Gear)
					{
						flag = true;
					}
				}
				text += $"http://www.roblox.com/asset/?id={item}{(flag ? "&equipped=1" : "")};";
			}
		}
		else
		{
			foreach (string s in array)
			{
				if (ulong.TryParse(s, out var result))
				{
					list.Add(result);
				}
			}
			IEnumerable<ulong> enumerable = AvatarAuthoriser.FilterUnsafeAssets(list);
			foreach (ulong item in enumerable)
			{
				bool flag = false;
				AvatarItem byId = AvatarItems.GetById(item);
				int value;
				if (byId != null)
				{
					value = byId.AssetVersion;
					if (byId.Type == AvatarAssetType.Gear)
					{
						flag = true;
					}
				}
				else
				{
					value = 0;
				}
				text += $"http://www.roblox.com/asset/?id={item}&version={value}{(flag ? "&equipped=1" : "")};";
			}
		}
		if (!string.IsNullOrEmpty(colors))
		{
			text = text + "http://www.roblox.com/asset/bodycolorslist.ashx?colors=" + HttpUtility.UrlEncode(colors);
		}
		return Ok(text);
	}
}

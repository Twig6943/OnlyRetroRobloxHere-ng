using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.Json;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlyRetroRobloxHere.Common;
using OnlyRetroRobloxHere.WebServer.Models;

namespace OnlyRetroRobloxHere.WebServer.Controllers.Asset;

[ApiController]
[Route("Asset/CharacterFetch.ashx")]
public class CharacterFetchController : ControllerBase
{
	private readonly ILogger<CharacterFetchController> _logger;

	public CharacterFetchController(ILogger<CharacterFetchController> logger)
	{
		_logger = logger;
	}

	[HttpGet]
	public IActionResult Handle([FromQuery][Required] ulong userId)
	{
		if (!Directory.Exists(PathHelper.CharacterFetch))
		{
			return Ok();
		}
		string path = Path.Combine(PathHelper.CharacterFetch, $"{userId}.json");
		if (!System.IO.File.Exists(path))
		{
			return Ok();
		}
		if (Config.Instance.User.Launch.HackCustomHats == true)
		{
			CharacterFetchData? obj = JsonSerializer.Deserialize<CharacterFetchData>(System.IO.File.ReadAllText(path)) ?? throw new Exception($"Failed to deserialise CharacterFetchData for {userId}");
			BodyColorData bodyColors = obj.BodyColors;
			string text = "";
			string text2 = text;
			string format = "http://www.roblox.com/asset/bodycolorslist.ashx?colors={0}";
			text = text2 + string.Format(format, HttpUtility.UrlEncode($"{bodyColors.Head},{bodyColors.LeftArm},{bodyColors.RightArm},{bodyColors.LeftLeg},{bodyColors.RightLeg},{bodyColors.Torso}"));
			foreach (ulong asset in obj.Assets)
			{
				_ = AvatarItems.GetById(asset)?.AssetVersion;
				text += $";http://www.roblox.com/asset/?id={asset}";
			}
            return Ok(text);
        }
		else
		{
			string json = System.IO.File.ReadAllText(path);
			CharacterFetchData characterFetchData = JsonSerializer.Deserialize<CharacterFetchData>(json) ?? throw new Exception($"Failed to deserialise CharacterFetchData for {userId}");
			BodyColorData bodyColors = characterFetchData.BodyColors;
			string text = "";
			text += $"http://www.roblox.com/asset/bodycolorslist.ashx?colors={HttpUtility.UrlEncode($"{bodyColors.Head},{bodyColors.LeftArm},{bodyColors.RightArm},{bodyColors.LeftLeg},{bodyColors.RightLeg},{bodyColors.Torso}")}";
			foreach (ulong asset in characterFetchData.Assets)
			{
				int value = AvatarItems.GetById(asset)?.AssetVersion ?? 0;
				text += $";http://www.roblox.com/asset/?id={asset}&version={value}";
			}
			return Ok(text);
		}
	}
}

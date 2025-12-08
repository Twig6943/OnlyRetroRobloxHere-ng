using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlyRetroRobloxHere.Common;
using OnlyRetroRobloxHere.Common.Enums;
using OnlyRetroRobloxHere.WebServer.Models;

namespace OnlyRetroRobloxHere.WebServer.Controllers.Game;

[ApiController]
[Route("Game")]
public class ScriptsController : ControllerBase
{
	private readonly ILogger<ScriptsController> _logger;

	public ScriptsController(ILogger<ScriptsController> logger)
	{
		_logger = logger;
	}

	private static string GetScriptFromPath(string path)
	{
		if (!System.IO.File.Exists(path))
		{
			return "";
		}
		return System.IO.File.ReadAllText(path);
	}

	[HttpGet("Join.ashx")]
	public IActionResult Join([FromQuery] int userId = 0, [FromQuery] string? userName = "Player", [FromQuery] string? serverIp = "localhost", [FromQuery] ushort serverPort = 53640, [FromQuery] bool testMode = true, [FromQuery] ChatStyle chatStyle = ChatStyle.Classic)
	{
		if (userName == null)
		{
			userName = "Player";
		}
		if (serverIp == null)
		{
			serverIp = "localhost";
		}
		string scriptFromPath = GetScriptFromPath(ClientPaths.JoinScript);
		scriptFromPath = scriptFromPath.Replace("{userid}", userId.ToString()).Replace("{username}", userName).Replace("{serverip}", serverIp)
			.Replace("{serverport}", serverPort.ToString())
			.Replace("{testmode}", testMode.ToString().ToLowerInvariant())
			.Replace("{chatstyle}", chatStyle.ToString())
			.Replace("{charapp}", UrlConstructor.GetCharacterAppearanceUrl())
			.Replace("{membershiptype}", Config.Instance.User.Player.Membership.ToString());
		return Ok(ScriptSigner.Sign(scriptFromPath, 0uL));
	}

	[HttpGet("Studio.ashx")]
	public IActionResult Studio()
	{
		string scriptFromPath = GetScriptFromPath(ClientPaths.StudioScript);
		return Ok(ScriptSigner.Sign(scriptFromPath, 0uL));
	}

	[HttpGet("GameServer.ashx")]
	public IActionResult GameServer()
	{
		string scriptFromPath = GetScriptFromPath(ClientPaths.GameServerScript);
		return Ok(ScriptSigner.Sign(scriptFromPath, 0uL));
	}

	[HttpGet("Host.ashx")]
	public IActionResult Host(ushort serverPort = 53640)
	{
		string scriptFromPath = GetScriptFromPath(ClientPaths.HostScript);
		scriptFromPath = scriptFromPath.Replace("{port}", serverPort.ToString());
		return Ok(ScriptSigner.Sign(scriptFromPath, 0uL));
	}

	[HttpGet("Visit.ashx")]
	public IActionResult Visit()
	{
		string scriptFromPath = GetScriptFromPath(ClientPaths.VisitScript);
		scriptFromPath = scriptFromPath.Replace("{userid}", 0.ToString()).Replace("{username}", "Player").Replace("{charapp}", UrlConstructor.GetCharacterAppearanceUrl())
			.Replace("{membershiptype}", Config.Instance.User.Player.Membership.ToString());
		return Ok(ScriptSigner.Sign(scriptFromPath, 0uL));
	}

	[HttpGet("LoadPlaceInfo.ashx")]
	public IActionResult LoadPlaceInfo()
	{
		string scriptFromPath = GetScriptFromPath(ClientPaths.LoadPlaceInfoScript);
		return Ok(ScriptSigner.Sign(scriptFromPath, 0uL));
	}

	[HttpGet("PlaceSpecificScript.ashx")]
	public IActionResult PlaceSpecificScript([FromQuery] int placeId)
	{
		string scriptFromPath = GetScriptFromPath(ClientPaths.PlaceSpecificScript);
		scriptFromPath = scriptFromPath.Replace("{id}", placeId.ToString());
		return Ok(ScriptSigner.Sign(scriptFromPath, 0uL));
	}

	[HttpGet("AutoSave.ashx")]
	public IActionResult AutoSave()
	{
		string scriptFromPath = GetScriptFromPath(ClientPaths.AutoSaveScript);
		scriptFromPath = scriptFromPath.Replace("{enabled}", Config.Instance.User.Launch.AutoSaveEnabled.ToString().ToLowerInvariant()).Replace("{changesperplayer}", Config.Instance.User.Launch.AutoSaveChangesPerPlayer.ToString()).Replace("{savecheckinterval}", Config.Instance.User.Launch.AutoSaveCheckInterval.ToString())
			.Replace("{savecooldown}", Config.Instance.User.Launch.AutoSaveCooldown.ToString())
			.Replace("{debug}", Config.Instance.User.Launch.AutoSaveDebug.ToString().ToLowerInvariant());
		return Ok(ScriptSigner.Sign(scriptFromPath, 0uL));
	}

	[HttpGet("EggHuntScript.ashx")]
	public IActionResult EggHunt()
	{
		string scriptFromPath = GetScriptFromPath(ClientPaths.EggHuntScript);
		scriptFromPath = scriptFromPath.Replace("{enabled}", Config.Instance.User.Launch.EggHuntMode.ToString().ToLowerInvariant());
		return Ok(ScriptSigner.Sign(scriptFromPath, 0uL));
	}

	[HttpGet("RenderCharacter.ashx")]
	public IActionResult RenderCharacter([FromQuery] string? charapp = null, [FromQuery] string? outputDir = null)
	{
		if (charapp == null)
		{
			charapp = "";
		}
		if (outputDir == null)
		{
			outputDir = "";
		}
		if (!Config.Instance.IsRenderMode)
		{
			return Forbid();
		}
		string scriptFromPath = GetScriptFromPath(Path.Combine(PathHelper.SharedScripts, "render_character.lua"));
		scriptFromPath = scriptFromPath.Replace("{charapp}", charapp).Replace("{out}", outputDir.Replace("\\", "\\\\"));
		return Ok(ScriptSigner.Sign(scriptFromPath, 0uL));
	}

	[HttpGet("RenderHat.ashx")]
	public IActionResult RenderHat([FromQuery] string? url = null, [FromQuery] string? outputDir = null)
	{
		if (url == null)
		{
			url = "";
		}
		if (outputDir == null)
		{
			outputDir = "";
		}
		if (!Config.Instance.IsRenderMode)
		{
			return Forbid();
		}
		string scriptFromPath = GetScriptFromPath(Path.Combine(PathHelper.SharedScripts, "render_hat.lua"));
		scriptFromPath = scriptFromPath.Replace("{url}", url).Replace("{out}", outputDir.Replace("\\", "\\\\"));
		return Ok(ScriptSigner.Sign(scriptFromPath, 0uL));
	}

	[HttpGet("RenderClothes.ashx")]
	public IActionResult RenderClothes([FromQuery] string? url = null, [FromQuery] string? outputDir = null)
	{
		if (url == null)
		{
			url = "";
		}
		if (outputDir == null)
		{
			outputDir = "";
		}
		if (!Config.Instance.IsRenderMode)
		{
			return Forbid();
		}
		string scriptFromPath = GetScriptFromPath(Path.Combine(PathHelper.SharedScripts, "render_clothes.lua"));
		scriptFromPath = scriptFromPath.Replace("{url}", url).Replace("{out}", outputDir.Replace("\\", "\\\\"));
		return Ok(ScriptSigner.Sign(scriptFromPath, 0uL));
	}

	[HttpGet("RenderHead.ashx")]
	public IActionResult RenderHead([FromQuery] string? url = null, [FromQuery] string? outputDir = null)
	{
		if (url == null)
		{
			url = "";
		}
		if (outputDir == null)
		{
			outputDir = "";
		}
		if (!Config.Instance.IsRenderMode)
		{
			return Forbid();
		}
		string scriptFromPath = GetScriptFromPath(Path.Combine(PathHelper.SharedScripts, "render_head.lua"));
		scriptFromPath = scriptFromPath.Replace("{url}", url).Replace("{out}", outputDir.Replace("\\", "\\\\"));
		return Ok(ScriptSigner.Sign(scriptFromPath, 0uL));
	}

	[HttpGet("RenderBodyPart.ashx")]
	public IActionResult RenderBodyPart([FromQuery] string? url = null, [FromQuery] string? outputDir = null)
	{
		if (url == null)
		{
			url = "";
		}
		if (outputDir == null)
		{
			outputDir = "";
		}
		if (!Config.Instance.IsRenderMode)
		{
			return Forbid();
		}
		string scriptFromPath = GetScriptFromPath(Path.Combine(PathHelper.SharedScripts, "render_bodypart.lua"));
		scriptFromPath = scriptFromPath.Replace("{url}", url).Replace("{out}", outputDir.Replace("\\", "\\\\"));
		return Ok(ScriptSigner.Sign(scriptFromPath, 0uL));
	}

	[HttpGet("RenderGear.ashx")]
	public IActionResult RenderGear([FromQuery] string? url = null, [FromQuery] string? outputDir = null)
	{
		if (url == null)
		{
			url = "";
		}
		if (outputDir == null)
		{
			outputDir = "";
		}
		if (!Config.Instance.IsRenderMode)
		{
			return Forbid();
		}
		string scriptFromPath = GetScriptFromPath(Path.Combine(PathHelper.SharedScripts, "render_gear.lua"));
		scriptFromPath = scriptFromPath.Replace("{url}", url).Replace("{out}", outputDir.Replace("\\", "\\\\"));
		return Ok(ScriptSigner.Sign(scriptFromPath, 0uL));
	}

	[HttpGet("RenderModel.ashx")]
	public IActionResult RenderModel([FromQuery] string? url = null, [FromQuery] string? outputDir = null)
	{
		if (url == null)
		{
			url = "";
		}
		if (outputDir == null)
		{
			outputDir = "";
		}
		if (!Config.Instance.IsRenderMode)
		{
			return Forbid();
		}
		string scriptFromPath = GetScriptFromPath(Path.Combine(PathHelper.SharedScripts, "render_model.lua"));
		scriptFromPath = scriptFromPath.Replace("{url}", url).Replace("{out}", outputDir.Replace("\\", "\\\\"));
		return Ok(ScriptSigner.Sign(scriptFromPath, 0uL));
	}

	[HttpGet("RenderPackage.ashx")]
	public IActionResult RenderPackage([FromQuery] ulong id = 0uL, [FromQuery] string? outputDir = null)
	{
		if (outputDir == null)
		{
			outputDir = "";
		}
		if (!Config.Instance.IsRenderMode)
		{
			return Forbid();
		}
		string scriptFromPath = GetScriptFromPath(Path.Combine(PathHelper.SharedScripts, "render_package.lua"));
		AvatarItem byId = AvatarItems.GetById(id);
		if (byId == null)
		{
			return BadRequest();
		}
		string text = "";
		for (int i = 0; i < byId.Items.Count; i++)
		{
			AvatarItem byId2 = AvatarItems.GetById(byId.Items[i]);
			if (byId2 == null)
			{
				return BadRequest();
			}
			string text2 = ((i != 0) ? "," : "");
			text2 += $"\"http://www.roblox.com/asset/?id={byId2.Id}&version={byId2.AssetVersion}\"";
			text += text2;
		}
		scriptFromPath = scriptFromPath.Replace("{pkg}", text).Replace("{out}", outputDir.Replace("\\", "\\\\"));
		return Ok(ScriptSigner.Sign(scriptFromPath, 0uL));
	}
}

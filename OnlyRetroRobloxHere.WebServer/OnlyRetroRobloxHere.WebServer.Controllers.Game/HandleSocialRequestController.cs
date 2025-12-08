using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlyRetroRobloxHere.WebServer.Services;

namespace OnlyRetroRobloxHere.WebServer.Controllers.Game;

[ApiController]
public class HandleSocialRequestController : ControllerBase
{
	private readonly ILogger<HandleSocialRequestController> _logger;

	public HandleSocialRequestController(ILogger<HandleSocialRequestController> logger)
	{
		_logger = logger;
	}

	[HttpGet("Game/LuaWebService/HandleSocialRequest.ashx")]
	[HttpPost("Game/LuaWebService/HandleSocialRequest.ashx")]
	public IActionResult Get([FromQuery(Name = "method")] string method, [FromQuery(Name = "playerId")] int playerId = 0, [FromQuery(Name = "userId")] int userId = 0)
	{
		string content;
		switch (method)
		{
		case "IsBestFriendsWith":
		case "IsFriendsWith":
			content = "<Value Type=\"boolean\">" + (playerId == userId || FriendsService.Instance.AreFriend(playerId, userId)).ToString().ToLowerInvariant() + "</Value>";
			break;
		case "IsInGroup":
			content = "<Value Type=\"boolean\">false</Value>";
			break;
		case "GetGroupRank":
			content = "<Value Type=\"integer\">0</Value>";
			break;
		case "GetGroupRole":
			content = "Guest";
			break;
		default:
			throw new Exception("Unknown method " + method);
		}
		return Content(content, "text/html");
	}
}

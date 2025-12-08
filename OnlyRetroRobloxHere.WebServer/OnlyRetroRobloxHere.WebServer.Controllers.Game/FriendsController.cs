using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlyRetroRobloxHere.WebServer.Services;

namespace OnlyRetroRobloxHere.WebServer.Controllers.Game;

[ApiController]
public class FriendsController : ControllerBase
{
	private readonly ILogger<FriendsController> _logger;

	public FriendsController(ILogger<FriendsController> logger)
	{
		_logger = logger;
	}

	[HttpGet("Game/AreFriends")]
	[HttpGet("Friend/AreFriends")]
	public IActionResult GetAreFriends([FromQuery(Name = "userId")] int userId, [FromQuery(Name = "otherUserIds")] string otherUserIdsStr)
	{
		IEnumerable<int> users = otherUserIdsStr.Split(',').Select(int.Parse);
		IEnumerable<int> enumerable = FriendsService.Instance.AreFriends(userId, users);
		string text = "S";
		if (enumerable.Count() > 0)
		{
			text += string.Join(',', enumerable);
			text += ",";
		}
		return Content(text, "text/plain");
	}

	[HttpPost("Game/CreateFriend")]
	[HttpPost("Friend/CreateFriend")]
	[HttpGet("Game/CreateFriend")]
	[HttpGet("Friend/CreateFriend")]
	public IActionResult GetCreateFriend([FromQuery(Name = "firstUserId")] int firstUserId, [FromQuery(Name = "secondUserId")] int secondUserId)
	{
		FriendsService.Instance.CreateFriend(firstUserId, secondUserId);
		return Ok();
	}

	[HttpPost("Game/BreakFriend")]
	[HttpPost("Friend/BreakFriend")]
	[HttpGet("Game/BreakFriend")]
	[HttpGet("Friend/BreakFriend")]
	public IActionResult GetBreakFriend([FromQuery(Name = "firstUserId")] int firstUserId, [FromQuery(Name = "secondUserId")] int secondUserId)
	{
		FriendsService.Instance.BreakFriend(firstUserId, secondUserId);
		return Ok();
	}
}

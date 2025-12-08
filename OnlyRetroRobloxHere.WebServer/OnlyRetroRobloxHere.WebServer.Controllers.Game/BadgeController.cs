using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlyRetroRobloxHere.WebServer.Services;

namespace OnlyRetroRobloxHere.WebServer.Controllers.Game;

[ApiController]
[Route("Game")]
public class BadgeController : ControllerBase
{
	private readonly ILogger<BadgeController> _logger;

	public BadgeController(ILogger<BadgeController> logger)
	{
		_logger = logger;
	}

	[HttpPost("Badge/AwardBadge.ashx")]
	[HttpGet("Badge/AwardBadge.ashx")]
	[HttpGet("AwardBadge.ashx")]
	public IActionResult AwardBadge([FromQuery] int badgeId, [FromQuery] int userId)
	{
		string value = BadgeService.Instance.AwardBadge(userId, badgeId);
		return Ok(value);
	}

	[HttpPost("Badge/HasBadge.ashx")]
	[HttpGet("Badge/HasBadge.ashx")]
	[HttpGet("HasBadge.ashx")]
	public IActionResult HasBadge([FromQuery] int badgeId, [FromQuery] int userId)
	{
		bool flag = BadgeService.Instance.HasBadge(userId, badgeId);
		return Ok(flag ? "Success" : "Failure");
	}

	[HttpPost("Badge/IsBadgeDisabled.ashx")]
	[HttpGet("Badge/IsBadgeDisabled.ashx")]
	[HttpGet("IsBadgeDisabled.ashx")]
	public IActionResult IsDisabled()
	{
		return Ok("0");
	}

	[HttpGet("IsBadgeLegal.ashx")]
	public IActionResult IsLegal()
	{
		return Ok("1");
	}
}

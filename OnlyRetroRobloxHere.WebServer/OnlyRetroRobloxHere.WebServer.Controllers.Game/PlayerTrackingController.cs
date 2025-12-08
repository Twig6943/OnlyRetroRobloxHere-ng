using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlyRetroRobloxHere.WebServer.Services;

namespace OnlyRetroRobloxHere.WebServer.Controllers.Game;

[ApiController]
[Route("Game/PlayerTracking.ashx")]
public class PlayerTrackingController : ControllerBase
{
	private readonly ILogger<PlayerTrackingController> _logger;

	public PlayerTrackingController(ILogger<PlayerTrackingController> logger)
	{
		_logger = logger;
	}

	[HttpGet]
	public IActionResult Handle([FromQuery(Name = "m")] string mode, [FromQuery(Name = "i")] int id, [FromQuery(Name = "n")] string? name = null)
	{
		base.Response.Headers.CacheControl = "no-cache";
		if (!(mode == "u"))
		{
			if (!(mode == "r"))
			{
				return BadRequest("Unknown mode");
			}
			if (string.IsNullOrEmpty(name))
			{
				return BadRequest("No name provided");
			}
			PlayerTrackingService.Default.RegisterPlayer(id, name);
		}
		else
		{
			PlayerTrackingService.Default.UnregisterPlayer(id);
		}
		return Ok();
	}
}

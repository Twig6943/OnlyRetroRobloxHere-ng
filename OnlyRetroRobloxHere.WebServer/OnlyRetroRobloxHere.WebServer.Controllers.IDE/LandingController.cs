using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace OnlyRetroRobloxHere.WebServer.Controllers.IDE;

[ApiController]
[Route("IDE/Landing.aspx")]
[Route("My/Places.aspx")]
public class LandingController : ControllerBase
{
	private readonly ILogger<LandingController> _logger;

	public LandingController(ILogger<LandingController> logger)
	{
		_logger = logger;
	}

	[HttpGet]
	public IActionResult Get()
	{
		return Content("<head><title>Only Retro Roblox Here</title></head><html><body><marquee>Only Retro Roblox Here</marquee></body></html>", "text/html");
	}
}

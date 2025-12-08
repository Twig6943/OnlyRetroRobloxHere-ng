using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace OnlyRetroRobloxHere.WebServer.Controllers.ClientSettings;

[ApiController]
[Route("Setting/QuietGet/{setting}")]
public class QuietGetController : ControllerBase
{
	private readonly ILogger<QuietGetController> _logger;

	public QuietGetController(ILogger<QuietGetController> logger)
	{
		_logger = logger;
	}

	[HttpGet]
	public IActionResult Get(string setting)
	{
		string content = ((!System.IO.File.Exists(ClientPaths.Flags)) ? "{}" : System.IO.File.ReadAllText(ClientPaths.Flags));
		return Content(content, "application/json");
	}
}

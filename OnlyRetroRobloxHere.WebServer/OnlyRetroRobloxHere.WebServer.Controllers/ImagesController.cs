using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlyRetroRobloxHere.Common;

namespace OnlyRetroRobloxHere.WebServer.Controllers;

[ApiController]
[Route("Images")]
public class ImagesController : ControllerBase
{
	private readonly ILogger<ImagesController> _logger;

	public ImagesController(ILogger<ImagesController> logger)
	{
		_logger = logger;
	}

	[HttpGet("{*path}")]
	public IActionResult Get(string path)
	{
		string path2 = Path.Combine(PathHelper.Data, "wwwimgs", path);
		if (System.IO.File.Exists(path2))
		{
			return File(System.IO.File.ReadAllBytes(path2), "image/png");
		}
		return NotFound();
	}
}

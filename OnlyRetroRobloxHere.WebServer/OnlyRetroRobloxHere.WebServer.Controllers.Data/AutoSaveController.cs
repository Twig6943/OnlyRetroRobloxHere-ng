using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace OnlyRetroRobloxHere.WebServer.Controllers.Data;

[ApiController]
[Route("Data/AutoSave.ashx")]
public class AutoSaveController : ControllerBase
{
	private readonly ILogger<AutoSaveController> _logger;

	public AutoSaveController(ILogger<AutoSaveController> logger)
	{
		_logger = logger;
	}

	[HttpPost]
	public async Task<IActionResult> Handle()
	{
		await AutoSaveHelper.Save(base.Request.Body, shouldCompress: false);
		return Ok();
	}
}

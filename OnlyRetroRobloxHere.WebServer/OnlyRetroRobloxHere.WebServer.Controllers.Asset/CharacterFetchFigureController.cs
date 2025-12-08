using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlyRetroRobloxHere.Common.Enums;

namespace OnlyRetroRobloxHere.WebServer.Controllers.Asset;

[ApiController]
[Route("Asset/CharacterFetchFigure.ashx")]
public class CharacterFetchFigureController : ControllerBase
{
	private readonly ILogger<CharacterFetchFigureController> _logger;

	public CharacterFetchFigureController(ILogger<CharacterFetchFigureController> logger)
	{
		_logger = logger;
	}

	[HttpGet]
	public IActionResult Handle([FromQuery] FigureCharacterType figureType)
	{
		return Ok($"http://www.roblox.com/asset/bodycolorsfigure.ashx?figureType={figureType}");
	}
}

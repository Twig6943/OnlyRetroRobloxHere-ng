using System.ComponentModel.DataAnnotations;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlyRetroRobloxHere.Common;
using OnlyRetroRobloxHere.WebServer.Enums;

namespace OnlyRetroRobloxHere.WebServer.Controllers.Thumbs;

[ApiController]
public class AvatarController : ThumbnailController
{
	private static readonly string _renderPath = Path.Combine(PathHelper.UserAppData, "render.png");

	private static readonly string _defaultPath = Path.Combine(PathHelper.ThumbnailsDeprecated, "avatar.png");

	private static byte[]? _renderBytes = null;

	private static byte[]? _defaultBytes = null;

	private readonly ILogger<AvatarController> _logger;

	public AvatarController(ILogger<AvatarController> logger)
	{
		_logger = logger;
	}

	[HttpGet]
	[Route("Thumbs/Avatar.ashx")]
	public IActionResult GetAvatar([FromQuery(Name = "x")][Required] int width, [FromQuery(Name = "y")][Required] int height, [FromQuery(Name = "format")] ThumbnailFormat format = ThumbnailFormat.Png, [FromQuery(Name = "userid")] ulong? userId = null, [FromQuery(Name = "username")] string? userName = null)
	{
		if (!ThumbnailController.IsValidSize(width, height))
		{
			return BadRequest();
		}
		if (((userId is ulong && userId.Value == (ulong)Config.Instance.User.Player.Id) || userName?.ToLowerInvariant() == Config.Instance.User.Player.Name.ToLowerInvariant()) && System.IO.File.Exists(_renderPath))
		{
			if (_renderBytes == null)
			{
				_renderBytes = System.IO.File.ReadAllBytes(_renderPath);
			}
			return Thumbnail(_renderBytes, width, height, format);
		}
		if (_defaultBytes == null)
		{
			_defaultBytes = System.IO.File.ReadAllBytes(_defaultPath);
		}
		return Thumbnail(_defaultBytes, width, height, format);
	}
}

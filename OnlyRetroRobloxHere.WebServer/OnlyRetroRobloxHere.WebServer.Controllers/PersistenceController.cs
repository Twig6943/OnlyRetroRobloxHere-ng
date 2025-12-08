using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlyRetroRobloxHere.WebServer.Services;

namespace OnlyRetroRobloxHere.WebServer.Controllers;

[ApiController]
[Route("Persistence")]
public class PersistenceController : ControllerBase
{
	private readonly ILogger<PersistenceController> _logger;

	public PersistenceController(ILogger<PersistenceController> logger)
	{
		_logger = logger;
	}

	[HttpGet("GetBlobUrl.ashx")]
	public IActionResult GetBlob(int userId)
	{
		base.Response.Headers.CacheControl = "no-cache";
		byte[] blob = DataPersistenceService.Instance.GetBlob(userId);
		base.Response.Headers["Content-Encoding"] = "gzip";
		return File(blob, "application/octet-stream");
	}

	[HttpPost("SetBlob.ashx")]
	public async Task<IActionResult> SetBlob(int userId)
	{
		base.Response.Headers.CacheControl = "no-cache";
		byte[] blob;
		using (MemoryStream ms = new MemoryStream())
		{
			await base.Request.Body.CopyToAsync(ms);
			blob = ms.ToArray();
		}
		bool alreadyCompressed = base.Request.Headers.AcceptEncoding.Contains("gzip");
		DataPersistenceService.Instance.SaveBlob(userId, blob, alreadyCompressed);
		return Ok();
	}
}

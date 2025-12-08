using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using OnlyRetroRobloxHere.Common;

namespace OnlyRetroRobloxHere.ServerAuthInterface.Web;

internal static class Server
{
	private static object V1AuthHandler(HttpContext context, string key)
	{
		IPAddress iPAddress = context.Connection.RemoteIpAddress.MapToIPv4();
		if (AuthService.Instance.IsIPAuthorised(iPAddress))
		{
			Logger.Instance.Warn($"{iPAddress} - Authorized IP tried to use key \"{key}\"");
			return true;
		}
		if (KeyService.Instance.ValidateThenInvalidateKey(key))
		{
			Logger.Instance.Info($"{iPAddress} - Authorized using key \"{key}\"");
			AuthService.Instance.AuthoriseIP(iPAddress);
			return true;
		}
		return false;
	}

	private static object V1IsAuthHandler(HttpContext context)
	{
		return AuthService.Instance.IsIPAuthorised(context.Connection.RemoteIpAddress);
	}

	public static async Task Start()
	{
		WebApplicationBuilder webApplicationBuilder = WebApplication.CreateBuilder();
		webApplicationBuilder.WebHost.ConfigureKestrel(delegate(KestrelServerOptions k)
		{
			k.ListenAnyIP(Config.Default.WebServerPort);
		});
		WebApplication webApplication = webApplicationBuilder.Build();
		webApplication.UseRouting();
		webApplication.MapGet("/v1/auth/{key}", new Func<HttpContext, string, object>(V1AuthHandler));
		webApplication.MapGet("/v1/is-auth", new Func<HttpContext, object>(V1IsAuthHandler));
		await webApplication.RunAsync();
	}
}

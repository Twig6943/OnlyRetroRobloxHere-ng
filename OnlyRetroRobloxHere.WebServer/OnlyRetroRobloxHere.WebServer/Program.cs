using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.DependencyInjection;
using OnlyRetroRobloxHere.Common;

namespace OnlyRetroRobloxHere.WebServer;

public class Program
{
	private static void FixUrl(RewriteContext context)
	{
		HttpRequest request = context.HttpContext.Request;
		if (request.Path.Value != null && request.Path.Value.StartsWith("//"))
		{
			string value = request.Path.Value;
			request.Path = new PathString(value.Substring(1, value.Length - 1));
		}
	}

	private static async Task OnlyAllowLocal(HttpContext context, RequestDelegate next)
	{
		if (context.Connection.RemoteIpAddress == null || !IPAddress.IsLoopback(context.Connection.RemoteIpAddress))
		{
			context.Response.StatusCode = 401;
		}
		else
		{
			await next(context);
		}
	}

	private static void WatchProcessId(int processId)
	{
		try
		{
			using Process process = Process.GetProcessById(processId);
			process.WaitForExit();
		}
		catch (ArgumentException)
		{
		}
		Environment.Exit(123);
	}

	public static void Main(string[] args)
	{
		AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler.Handle;
		string text = null;
		int processId = 0;
		bool isRenderMode = false;
		foreach (string text2 in args)
		{
			if (text2.StartsWith("--client="))
			{
				string text3 = text2;
				text = text3.Substring(9, text3.Length - 9).Trim();
			}
			else if (text2.StartsWith("--pid="))
			{
				string text3 = text2;
				string text4 = text3.Substring(6, text3.Length - 6).Trim();
				if (!int.TryParse(text4, out processId))
				{
					Logger.Instance.Warn("Invalid process ID provided: " + text4);
				}
			}
			else if (text2 == "--rm")
			{
				isRenderMode = true;
			}
		}
		if (text == null)
		{
			Logger.Instance.Error("Failed to initialise config");
			return;
		}
		Logger.Instance.Info("Client: " + text);
		Config.Init(text);
		Config.Instance.IsRenderMode = isRenderMode;
		_ = Common.AssetPackDirectories;
		_ = LocalAssetHelper.Instance;
		WebApplicationBuilder webApplicationBuilder = WebApplication.CreateBuilder(args);
		webApplicationBuilder.Services.AddControllers();
		WebApplication webApplication = webApplicationBuilder.Build();
		webApplication.Use(OnlyAllowLocal);
		webApplication.UseRewriter(new RewriteOptions().Add(FixUrl));
		webApplication.MapControllers();
		if (processId != 0)
		{
			Thread thread = new Thread((ThreadStart)delegate
			{
				WatchProcessId(processId);
			});
			thread.IsBackground = true;
			thread.Start();
		}
		webApplication.Run();
	}
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using OnlyRetroRobloxHere.Common;
using OnlyRetroRobloxHere.ServerAuthInterface.Proxy;
using OnlyRetroRobloxHere.ServerAuthInterface.Web;

namespace OnlyRetroRobloxHere.ServerAuthInterface;

internal class Program
{
	private static Process? _RobloxProcess;

	private static void Main(string[] args)
	{
		AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
		AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
		Config.Load(args);
		Logger.Instance.Info($"Only Retro Roblox Here Server Authorization Interface {Assembly.GetExecutingAssembly().GetName().Version}");
		Logger.Instance.Info($"Base port: {Config.Default.BasePort}");
		List<Task> list = new List<Task>();
		CheckProcessId();
		Logger.Instance.Info("Starting public Roblox proxy");
		string localIP = Utils.GetLocalIP();
		OnlyRetroRobloxHere.ServerAuthInterface.Proxy.Server server = new OnlyRetroRobloxHere.ServerAuthInterface.Proxy.Server();
		list.Add(server.Start(localIP, Config.Default.RobloxPort, Config.Default.ProxyPort));
		Logger.Instance.Info("Starting web server");
		list.Add(OnlyRetroRobloxHere.ServerAuthInterface.Web.Server.Start());
		Logger.Instance.Info($"Public Roblox proxy started on {Config.Default.ProxyPort} (UDP)");
		Logger.Instance.Info($"Web server started on {Config.Default.WebServerPort} (TCP)");
		Logger.Instance.Info($"Private Roblox server started on {Config.Default.RobloxPort} (UDP) (keep this port closed)");
		list.Add(Task.Run(delegate
		{
			InputMonitor.Start();
		}));
		InputMonitor.DisplayHelpMenu();
		Task.WaitAny(list.ToArray());
		Logger.Instance.Info("A task closed, exiting!");
	}

	private static void CheckProcessId()
	{
		if (Config.Default.ClientProcessId == -1)
		{
			Logger.Instance.Warn("Client process id is not defined, disabling process closing checks.");
			return;
		}
		try
		{
			_RobloxProcess = Process.GetProcessById(Config.Default.ClientProcessId);
		}
		catch (Exception value)
		{
			Logger.Instance.Error($"Failed to find process by id {Config.Default.ClientProcessId}: {value}");
			Logger.Instance.Warn("Closing!");
			Environment.Exit(1001);
			return;
		}
		Task.Run((Action)ClientProcessClosureCheck);
	}

	private static void ClientProcessClosureCheck()
	{
		Logger.Instance.Info("Starting client process closure check");
		_RobloxProcess.WaitForExit();
		Logger.Instance.Warn("Client process closed, exiting!");
		Environment.Exit(1002);
	}

	private static void OnProcessExit(object? sender, EventArgs e)
	{
		Logger.Instance.Info("Got process exit event.");
		if (_RobloxProcess != null)
		{
			Logger.Instance.Warn("Closing Roblox client");
			_RobloxProcess.CloseMainWindow();
			_RobloxProcess.Close();
		}
	}

	private static void OnUnhandledException(object? sender, UnhandledExceptionEventArgs e)
	{
		Exception ex = (Exception)e.ExceptionObject;
		try
		{
			Logger.Instance.Error("Unhandled exception!");
			Logger.Instance.Error(ex.ToString());
		}
		catch
		{
		}
	}
}

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using OnlyRetroRobloxHere.Common.Enums;

namespace OnlyRetroRobloxHere.Common;

public class Logger
{
	public static readonly Logger Instance;

	private StreamWriter _Writer;

	private bool _Verbose;

	private static string ConstructLogFileName()
	{
		return (Assembly.GetEntryAssembly()?.GetName().Name ?? "Unknown") + "_" + DateTime.UtcNow.ToString("O").Replace("-", "").Replace(":", "")
			.Replace(".", "") + ".log";
	}

	private Logger(bool verbose)
	{
		string path = ConstructLogFileName();
		string path2 = Path.Combine(PathHelper.Logs, path);
		_Writer = new StreamWriter(path2);
		_Writer.AutoFlush = true;
		_Verbose = verbose;
	}

	private string ConstructLogOutput(LogType type, string message)
	{
		return $"{DateTime.UtcNow.ToString("O")} [{type}] {message}";
	}

	private void Log(LogType type, string message)
	{
		string value = ConstructLogOutput(type, message);
		Console.WriteLine(value);
		_Writer.WriteLine(value);
	}

	[Conditional("DEBUG")]
	public void Verbose(string message)
	{
		if (_Verbose)
		{
			Log(LogType.Verbose, message);
		}
	}

	[Conditional("DEBUG")]
	public void Debug(string message)
	{
		Log(LogType.Verbose, message);
	}

	public void Info(string message)
	{
		Log(LogType.Information, message);
	}

	public void Warn(string message)
	{
		Log(LogType.Warning, message);
	}

	public void Error(string message)
	{
		Log(LogType.Error, message);
	}

	private static void CleanupOldLogs()
	{
		string[] files = Directory.GetFiles(PathHelper.Logs);
		foreach (string text in files)
		{
			if (!text.EndsWith(".log"))
			{
				continue;
			}
			try
			{
				FileInfo fileInfo = new FileInfo(text);
				if (fileInfo.LastAccessTime < DateTime.Now.AddDays(-2.0))
				{
					fileInfo.Delete();
				}
			}
			catch
			{
			}
		}
	}

	static Logger()
	{
		Directory.CreateDirectory(PathHelper.Logs);
		CleanupOldLogs();
		Instance = new Logger(verbose: false);
	}
}

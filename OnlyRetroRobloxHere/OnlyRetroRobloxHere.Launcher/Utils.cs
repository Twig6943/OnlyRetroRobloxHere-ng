using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using OnlyRetroRobloxHere.Common;
using OnlyRetroRobloxHere.Launcher.Models.Attributes;

namespace OnlyRetroRobloxHere.Launcher;

internal static class Utils
{
    private static readonly char[] PathSplitDelimiters = new char[2]
    {
        Path.DirectorySeparatorChar,
        Path.AltDirectorySeparatorChar
    };

    private static string? _version = null;

    private static BuildMetadataAttribute? _buildMetadataAttribute = null;

    public static IReadOnlyCollection<string> ValidMapExtensions { get; } = (IReadOnlyCollection<string>)(object)new string[4] { ".rbxl", ".rbxlx", ".rbxl.gz", ".rbxlx.gz" };

    public static string Version => _version ?? (_version = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion);

    public static BuildMetadataAttribute BuildMetadata => _buildMetadataAttribute ?? (_buildMetadataAttribute = Assembly.GetExecutingAssembly().GetCustomAttribute<BuildMetadataAttribute>());

    public static MessageBoxResult ShowMessageBox(string message, MessageBoxButton button, MessageBoxImage image)
    {
        return MessageBox.Show(message, "Only Retro Roblox Here", button, image);
    }

    public static void ClearClientAddonsCache()
    {
        if (!Directory.Exists(PathHelper.ClientAddonsCache))
        {
            return;
        }
        string[] files = Directory.GetFiles(PathHelper.ClientAddonsCache);
        foreach (string path in files)
        {
            try
            {
                File.Delete(path);
            }
            catch
            {
            }
        }
    }

    public static bool IsUdpPortAvailable(ushort port)
    {
        return !IPGlobalProperties.GetIPGlobalProperties().GetActiveUdpListeners().Any((IPEndPoint p) => p.Port == port);
    }

    public static bool IsTcpPortAvailable(ushort port)
    {
        return !IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners().Any((IPEndPoint p) => p.Port == port);
    }

    public static bool IsPortSafeForAuthServers(ushort port)
    {
        return port < ushort.MaxValue;
    }

    public static bool IsAuthServerPortAvailable(ushort basePort)
    {
        if (!IsPortSafeForAuthServers(basePort))
        {
            return false;
        }
        ushort port = basePort;
        ushort port2 = (ushort)(basePort + 1);
        bool num = IsUdpPortAvailable(basePort);
        bool flag = IsTcpPortAvailable(port);
        bool flag2 = IsUdpPortAvailable(port2);
        return num && flag && flag2;
    }

    public static bool IsHoldingDownCtrl()
    {
        if (!Keyboard.IsKeyDown(Key.LeftCtrl))
        {
            return Keyboard.IsKeyDown(Key.RightCtrl);
        }
        return true;
    }

    public static bool IsDebug
    {
        get
        {
            #if DEBUG
            return true;
            #else
            return false;
            #endif
        }
    }

    public static string[] SplitPath(string path)
    {
        return path.Split(PathSplitDelimiters, StringSplitOptions.RemoveEmptyEntries);
    }

    public static string GetMapsDirectory()
    {
        if (string.IsNullOrWhiteSpace(Settings.Default.Launch.CustomMapsDirectory))
        {
            return PathHelper.Maps;
        }
        return Settings.Default.Launch.CustomMapsDirectory;
    }

    public static Stream GetStreamFromUri(Uri uri)
    {
        string absoluteUri = uri.AbsoluteUri;
        if (absoluteUri.StartsWith("file://"))
        {
            string text = absoluteUri;
            int num = (absoluteUri.StartsWith("file:///") ? 8 : 7);
            return File.OpenRead(HttpUtility.UrlDecode(text.Substring(num, text.Length - num)));
        }
        if (absoluteUri.StartsWith("pack://"))
        {
            return Application.GetResourceStream(uri).Stream;
        }
        return File.OpenRead(HttpUtility.UrlDecode(absoluteUri));
    }

    public static void OpenUrl(string url)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            url = url.Replace("&", "^&");
            Process.Start(new ProcessStartInfo(url)
            {
                UseShellExecute = true
            });
            return;
        }
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            Process.Start("xdg-open", url);
            return;
        }
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            Process.Start("open", url);
            return;
        }
        throw new Exception("Unhandled platform");
    }

    public static BitmapImage ToBitmapImage(Bitmap bitmap)
    {
        using MemoryStream memoryStream = new MemoryStream();
        bitmap.Save(memoryStream, ImageFormat.Png);
        memoryStream.Position = 0L;
        BitmapImage bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        bitmapImage.StreamSource = memoryStream;
        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        bitmapImage.EndInit();
        bitmapImage.Freeze();
        return bitmapImage;
    }
}

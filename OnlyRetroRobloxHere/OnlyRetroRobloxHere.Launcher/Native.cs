using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace OnlyRetroRobloxHere.Launcher;

internal class Native
{
	private delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);

	[DllImport("user32.dll")]
	private static extern bool EnumThreadWindows(int dwThreadId, EnumThreadDelegate lpfn, IntPtr lParam);

	public static IEnumerable<IntPtr> EnumerateProcessWindowHandles(int processId)
	{
		List<IntPtr> handles = new List<IntPtr>();
		foreach (ProcessThread thread in Process.GetProcessById(processId).Threads)
		{
			EnumThreadWindows(thread.Id, delegate(IntPtr hWnd, IntPtr lParam)
			{
				handles.Add(hWnd);
				return true;
			}, IntPtr.Zero);
		}
		return handles;
	}

	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern int GetWindowTextLength(IntPtr hWnd);

	public static string GetWindowTitle(IntPtr hWnd)
	{
		int num = GetWindowTextLength(hWnd) + 1;
		StringBuilder stringBuilder = new StringBuilder(num);
		GetWindowText(hWnd, stringBuilder, num);
		return stringBuilder.ToString();
	}
}

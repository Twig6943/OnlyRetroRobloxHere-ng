using System;
using System.Threading;

namespace OnlyRetroRobloxHere.Launcher;

internal class InterProcessLock : IDisposable
{
	public Mutex Mutex { get; private set; }

	public bool IsAcquired { get; private set; }

	public InterProcessLock(string name, TimeSpan timeout)
	{
		Mutex = new Mutex(initiallyOwned: false, "Global\\OnlyRetroRobloxHere-" + name);
		IsAcquired = Mutex.WaitOne(timeout);
	}

	public static bool TryCreate(string name, TimeSpan timeout, out InterProcessLock ipLock)
	{
		ipLock = new InterProcessLock(name, timeout);
		return ipLock.IsAcquired;
	}

	public void Dispose()
	{
		if (IsAcquired)
		{
			Mutex.ReleaseMutex();
			IsAcquired = false;
		}
	}
}

using System.Collections.ObjectModel;

namespace OnlyRetroRobloxHere.Launcher;

internal static class Clients
{
	public delegate void ClientsChangedHandler();

	public static ObservableCollection<string> List { get; } = new ObservableCollection<string>();

	public static event ClientsChangedHandler? ClientsChangedEvent;

	public static void NotifyChange()
	{
		Clients.ClientsChangedEvent?.Invoke();
	}
}

namespace OnlyRetroRobloxHere.Launcher;

internal static class Sounds
{
	private static SimpleAudioPlayer? _pageTurn;

	private static SimpleAudioPlayer? _ping;

	private static SimpleAudioPlayer? _secretActivated;

	public static SimpleAudioPlayer PageTurn => _pageTurn ?? (_pageTurn = new SimpleAudioPlayer("pack://application:,,,/OnlyRetroRobloxHere;component/Resources/Launcher/PageTurn.wav"));

	public static SimpleAudioPlayer Ping => _ping ?? (_ping = new SimpleAudioPlayer("pack://application:,,,/OnlyRetroRobloxHere;component/Resources/Sounds/Ping.ogg"));

	public static SimpleAudioPlayer SecretActivated => _secretActivated ?? (_secretActivated = new SimpleAudioPlayer("pack://application:,,,/OnlyRetroRobloxHere;component/Resources/Launcher/SecretActivated.wav"));

	public static SimpleAudioPlayer CashRegister { get; } = new SimpleAudioPlayer("pack://application:,,,/OnlyRetroRobloxHere;component/Resources/Sounds/CashRegister.ogg");
}

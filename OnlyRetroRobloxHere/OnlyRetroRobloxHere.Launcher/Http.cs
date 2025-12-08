using System.Net;
using System.Net.Http;

namespace OnlyRetroRobloxHere.Launcher;

internal static class Http
{
	public static HttpClient Client { get; }

	static Http()
	{
		Client = new HttpClient(new HttpClientHandler
		{
			AutomaticDecompression = DecompressionMethods.All
		});
	}
}

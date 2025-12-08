using System;
using System.Net;
using System.Net.Sockets;

namespace OnlyRetroRobloxHere.ServerAuthInterface;

internal static class Utils
{
	public static string GetLocalIP()
	{
		using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP);
		socket.Connect("8.8.8.8", 65530);
		return ((socket.LocalEndPoint is IPEndPoint iPEndPoint) ? iPEndPoint.Address.ToString() : null) ?? throw new Exception("Failed to get Local IP");
	}
}

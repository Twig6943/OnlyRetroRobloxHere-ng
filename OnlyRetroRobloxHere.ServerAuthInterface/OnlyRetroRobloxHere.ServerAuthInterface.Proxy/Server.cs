using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using OnlyRetroRobloxHere.Common;

namespace OnlyRetroRobloxHere.ServerAuthInterface.Proxy;

internal class Server
{
	public int ConnectionTimeout { get; set; } = 240000;

	public async Task Start(string remoteServerHostNameOrAddress, ushort remoteServerPort, ushort localPort, string? localIp = null)
	{
		ConcurrentDictionary<IPEndPoint, Connection> connections = new ConcurrentDictionary<IPEndPoint, Connection>();
		IPEndPoint remoteServerEndPoint = new IPEndPoint((await Dns.GetHostAddressesAsync(remoteServerHostNameOrAddress).ConfigureAwait(continueOnCapturedContext: false))[0], remoteServerPort);
		UdpClient localServer = new UdpClient(AddressFamily.InterNetworkV6);
		localServer.Client.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, optionValue: false);
		IPAddress address = (string.IsNullOrEmpty(localIp) ? IPAddress.IPv6Any : IPAddress.Parse(localIp));
		localServer.Client.Bind(new IPEndPoint(address, localPort));
		Task.Run(async delegate
		{
			while (true)
			{
				await Task.Delay(TimeSpan.FromSeconds(10.0)).ConfigureAwait(continueOnCapturedContext: false);
				KeyValuePair<IPEndPoint, Connection>[] array = connections.ToArray();
				for (int i = 0; i < array.Length; i++)
				{
					KeyValuePair<IPEndPoint, Connection> keyValuePair = array[i];
					if (keyValuePair.Value.LastActivity + ConnectionTimeout < Environment.TickCount64 || !keyValuePair.Value.IsRunning)
					{
						connections.TryRemove(keyValuePair.Key, out Connection _);
						keyValuePair.Value.Stop();
					}
				}
			}
		});
		while (true)
		{
			try
			{
				UdpReceiveResult udpReceiveResult = await localServer.ReceiveAsync().ConfigureAwait(continueOnCapturedContext: false);
				IPEndPoint sourceEndPoint = udpReceiveResult.RemoteEndPoint;
				Connection orAdd = connections.GetOrAdd(sourceEndPoint, delegate
				{
					Connection connection = new Connection(localServer, sourceEndPoint, remoteServerEndPoint);
					connection.Run();
					return connection;
				});
				await orAdd.SendToServerAsync(udpReceiveResult.Buffer).ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (Exception value)
			{
				Logger.Instance.Warn($"An exception occurred on receiving a client datagram: {value}");
			}
		}
	}
}

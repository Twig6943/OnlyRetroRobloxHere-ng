using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using OnlyRetroRobloxHere.Common;

namespace OnlyRetroRobloxHere.ServerAuthInterface.Proxy;

internal class Connection
{
	private readonly UdpClient _localServer;

	private readonly UdpClient _forwardClient;

	private readonly IPEndPoint _sourceEndpoint;

	private readonly IPEndPoint _remoteEndpoint;

	private readonly EndPoint? _serverLocalEndpoint;

	private EndPoint? _forwardLocalEndpoint;

	private bool _isRunning;

	private long _totalBytesForwarded;

	private long _totalBytesResponded;

	private readonly TaskCompletionSource<bool> _forwardConnectionBindCompleted = new TaskCompletionSource<bool>();

	public bool IsRunning => _isRunning;

	public long LastActivity { get; private set; } = Environment.TickCount64;

	public Connection(UdpClient localServer, IPEndPoint sourceEndpoint, IPEndPoint remoteEndpoint)
	{
		_localServer = localServer;
		_serverLocalEndpoint = _localServer.Client.LocalEndPoint;
		_isRunning = true;
		_remoteEndpoint = remoteEndpoint;
		_sourceEndpoint = sourceEndpoint;
		_forwardClient = new UdpClient(AddressFamily.InterNetworkV6);
		_forwardClient.Client.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, optionValue: false);
	}

	public async Task SendToServerAsync(byte[] message)
	{
		LastActivity = Environment.TickCount64;
		await _forwardConnectionBindCompleted.Task.ConfigureAwait(continueOnCapturedContext: false);
		if (_isRunning)
		{
			int num = await _forwardClient.SendAsync(message, message.Length, _remoteEndpoint).ConfigureAwait(continueOnCapturedContext: false);
			Interlocked.Add(ref _totalBytesForwarded, num);
		}
	}

	public void Run()
	{
		Task.Run(async delegate
		{
			using (_forwardClient)
			{
				if (!AuthService.Instance.IsIPAuthorised(_sourceEndpoint.Address.MapToIPv4()))
				{
					Logger.Instance.Info($"{_sourceEndpoint} - Not authorized");
					Stop();
					return;
				}
				_forwardClient.Client.Bind(new IPEndPoint(IPAddress.Any, 0));
				_forwardLocalEndpoint = _forwardClient.Client.LocalEndPoint;
				_forwardConnectionBindCompleted.SetResult(result: true);
				Logger.Instance.Info($"{_sourceEndpoint} - Connected");
				while (_isRunning)
				{
					try
					{
						UdpReceiveResult udpReceiveResult = await _forwardClient.ReceiveAsync().ConfigureAwait(continueOnCapturedContext: false);
						LastActivity = Environment.TickCount64;
						int num = await _localServer.SendAsync(udpReceiveResult.Buffer, udpReceiveResult.Buffer.Length, _sourceEndpoint).ConfigureAwait(continueOnCapturedContext: false);
						Interlocked.Add(ref _totalBytesResponded, num);
					}
					catch (Exception)
					{
						if (_isRunning)
						{
							Logger.Instance.Info("An exception occured while receiving a server datagram. Are you sure your server is open?");
						}
					}
				}
			}
		});
	}

	public void Stop()
	{
		if (!_isRunning)
		{
			return;
		}
		try
		{
			Logger.Instance.Info($"{_sourceEndpoint} - Disconnected");
			_isRunning = false;
			_forwardClient.Close();
		}
		catch (Exception value)
		{
			Console.WriteLine($"An exception occurred while closing UdpConnection : {value}");
		}
	}
}

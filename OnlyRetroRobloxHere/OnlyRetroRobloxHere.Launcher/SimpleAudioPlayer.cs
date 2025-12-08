using System;
using System.IO;
using NAudio.Vorbis;
using NAudio.Wave;
using OnlyRetroRobloxHere.Launcher.Extensions.NAudio;

namespace OnlyRetroRobloxHere.Launcher;

internal class SimpleAudioPlayer : IDisposable
{
	private Stream _stream;

	private WaveStream _waveStream;

	private WaveOut _player;

	public Uri Path { get; }

	public SimpleAudioPlayer(Uri path, bool loop = false)
	{
		Path = path;
		_stream = Utils.GetStreamFromUri(path);
		string absoluteUri = path.AbsoluteUri;
		if (path.AbsolutePath.EndsWith(".wav"))
		{
			_waveStream = new WaveFileReader(_stream);
		}
		else if (path.AbsolutePath.EndsWith(".mp3"))
		{
			_waveStream = new Mp3FileReader(_stream);
		}
		else
		{
			if (!path.AbsolutePath.EndsWith(".ogg"))
			{
				throw new Exception("Unknown file type for path " + absoluteUri);
			}
			_waveStream = new VorbisWaveReader(_stream);
		}
		if (loop)
		{
			_waveStream = new LoopStream(_waveStream);
		}
		_player = new WaveOut();
		_player.Init(_waveStream);
	}

	public SimpleAudioPlayer(string path, bool loop = false)
		: this(new Uri(path), loop)
	{
	}

	public void Play()
	{
		_waveStream.Seek(0L, SeekOrigin.Begin);
		_player.Play();
	}

	public void Stop()
	{
		_player.Stop();
	}

	public void Pause()
	{
		_player.Pause();
	}

	public void Resume()
	{
		_player.Resume();
	}

	public void Dispose()
	{
		_player.Dispose();
		_waveStream.Dispose();
		_stream?.Dispose();
	}
}

using System.Collections.Generic;
using System.Text;
using Ionic.Zlib;

namespace OnlyRetroRobloxHere.WebServer.Services;

internal class DataPersistenceService
{
	private static byte[] _emptyBlobBytes = GZipStream.CompressBuffer(Encoding.UTF8.GetBytes("<Table></Table>"));

	private const long MaxBlobSize = 280000L;

	public static DataPersistenceService Instance { get; set; } = new DataPersistenceService();

	public Dictionary<long, byte[]> Blobs { get; private set; } = new Dictionary<long, byte[]>();

	public void SaveBlob(long userId, byte[] blob, bool alreadyCompressed)
	{
		byte[] value = (alreadyCompressed ? blob : GZipStream.CompressBuffer(blob));
		Blobs[userId] = value;
	}

	public byte[] GetBlob(long userId)
	{
		if (!Blobs.ContainsKey(userId))
		{
			return _emptyBlobBytes;
		}
		return Blobs[userId];
	}
}

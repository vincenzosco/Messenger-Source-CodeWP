using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Facebook.Messenger.Utilities
{
	public class StorageUtils
	{
		public static async Task WriteBytesAsync(StorageFile file, byte[] bytes)
		{
			using (Stream stream = await ((IStorageFile)(object)file).OpenStreamForWriteAsync())
			{
				await stream.WriteAsync(bytes, 0, bytes.Length);
				await stream.FlushAsync();
			}
		}

		public static async Task<byte[]> ReadBytesAsync(StorageFile file)
		{
			IRandomAccessStream stream = await file.OpenAsync((FileAccessMode)0);
			try
			{
				using (Stream readStream = ((IInputStream)(object)stream).AsStreamForRead())
				{
					byte[] bytes = new byte[readStream.Length];
					await readStream.ReadAsync(bytes, 0, bytes.Length);
					return bytes;
				}
			}
			finally
			{
				if (stream != null)
				{
					((IDisposable)stream).Dispose();
				}
			}
		}
	}
}

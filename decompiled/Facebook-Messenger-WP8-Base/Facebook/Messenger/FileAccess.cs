using System;
using System.IO;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Logging;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Facebook.Messenger
{
	public class FileAccess : IFileAccess
	{
		public async Task<Stream> GetFileStream(string filePath)
		{
			try
			{
				return ((IInputStream)(object)(await (await StorageFile.GetFileFromPathAsync(filePath)).OpenAsync((FileAccessMode)0))).AsStreamForRead();
			}
			catch (Exception ex)
			{
				Logger.Error(this, $"Can't read file at {filePath}", "GetFileStream", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\FileAccess.cs", 22);
				Logger.Error(ex.ToString(), "GetFileStream", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\FileAccess.cs", 23);
			}
			return null;
		}

		public async Task DeleteFileAtPath(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				return;
			}
			try
			{
				await (await StorageFile.GetFileFromPathAsync(path)).DeleteAsync((StorageDeleteOption)1);
			}
			catch (Exception ex)
			{
				Logger.Error(this, $"Can't delete file at {path}", "DeleteFileAtPath", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\FileAccess.cs", 40);
				Logger.Error(ex.ToString(), "DeleteFileAtPath", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\FileAccess.cs", 41);
			}
		}

		public async Task<string> RenameFileAtPath(string path, string fileName)
		{
			try
			{
				StorageFile file = await StorageFile.GetFileFromPathAsync(path);
				await file.RenameAsync(fileName, (NameCollisionOption)0);
				path = file.Path;
			}
			catch (Exception ex)
			{
				Logger.Error(this, $"Can't rename file at {path}", "RenameFileAtPath", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\FileAccess.cs", 55);
				Logger.Error(ex.ToString(), "RenameFileAtPath", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\FileAccess.cs", 56);
			}
			return path;
		}
	}
}

using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;
using Facebook.Base.Logging;
using Windows.Storage;

namespace Facebook.Messenger.Utilities
{
	public class ErrorIgnoringStorageUtils
	{
		public static async Task DeleteIfExists(StorageFolder folder, string itemName, StorageDeleteOption deleteOption = (StorageDeleteOption)1)
		{
			IStorageItem item = await TryGetItemAsync(folder, itemName);
			if (item != null)
			{
				try
				{
					await item.DeleteAsync((StorageDeleteOption)1);
				}
				catch (Exception ex)
				{
					Logger.Error(string.Format("Couldn't delete file at path {0}", Path.Combine(folder.Path, itemName)), "DeleteIfExists", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\Utilities\\ErrorIgnoringStorageUtils.cs", 34);
					Logger.Error(ex.ToString(), "DeleteIfExists", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\Utilities\\ErrorIgnoringStorageUtils.cs", 35);
				}
			}
		}

		public static async Task<IStorageItem> TryGetItemAsync(StorageFolder folder, string itemName)
		{
			int num = default(int);
			int num2 = num;
			int num3 = 0;
			try
			{
				string path = Path.Combine(folder.Path, itemName);
				if (IsolatedStorageFile.GetUserStoreForApplication().FileExists(path) || IsolatedStorageFile.GetUserStoreForApplication().DirectoryExists(path))
				{
					return await folder.GetItemAsync(itemName);
				}
				return null;
			}
			catch (Exception ex)
			{
				Logger.Error(string.Format("Couldn't get item: {0}", itemName), "TryGetItemAsync", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\Utilities\\ErrorIgnoringStorageUtils.cs", 58);
				Logger.Error(ex.ToString(), "TryGetItemAsync", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\Utilities\\ErrorIgnoringStorageUtils.cs", 59);
				return null;
			}
		}

		public static void CreateFileInIsolatedStorage(string fileName, Action<IsolatedStorageFile, string> createAction)
		{
			try
			{
				using (IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication())
				{
					if (!isolatedStorageFile.FileExists(fileName))
					{
						createAction(isolatedStorageFile, fileName);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(string.Format("Couldn't create file: {0}", fileName), "CreateFileInIsolatedStorage", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\Utilities\\ErrorIgnoringStorageUtils.cs", 79);
				Logger.Error(ex.ToString(), "CreateFileInIsolatedStorage", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\Utilities\\ErrorIgnoringStorageUtils.cs", 80);
			}
		}

		public static bool DeleteFileInIsolateStorage(string file)
		{
			bool flag = false;
			try
			{
				using (IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication())
				{
					flag = isolatedStorageFile.FileExists(file);
					if (flag)
					{
						isolatedStorageFile.DeleteFile(file);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(string.Format("Couldn't delete file: {0}", file), "DeleteFileInIsolateStorage", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\Utilities\\ErrorIgnoringStorageUtils.cs", 100);
				Logger.Error(ex.ToString(), "DeleteFileInIsolateStorage", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\Utilities\\ErrorIgnoringStorageUtils.cs", 101);
			}
			return flag;
		}

		public static void AppendToFile(string path, byte[] buffer)
		{
			try
			{
				using (IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication())
				{
					using (IsolatedStorageFileStream isolatedStorageFileStream = (isolatedStorageFile.FileExists(path) ? isolatedStorageFile.OpenFile(path, FileMode.Append) : isolatedStorageFile.CreateFile(path)))
					{
						isolatedStorageFileStream.Write(buffer, 0, buffer.Length);
						isolatedStorageFileStream.Flush();
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(string.Format("Couldn't append file: {0}", path), "AppendToFile", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\Utilities\\ErrorIgnoringStorageUtils.cs", 123);
				Logger.Error(ex.ToString(), "AppendToFile", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\Utilities\\ErrorIgnoringStorageUtils.cs", 124);
			}
		}

		public static void OverwriteFile(string path, byte[] buffer)
		{
			try
			{
				using (IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication())
				{
					using (IsolatedStorageFileStream isolatedStorageFileStream = isolatedStorageFile.OpenFile(path, FileMode.Create))
					{
						isolatedStorageFileStream.Write(buffer, 0, buffer.Length);
						isolatedStorageFileStream.Flush();
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(string.Format("Couldn't overwrite file: {0}", path), "OverwriteFile", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\Utilities\\ErrorIgnoringStorageUtils.cs", 143);
				Logger.Error(ex.ToString(), "OverwriteFile", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\Utilities\\ErrorIgnoringStorageUtils.cs", 144);
			}
		}

		public static byte[] ReadFile(string path)
		{
			try
			{
				using (IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication())
				{
					if (isolatedStorageFile.FileExists(path))
					{
						using (IsolatedStorageFileStream isolatedStorageFileStream = isolatedStorageFile.OpenFile(path, FileMode.Open))
						{
							byte[] array = new byte[isolatedStorageFileStream.Length];
							isolatedStorageFileStream.Read(array, 0, array.Length);
							return array;
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(string.Format("Couldn't read file: {0}", path), "ReadFile", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\Utilities\\ErrorIgnoringStorageUtils.cs", 167);
				Logger.Error(ex.ToString(), "ReadFile", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\Utilities\\ErrorIgnoringStorageUtils.cs", 168);
			}
			return null;
		}
	}
}

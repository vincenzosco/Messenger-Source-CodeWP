using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Base.Logging;
using Windows.Storage;

namespace Facebook.Messenger.Base
{
	public static class StorageExtensions
	{
		public static async Task DeleteRecursivelyAsync(this StorageFolder root, StorageDeleteOption option = (StorageDeleteOption)0)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				IEnumerable<Task> tasks = (await root.GetFilesAsync()).Select((StorageFile file) =>
				{
					//IL_0002: Unknown result type (might be due to invalid IL or missing references)
					return file.DeleteAsync(option).AsTask();
				});
				await Task.WhenAll(tasks);
			}
			catch (Exception arg)
			{
				Logger.Warning(string.Format("Couldn't delete nested files in {0} {1}", root.Path, arg), "DeleteRecursivelyAsync", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\StorageExtensions.cs", 21);
			}
			try
			{
				IEnumerable<Task> tasks2 = (await root.GetFoldersAsync()).Select((StorageFolder folder) =>
				{
					//IL_0002: Unknown result type (might be due to invalid IL or missing references)
					return folder.DeleteRecursivelyAsync(option);
				});
				await Task.WhenAll(tasks2);
			}
			catch (Exception arg2)
			{
				Logger.Warning(string.Format("Couldn't delete nested folders in {0} {1}", root.Path, arg2), "DeleteRecursivelyAsync", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\StorageExtensions.cs", 32);
			}
			try
			{
				await root.DeleteAsync(option);
			}
			catch (Exception arg3)
			{
				Logger.Warning(string.Format("Couldn't delete folder {0} {1}", root.Path, arg3), "DeleteRecursivelyAsync", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\StorageExtensions.cs", 41);
			}
		}
	}
}

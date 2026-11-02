using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Facebook.Models;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace Facebook.Messenger.WP8.ViewModels.Internal
{
	public class DiskDetailsViewModel
	{
		private const string LevelSpacing = "  ";

		private readonly FacebookContext _context;

		public ObservableCollection<FileViewModel> FilesystemListing { get; private set; }

		public DiskDetailsViewModel(FacebookContext context)
		{
			_context = context;
			FilesystemListing = new ObservableCollection<FileViewModel>();
			CalculateFileListing();
		}

		public async void ReportProblemWithFile(FileViewModel vm)
		{
			if (!(await CrashSender.PostToServerAndDeleteLogFile("File sent via file picker", await StorageFile.GetFileFromPathAsync(vm.FullPath), _context.Session.Credentials.AccessToken)))
			{
				throw new Exception("Unknown failure");
			}
		}

		private async void CalculateFileListing()
		{
			StorageFolder root = ApplicationData.Current.LocalFolder;
			await BuildFolder((IStorageFolder)(object)root, string.Empty);
		}

		private async Task BuildFolder(IStorageFolder root, string indent)
		{
			foreach (StorageFile file in await root.GetFilesAsync().AsTask<IReadOnlyList<StorageFile>>())
			{
				ulong fileSizeKb = (await file.GetBasicPropertiesAsync().AsTask<BasicProperties>()).Size / 1024;
				string description = string.Format("{0}{1}KB {2}", indent, fileSizeKb, file.Name);
				FilesystemListing.Add(new FileViewModel(description, file.Path));
			}
			string nextIndent = indent + "  ";
			foreach (StorageFolder folder in await root.GetFoldersAsync().AsTask<IReadOnlyList<StorageFolder>>())
			{
				string description2 = string.Format("{0}[{1}]", indent, folder.Name);
				FilesystemListing.Add(new FileViewModel(description2, folder.Path));
				await BuildFolder((IStorageFolder)(object)folder, nextIndent);
			}
		}
	}
}

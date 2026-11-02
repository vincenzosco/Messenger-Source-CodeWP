namespace Facebook.Messenger.WP8.ViewModels.Internal
{
	public class FileViewModel
	{
		public string Description { get; private set; }

		public string FullPath { get; private set; }

		public FileViewModel(string description, string fullPath)
		{
			Description = description;
			FullPath = fullPath;
		}
	}
}

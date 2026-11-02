using System.Windows.Media;
using Facebook.Models;

namespace Facebook.Messenger.WP8.ViewModels
{
	public abstract class ImageDrawerItemViewModel : Notifiable
	{
		private bool _isSelected;

		public bool IsSelected
		{
			get
			{
				return _isSelected;
			}
			set
			{
				Set(ref _isSelected, value, "IsSelected");
			}
		}

		public virtual bool CanSend
		{
			get
			{
				return true;
			}
		}

		public abstract string GetName();

		public abstract PhotoData GetImageToSend();

		public abstract ImageSource GetDisplayImage(int width, int height);

		public abstract RotationAngle GetRotationAngle();
	}
}

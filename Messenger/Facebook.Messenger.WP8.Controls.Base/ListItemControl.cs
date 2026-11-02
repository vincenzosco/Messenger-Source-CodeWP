namespace Facebook.Messenger.WP8.Controls.Base
{
	public abstract class ListItemControl<TViewModel> : ViewModelControl<TViewModel> where TViewModel : class
	{
		protected abstract void OnItemValueAssigned();

		protected abstract void Recycled(TViewModel oldViewModel);

		protected sealed override void ViewModelUpdated(TViewModel oldViewModel)
		{
			base.ViewModelUpdated(oldViewModel);
			if (oldViewModel != null)
			{
				Recycled(oldViewModel);
			}
			if (ViewModel != null)
			{
				OnItemValueAssigned();
			}
		}
	}
}

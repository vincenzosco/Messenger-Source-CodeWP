using Facebook.Messenger.WP8.Controls.Base;
using Facebook.Messenger.WP8.Extensions;
using Facebook.Messenger.WP8.ViewModels;

namespace Facebook.Messenger.WP8.Controls
{
	public abstract class ContextualMessageListItemControl : ListItemControl<ContextualMessageViewModel>, ITiltable
	{
		protected static LayoutGenerator LayoutGenerator = new LayoutGenerator();
	}
}

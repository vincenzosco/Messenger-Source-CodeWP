namespace Facebook.Models.Signals
{
	public class ContactSyncToggled
	{
		public bool IsEnabled { get; set; }

		public ContactSyncToggled(bool isEnabled)
		{
			IsEnabled = isEnabled;
		}
	}
}

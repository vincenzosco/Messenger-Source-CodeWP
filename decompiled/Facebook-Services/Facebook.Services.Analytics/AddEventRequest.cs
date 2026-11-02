using Facebook.Base;

namespace Facebook.Services.Analytics
{
	public class AddEventRequest : Request
	{
		public AnalyticsEvent Event { get; private set; }

		public AddEventRequest(string name, string module)
		{
			Event = new AnalyticsEvent(name, module);
		}

		public AddEventRequest AddData(string key, string value)
		{
			Event.AddData(key, value ?? string.Empty);
			return this;
		}
	}
}

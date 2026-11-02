using System;
using Newtonsoft.Json;

namespace Facebook.Messenger.WP8.Events
{
	public class IdClickedArgs : EventArgs
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		public IdClickedArgs()
		{
		}

		public IdClickedArgs(string id)
		{
			Id = id;
		}
	}
}

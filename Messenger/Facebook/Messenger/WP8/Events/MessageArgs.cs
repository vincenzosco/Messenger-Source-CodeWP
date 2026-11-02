using System.Collections.Generic;
using Facebook.Models;
using Newtonsoft.Json;

namespace Facebook.Messenger.WP8.Events
{
	public class MessageArgs : IdClickedArgs
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("first_name")]
		public string FirstName { get; set; }

		[JsonProperty("middle_name")]
		public string MiddleName { get; set; }

		[JsonProperty("last_name")]
		public string LastName { get; set; }

		[JsonIgnore]
		public string NameWithSemicolon => Name + ";";

		public MessageArgs()
		{
		}

		public MessageArgs(FacebookProfile profile)
			: base(profile.Id)
		{
			Name = profile.Name;
		}

		public MessageArgs(FacebookUser user)
			: base(user.Id)
		{
			Name = user.Name;
			FirstName = user.FirstName;
			MiddleName = user.MiddleName;
			LastName = user.LastName;
		}

		public MessageArgs(string id, string name)
			: base(id)
		{
			Name = name;
		}

		public MessageArgs(List<string> values)
			: base(values[0])
		{
			Name = values[1];
			FirstName = values[2];
			LastName = values[3];
			MiddleName = values[4];
		}
	}
}

using System.Collections.Generic;
using System.Runtime.Serialization;
using Facebook.Models;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Cache
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class ContactCacheData
	{
		[JsonProperty("contacts")]
		[DataMember]
		public readonly Dictionary<string, FacebookUser> Contacts;

		[DataMember]
		[JsonProperty("last_sync_time")]
		public long LastSyncTime { get; set; }

		[JsonProperty("last_full_sync_time")]
		[DataMember]
		public long LastFullSyncTime { get; set; }

		public ContactCacheData()
		{
			Contacts = new Dictionary<string, FacebookUser>();
		}

		public void Clear()
		{
			Contacts.Clear();
			LastSyncTime = 0L;
			LastFullSyncTime = 0L;
		}
	}
}

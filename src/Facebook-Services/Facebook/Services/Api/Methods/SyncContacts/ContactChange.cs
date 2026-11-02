using System.Runtime.Serialization;
using Facebook.Models.PhoneContacts;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.SyncContacts
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class ContactChange
	{
		[DataMember]
		[JsonProperty("contact")]
		public FBPhoneContact Contact;

		[JsonProperty("client_contact_id")]
		[DataMember]
		public string ClientContactId;

		[JsonProperty("update_type")]
		[DataMember]
		public string ChangeType { get; set; }
	}
}

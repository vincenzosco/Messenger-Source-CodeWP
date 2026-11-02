using System.Collections.Generic;
using System.Runtime.Serialization;
using Facebook.Base;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.SyncContacts;

public class Response : Facebook.Base.Response
{
	[JsonProperty("import_id")]
	[DataMember]
	public string ImportId { get; set; }

	[JsonProperty("contact_changes")]
	[JsonConverter(typeof(ContactChangeResultConverter))]
	[DataMember]
	public Dictionary<string, ContactChangeResult> ContactChanges { get; set; }
}

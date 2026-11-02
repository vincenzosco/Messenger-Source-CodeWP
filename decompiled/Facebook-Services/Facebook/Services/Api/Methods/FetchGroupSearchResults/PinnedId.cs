using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.FetchGroupSearchResults;

[DataContract]
internal class PinnedId
{
	[DataMember]
	[JsonProperty("thread_id")]
	public string Id { get; set; }
}

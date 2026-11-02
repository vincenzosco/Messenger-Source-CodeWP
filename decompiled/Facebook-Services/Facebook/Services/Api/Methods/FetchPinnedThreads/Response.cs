using System.Collections.Generic;
using System.Runtime.Serialization;
using Facebook.Models;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.FetchPinnedThreads;

public class Response : FacebookApiResponse
{
	[JsonProperty(PropertyName = "data")]
	[DataMember]
	public List<PinnedThread> PinnedThreads;

	public bool ResultWasPersisted { get; set; }

	public Response()
	{
		ResultWasPersisted = false;
		PinnedThreads = new List<PinnedThread>();
	}
}

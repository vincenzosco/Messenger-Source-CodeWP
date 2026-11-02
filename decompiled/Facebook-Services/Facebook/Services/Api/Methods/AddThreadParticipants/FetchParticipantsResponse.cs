using System.Collections.Generic;
using System.Runtime.Serialization;
using Facebook.Models;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.AddThreadParticipants;

[DataContract]
[JsonObject(MemberSerialization.OptIn)]
public class FetchParticipantsResponse : FacebookApiResponse
{
	[JsonProperty("data")]
	public List<FacebookUser> Participants;
}

using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models;

[JsonObject(MemberSerialization.OptIn)]
[DataContract]
public class FacebookProfile : Notifiable, IEquatable<FacebookProfile>
{
	[JsonProperty(PropertyName = "id")]
	[DataMember]
	public string Id { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "name")]
	public string Name { get; set; }

	[JsonProperty(PropertyName = "type")]
	[DataMember]
	public string Type { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "username")]
	public string UserName { get; set; }

	public bool IsEmployee { get; set; }

	public bool IsKnown()
	{
		if (!string.IsNullOrWhiteSpace(Id))
		{
			return !string.Equals(Id, "0");
		}
		return false;
	}

	public bool Equals(FacebookProfile other)
	{
		if (object.ReferenceEquals(other, null))
		{
			return false;
		}
		if (object.ReferenceEquals(this, other))
		{
			return true;
		}
		return Id == other.Id;
	}

	public override int GetHashCode()
	{
		return Id.GetHashCode();
	}
}

using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Facebook.Models;

[JsonObject(MemberSerialization.OptIn)]
[DataContract]
public class FacebookCheckin
{
	private string _checkinText;

	[JsonProperty(PropertyName = "checkin_id")]
	[DataMember]
	public string CheckinId { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "actor_id")]
	public string ActorId { get; set; }

	public FacebookUser Actor { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "author_uid")]
	public string AuthorId { get; set; }

	public FacebookUser Author { get; set; }

	[JsonProperty(PropertyName = "target_id")]
	[DataMember]
	public string PlaceId { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "post_id")]
	public string PostId { get; set; }

	[JsonConverter(typeof(StringEnumConverter))]
	[DataMember]
	[JsonProperty(PropertyName = "type")]
	public CheckinType CheckinType { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "target_type")]
	public string TargetType { get; set; }

	public FacebookPlace Place { get; set; }

	[DataMember]
	[JsonConverter(typeof(CoordinatesConverter))]
	[JsonProperty(PropertyName = "coords")]
	public Dictionary<string, double> Coords { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "timestamp")]
	public long Timestamp { get; set; }

	[JsonProperty(PropertyName = "message")]
	[DataMember]
	public string Message { get; set; }

	[JsonProperty(PropertyName = "tagged_uids")]
	[JsonConverter(typeof(DictionaryKeysToListConverter))]
	[DataMember]
	public List<string> TaggedUids { get; set; }

	public List<FacebookUser> TaggedUsers { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "app_id")]
	public string AppId { get; set; }

	public double Longitude
	{
		get
		{
			if (Coords == null)
			{
				return double.NaN;
			}
			return Coords["longitude"];
		}
	}

	public double Latitude
	{
		get
		{
			if (Coords == null)
			{
				return double.NaN;
			}
			return Coords["latitude"];
		}
	}

	public string CheckinText
	{
		get
		{
			if (_checkinText != null)
			{
				return _checkinText;
			}
			if (TaggedUids == null || TaggedUids.Count == 0)
			{
				return _checkinText = $"at {Place.Name}";
			}
			return _checkinText = string.Format("with {0} other friend{1} at {2}", new object[3]
			{
				TaggedUids.Count,
				(TaggedUids.Count > 1) ? "s" : string.Empty,
				Place.Name
			});
		}
	}

	public FacebookUser DisplayUser { get; set; }
}

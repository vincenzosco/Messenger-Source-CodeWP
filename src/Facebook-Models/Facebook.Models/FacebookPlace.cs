using System.IO;
using System.Runtime.Serialization;
using Facebook.Base;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class FacebookPlace
	{
		[DataMember]
		[JsonProperty(PropertyName = "page_id")]
		public string PageId { get; set; }

		[JsonProperty(PropertyName = "target_id")]
		[DataMember]
		public string TargetId
		{
			set
			{
				PageId = value;
			}
		}

		[JsonProperty(PropertyName = "name")]
		[DataMember]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "short_name")]
		[DataMember]
		public string ShortName { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "checkin_count")]
		public int CheckinCount { get; set; }

		[JsonProperty(PropertyName = "latitude")]
		[DataMember]
		public string Latitude { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "longitude")]
		public string Longitude { get; set; }

		[JsonProperty(PropertyName = "pic_square")]
		[DataMember]
		public string Pic { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "display_subtext")]
		public string DisplaySubtext { get; set; }

		public string CountryCode { get; set; }

		public string State { get; set; }

		public string CreateLocationObject()
		{
			StringWriter stringWriter = new StringWriter();
			JsonTextWriter jsonTextWriter = new JsonTextWriter(stringWriter);
			jsonTextWriter.WriteStartObject();
			jsonTextWriter.WriteProperty("latitude", Latitude);
			jsonTextWriter.WriteProperty("longitude", Longitude);
			jsonTextWriter.WriteEndObject();
			return stringWriter.ToString();
		}

		public bool IsValidLocation()
		{
			if (!string.IsNullOrWhiteSpace(Latitude))
			{
				return !string.IsNullOrWhiteSpace(Longitude);
			}
			return false;
		}
	}
}

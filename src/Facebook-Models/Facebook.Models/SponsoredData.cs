using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class SponsoredData
	{
		[JsonProperty(PropertyName = "impression_logging_url")]
		[DataMember]
		public string ImpressionLoggingUrl { get; set; }

		[JsonProperty(PropertyName = "is_demo_ad")]
		[DataMember]
		public bool? IsDemoAd { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "is_non_connected_page_post")]
		public bool? IsNonConnectedPagePost { get; set; }

		[JsonProperty(PropertyName = "third_party_click_tracking_url")]
		[DataMember]
		public string ThirdPartyClickTrackingUrl { get; set; }

		[JsonProperty(PropertyName = "third_party_impression_logging_urls")]
		[DataMember]
		public List<string> ThirdPartyImpressionLoggingUrls { get; set; }
	}
}

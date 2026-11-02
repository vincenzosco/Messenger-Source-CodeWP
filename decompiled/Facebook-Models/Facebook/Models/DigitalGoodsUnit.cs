using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models;

[JsonObject(MemberSerialization.OptIn)]
[DataContract]
public class DigitalGoodsUnit
{
	public Story Parent { get; set; }

	[JsonConverter(typeof(FacebookSubTypeConverter))]
	[DataMember]
	[JsonProperty(PropertyName = "__type__")]
	public FacebookSubType Type { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "body")]
	public StringEntity Body { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "context_sentence")]
	public StringEntity ContextSentence { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "explanation")]
	public string Explanation { get; set; }

	[JsonProperty(PropertyName = "icon")]
	[DataMember]
	public FacebookImage Icon { get; set; }

	[JsonProperty(PropertyName = "media")]
	[DataMember]
	public FeedMedia Media { get; set; }

	[JsonProperty(PropertyName = "name")]
	[DataMember]
	public string Name { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "product_name")]
	public string ProductName { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "sponsored_data")]
	public SponsoredData SponsoredData { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "store_id")]
	public string StoreID { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "store_type")]
	public string StoreType { get; set; }

	[JsonProperty(PropertyName = "store_url")]
	[DataMember]
	public string StoreUrl { get; set; }

	[DataMember]
	[JsonProperty(PropertyName = "tracking")]
	public string Tracking { get; set; }

	[JsonProperty(PropertyName = "web_fallback_url")]
	[DataMember]
	public string WebFallbackUrl { get; set; }

	[OnDeserialized]
	public void OnDeserialized(StreamingContext context)
	{
	}
}

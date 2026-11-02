using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Facebook.Models.JsonConverters;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class FacebookNotification
	{
		public class DictionaryConverter : JsonConverter
		{
			public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
			{
				throw new NotImplementedException();
			}

			public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				if (reader.TokenType == JsonToken.String)
				{
					object value = reader.Value;
					JsonConvert.PopulateObject(value.ToString(), dictionary);
				}
				return dictionary;
			}

			public override bool CanConvert(Type objectType)
			{
				throw new NotImplementedException();
			}
		}

		[DataMember]
		[JsonProperty(PropertyName = "notification_id")]
		public string NotificationId { get; set; }

		[JsonProperty(PropertyName = "sender_id")]
		[DataMember]
		public string SenderId { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "object_id")]
		public string ObjectId { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "object_type")]
		public string ObjectType { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "title_text")]
		public string TitleText { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "title_html")]
		public string TitleHtml { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "icon_url")]
		public string IconUrl { get; set; }

		[JsonProperty(PropertyName = "href")]
		[DataMember]
		public string Href { get; set; }

		[JsonConverter(typeof(UnixDateTimeConverter))]
		[DataMember]
		[JsonProperty(PropertyName = "created_time")]
		public DateTime CreatedTime { get; set; }

		[DataMember]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		[JsonProperty(PropertyName = "updated_time")]
		public DateTime UpdatedTime { get; set; }

		public DateTime LocalUpdatedTime => UpdatedTime.ToLocalTime();

		[JsonProperty(PropertyName = "is_unread")]
		[DataMember]
		public bool IsUnread { get; set; }

		[DataMember]
		[JsonConverter(typeof(DictionaryConverter))]
		[JsonProperty(PropertyName = "join_data")]
		public Dictionary<string, object> JoinData { get; set; }

		public Uri NotificationAppUrl { get; set; }
	}
}

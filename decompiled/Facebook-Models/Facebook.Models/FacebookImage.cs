using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class FacebookImage
	{
		private Uri _uri;

		[DataMember]
		[JsonProperty(PropertyName = "height")]
		public int? Height { get; set; }

		[JsonProperty(PropertyName = "width")]
		[DataMember]
		public int? Width { get; set; }

		[JsonProperty(PropertyName = "scale")]
		[DataMember]
		public int? Scale { get; set; }

		[JsonProperty(PropertyName = "name")]
		[DataMember]
		public string Name { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "uri")]
		public string Url { get; set; }

		public Uri Uri
		{
			get
			{
				if (_uri == null && Uri.IsWellFormedUriString(Url, UriKind.Absolute))
				{
					_uri = new Uri(Url, UriKind.Absolute);
				}
				return _uri;
			}
		}
	}
}

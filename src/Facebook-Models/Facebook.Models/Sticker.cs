using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class Sticker
	{
		[JsonProperty(PropertyName = "id")]
		[DataMember]
		public string Id { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "thread_image")]
		public FacebookImage ThreadImage { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "composer_image")]
		public FacebookImage ComposerImage { get; set; }

		[JsonProperty(PropertyName = "animated_image")]
		[DataMember]
		public FacebookImage AnimatedImage { get; set; }

		public Sticker()
		{
		}

		public Sticker(Sticker sticker)
		{
			Id = sticker.Id;
			ThreadImage = sticker.ThreadImage;
			ComposerImage = sticker.ComposerImage;
			AnimatedImage = sticker.AnimatedImage;
		}
	}
}

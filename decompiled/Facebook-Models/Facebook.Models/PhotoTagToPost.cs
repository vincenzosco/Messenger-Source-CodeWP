using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	public class PhotoTagToPost
	{
		[JsonProperty("x")]
		[DataMember]
		public float? XCoord { get; set; }

		[JsonProperty("y")]
		[DataMember]
		public float? YCoord { get; set; }

		[DataMember]
		[JsonProperty("tag_text")]
		public string Text { get; set; }

		[JsonProperty("tag_uid")]
		[DataMember]
		public string ObjectId { get; set; }

		public PhotoTagToPost(PhotoTagEdge photoTagEdge)
		{
			ObjectId = photoTagEdge.Profile.Id;
			Text = photoTagEdge.Profile.Name;
			XCoord = photoTagEdge.TagObject.PhotoTagLocation.XCoordPercent;
			YCoord = photoTagEdge.TagObject.PhotoTagLocation.YCoordPercent;
		}
	}
}

using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.SetThreadPicture
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class Data
	{
		[JsonProperty("pic_hash")]
		public string PictureHash;
	}
}

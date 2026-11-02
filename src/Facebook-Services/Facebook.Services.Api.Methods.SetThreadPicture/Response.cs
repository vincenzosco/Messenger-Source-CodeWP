using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.SetThreadPicture
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class Response : FacebookApiResponse
	{
		public string PictureHash { get; set; }
	}
}

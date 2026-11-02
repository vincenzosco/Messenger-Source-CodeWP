using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class ReadReceipt
	{
		[DataMember]
		[JsonProperty("uid")]
		public string Id { get; set; }

		[JsonProperty("time")]
		[DataMember]
		public long Timestamp { get; set; }

		public static ReadReceipt CreateCopy(ReadReceipt receipt)
		{
			ReadReceipt readReceipt = new ReadReceipt();
			readReceipt.Id = receipt.Id;
			readReceipt.Timestamp = receipt.Timestamp;
			return readReceipt;
		}
	}
}

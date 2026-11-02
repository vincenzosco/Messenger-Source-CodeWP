using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Services.Mqtt.Sync
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class SyncMessageBase
	{
		private const int DELTA_AND_BATCH_SIZE = 700;

		[DataMember]
		[JsonProperty("encoding")]
		public string Encoding
		{
			get
			{
				return "THRIFT";
			}
		}

		[DataMember]
		[JsonProperty("max_deltas_able_to_process")]
		public int MaxDeltasAbleToProcess
		{
			get
			{
				return 700;
			}
		}

		[JsonProperty("delta_batch_size")]
		[DataMember]
		public int DeltaBatchSize
		{
			get
			{
				return 700;
			}
		}
	}
}

using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Services.Mqtt.Sync
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class CreateQueueMessage : SyncMessageBase
	{
		[DataMember]
		[JsonProperty("initial_titan_sequence_id")]
		public long InitialTitanSequenceId { get; private set; }

		[JsonProperty("sync_api_version")]
		[DataMember]
		public int SyncApiVersion { get; private set; }

		[JsonProperty("queue_type")]
		[DataMember]
		public string QueueType
		{
			get
			{
				return "m";
			}
		}

		[DataMember]
		[JsonProperty("device_params")]
		public DeviceParams DeviceParams
		{
			get
			{
				return new DeviceParams();
			}
		}

		public CreateQueueMessage(long initialTitanSequenceId, int syncApiVersion)
		{
			InitialTitanSequenceId = initialTitanSequenceId;
			SyncApiVersion = syncApiVersion;
		}
	}
}

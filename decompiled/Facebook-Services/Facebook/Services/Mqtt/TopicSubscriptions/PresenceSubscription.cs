using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Mqtt;
using Facebook.Base.Mqtt.Messages;
using Facebook.Models.Signals;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Facebook.Services.Mqtt.TopicSubscriptions;

public class PresenceSubscription : Subscription
{
	private const string PresenceTopic = "/orca_presence";

	public PresenceSubscription(IDispatcher uiDispatcher)
		: base(uiDispatcher, "/orca_presence", QualityofService.AcknowledgedDelivery)
	{
	}

	public override async Task HandleMessage(PublishMessage message)
	{
		await PerformSignal(await ParseMessage(message.GetPayloadAsString()));
	}

	private static Task<PresenceUpdateSignal> ParseMessage(string json)
	{
		return Task.Run(() =>
		{
			JsonTextReader jsonTextReader = new JsonTextReader(new StringReader(json));
			if (jsonTextReader.TokenType == JsonToken.Null)
			{
				return (PresenceUpdateSignal)null;
			}
			HashSet<string> hashSet = new HashSet<string>();
			HashSet<string> hashSet2 = new HashSet<string>();
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			Dictionary<string, long> dictionary2 = new Dictionary<string, long>();
			JToken jToken = JToken.ReadFrom(jsonTextReader);
			if (jToken["list"] != null)
			{
				string a = (string)jToken["list_type"];
				JToken jToken2 = jToken["list"];
				foreach (JToken item in (IEnumerable<JToken>)jToken2)
				{
					string text = (string)item["u"];
					long num = (long)item["p"];
					int? num2 = (int?)item["d"];
					long? num3 = (long?)item["l"];
					if (num2.HasValue)
					{
						dictionary.Add(text, num2.Value);
					}
					if (num3.HasValue)
					{
						dictionary2.Add(text, num3.Value);
					}
					if (num == 2)
					{
						hashSet.Add(text);
					}
					else
					{
						hashSet2.Add(text);
					}
				}
				return new PresenceUpdateSignal
				{
					OfflineIds = hashSet2,
					OnlineIds = hashSet,
					UidToClientType = dictionary,
					UidToLastActiveTime = dictionary2,
					Type = ((!string.Equals(a, "full")) ? PresenceUpdateType.Incremental : PresenceUpdateType.Full)
				};
			}
			return (PresenceUpdateSignal)null;
		});
	}
}

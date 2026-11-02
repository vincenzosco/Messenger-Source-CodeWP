using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.FetchGatekeeperSettings;

[JsonObject(MemberSerialization.OptIn)]
[DataContract]
public class Response : FacebookApiResponse
{
	[JsonProperty("result")]
	public string GatekeeperBitmask { get; set; }

	[JsonProperty("hash")]
	public string GatekeeperBitmaskServerHash { get; set; }

	public string GatekeeperBitmaskClientHash { get; set; }

	public string QueryHash { get; set; }

	public IEnumerable<string> GatekeeperQueryOrder { get; set; }

	public bool IsResponseValid()
	{
		bool flag = string.Equals(GatekeeperBitmaskServerHash, GatekeeperBitmaskClientHash, StringComparison.OrdinalIgnoreCase);
		bool result = GatekeeperBitmask != null && GatekeeperBitmask.Length == GatekeeperQueryOrder.Count();
		if (flag)
		{
			return result;
		}
		return false;
	}

	public IReadOnlyDictionary<string, bool> CalculateDictionaryFromResponse()
	{
		char[] array = GatekeeperBitmask.ToCharArray();
		string[] array2 = GatekeeperQueryOrder.ToArray();
		Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
		for (int i = 0; i < array.Count(); i++)
		{
			string key = array2[i];
			bool value = array[i] == '1';
			dictionary[key] = value;
		}
		return dictionary;
	}
}

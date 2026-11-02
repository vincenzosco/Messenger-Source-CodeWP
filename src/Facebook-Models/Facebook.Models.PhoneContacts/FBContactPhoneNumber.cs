using System.Collections.Generic;
using System.Runtime.Serialization;
using Facebook.Base;
using Newtonsoft.Json;

namespace Facebook.Models.PhoneContacts
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class FBContactPhoneNumber
	{
		[JsonProperty("number")]
		[DataMember]
		public string PhoneNumber;

		[JsonProperty("type")]
		[DataMember]
		public string Type;

		[JsonProperty("label")]
		[DataMember]
		public string Label;

		public override int GetHashCode()
		{
			List<object> list = new List<object>();
			list.Add(PhoneNumber);
			list.Add(Type);
			list.Add(Label);
			return HashGenerator.CalculateHashCode(list);
		}
	}
}

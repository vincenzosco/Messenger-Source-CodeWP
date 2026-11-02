using System.Collections.Generic;
using System.Runtime.Serialization;
using Facebook.Base;
using Newtonsoft.Json;

namespace Facebook.Models.PhoneContacts
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class FBContactEmail
	{
		[DataMember]
		[JsonProperty("email")]
		public string Email;

		[DataMember]
		[JsonProperty("type")]
		public string Type;

		[JsonProperty("label")]
		[DataMember]
		public string Label;

		public override int GetHashCode()
		{
			List<object> list = new List<object>();
			list.Add(Email);
			list.Add(Type);
			list.Add(Label);
			return HashGenerator.CalculateHashCode(list);
		}
	}
}

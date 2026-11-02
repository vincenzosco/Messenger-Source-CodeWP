using System.Collections.Generic;
using System.Runtime.Serialization;
using Facebook.Base;
using Newtonsoft.Json;

namespace Facebook.Models.PhoneContacts
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class FBContactBirthday
	{
		[JsonProperty("year")]
		[DataMember]
		public string Year;

		[JsonProperty("month")]
		[DataMember]
		public string Month;

		[JsonProperty("day")]
		[DataMember]
		public string Day;

		public override int GetHashCode()
		{
			List<object> list = new List<object>();
			list.Add(Year);
			list.Add(Month);
			list.Add(Day);
			return HashGenerator.CalculateHashCode(list);
		}
	}
}

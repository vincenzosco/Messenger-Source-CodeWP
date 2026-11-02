using System.Collections.Generic;
using System.Runtime.Serialization;
using Facebook.Base;
using Newtonsoft.Json;

namespace Facebook.Models.PhoneContacts
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class FBContactName
	{
		[JsonProperty("first")]
		[DataMember]
		public string FirstName;

		[JsonProperty("middle")]
		[DataMember]
		public string MiddleName;

		[DataMember]
		[JsonProperty("last")]
		public string LastName;

		[JsonProperty("formatted")]
		[DataMember]
		public string FormattedName;

		public override int GetHashCode()
		{
			List<object> list = new List<object>();
			list.Add(FirstName);
			list.Add(MiddleName);
			list.Add(LastName);
			list.Add(FormattedName);
			return HashGenerator.CalculateHashCode(list);
		}
	}
}

using System.Collections.Generic;
using System.Runtime.Serialization;
using Facebook.Base;
using Newtonsoft.Json;

namespace Facebook.Models.PhoneContacts
{
	[DataContract]
	[JsonObject(MemberSerialization.OptIn)]
	public class FBPhoneContact
	{
		public string ContactId;

		[JsonProperty("emails")]
		[DataMember]
		public List<FBContactEmail> Emails;

		[JsonProperty("phones")]
		[DataMember]
		public List<FBContactPhoneNumber> Phones;

		[DataMember]
		[JsonProperty("name")]
		public FBContactName Name;

		[JsonProperty("birthday")]
		[DataMember]
		public FBContactBirthday Birthday;

		public override int GetHashCode()
		{
			int num = 0;
			if (Emails != null)
			{
				num ^= HashGenerator.CalculateHashCode(Emails);
			}
			if (Phones != null)
			{
				num ^= HashGenerator.CalculateHashCode(Phones);
			}
			if (Name != null)
			{
				num ^= Name.GetHashCode();
			}
			if (Birthday != null)
			{
				num ^= Birthday.GetHashCode();
			}
			return num;
		}
	}
}

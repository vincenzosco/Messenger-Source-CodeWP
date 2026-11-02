using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Facebook.Models
{
	public class Relationship : IAboutType
	{
		public enum Status
		{
			[EnumMember(Value = "none")]
			None,
			[EnumMember(Value = "COMPLICATED")]
			Complicated,
			[EnumMember(Value = "SINGLE")]
			Single,
			[EnumMember(Value = "IN_A_RELATIONSHIP")]
			InARelationship,
			[EnumMember(Value = "ENGAGED")]
			Engaged,
			[EnumMember(Value = "MARRIED")]
			Married,
			[EnumMember(Value = "WIDOWED")]
			Widowed,
			[EnumMember(Value = "SEPARATED")]
			Separated,
			[EnumMember(Value = "DIVORCED")]
			Divorced,
			[EnumMember(Value = "OPEN_RELATIONSHIP")]
			OpenRelationship,
			[EnumMember(Value = "CIVIL_UNION")]
			CivilUnion,
			[EnumMember(Value = "DOMESTIC_PARTNERSHIP")]
			DomesticPartnership
		}

		[DataMember]
		[JsonConverter(typeof(StringEnumConverter))]
		[JsonProperty(PropertyName = "status")]
		public Status RelationshipStatus { get; set; }

		[JsonProperty(PropertyName = "relation")]
		[DataMember]
		public FacebookProfile Relation { get; set; }

		public AboutType Type
		{
			get
			{
				return AboutType.Relationship;
			}
		}
	}
}

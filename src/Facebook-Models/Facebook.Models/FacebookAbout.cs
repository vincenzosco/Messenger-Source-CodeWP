using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models
{
	public class FacebookAbout
	{
		private Contact _contact;

		private string _gender;

		private string _interestedIn;

		private BasicInfo _basicInfo;

		[JsonProperty(PropertyName = "work_experiences")]
		[DataMember]
		[JsonConverter(typeof(WorkExperiencesConverter))]
		public List<WorkExperience> WorkExperiences { get; set; }

		[JsonProperty(PropertyName = "education_experiences")]
		[JsonConverter(typeof(EducationExperiencesConverter))]
		[DataMember]
		public List<EducationExperience> EducationExperiences { get; set; }

		[JsonProperty(PropertyName = "current_city")]
		[DataMember]
		public LivingCity CurrentCity { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "hometown")]
		public LivingCity Hometown { get; set; }

		[JsonProperty(PropertyName = "email_addresses")]
		[DataMember]
		public List<string> Emails { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "address")]
		public Address Address { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "websites")]
		public List<string> Websites { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "all_phones")]
		public List<Phone> PhoneNumbers { get; set; }

		public Contact Contact
		{
			get
			{
				return _contact ?? (_contact = new Contact
				{
					PhoneNumbers = PhoneNumbers,
					Address = Address,
					Websites = Websites,
					Emails = Emails
				});
			}
		}

		[JsonConverter(typeof(FamilyConverter))]
		[DataMember]
		[JsonProperty(PropertyName = "family_members")]
		public List<FamilyMember> FamilyMembers { get; set; }

		[JsonProperty(PropertyName = "relationship")]
		[DataMember]
		public Relationship Relationship { get; set; }

		[DataMember]
		[JsonProperty(PropertyName = "gender")]
		private string Gender
		{
			get
			{
				return _gender;
			}
			set
			{
				switch (value)
				{
				case "FEMALE":
					_gender = "Female";
					break;
				case "MALE":
					_gender = "Male";
					break;
				default:
					_gender = null;
					break;
				}
			}
		}

		[JsonProperty(PropertyName = "interested_in")]
		[DataMember]
		private string InterestedIn
		{
			get
			{
				return _interestedIn;
			}
			set
			{
				switch (value)
				{
				case "WOMEN":
					_interestedIn = "Women";
					break;
				case "MEN":
					_interestedIn = "Men";
					break;
				case "MEN_AND_WOMEN":
					_interestedIn = "Men and Women";
					break;
				default:
					_interestedIn = null;
					break;
				}
			}
		}

		[JsonConverter(typeof(StringPropertyConverter))]
		[DataMember]
		[JsonProperty(PropertyName = "birthdate")]
		private string Birthdate { get; set; }

		public BasicInfo BasicInfo
		{
			get
			{
				return _basicInfo ?? (_basicInfo = new BasicInfo
				{
					Birthdate = Birthdate,
					InterestedIn = InterestedIn,
					Gender = Gender
				});
			}
		}
	}
}

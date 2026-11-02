using System.Runtime.Serialization;

namespace Facebook.Models
{
	public enum AboutType
	{
		[EnumMember(Value = "timeline-about-work-experience")]
		WorkExperience,
		[EnumMember(Value = "timeline-about-education-experience")]
		EducationExperience,
		[EnumMember(Value = "timeline-about-living")]
		Living,
		[EnumMember(Value = "timeline-about-contact")]
		Contact,
		[EnumMember(Value = "timeline-about-basic-info")]
		BasicInfo,
		[EnumMember(Value = "timeline-about-family")]
		Family,
		[EnumMember(Value = "timeline-about-professional-skills")]
		ProfessionalSkills,
		[EnumMember(Value = "timeline-about-about")]
		About,
		[EnumMember(Value = "timeline-about-fav-quotes")]
		FavoriteQuotations,
		[EnumMember(Value = "timeline-about-relationship")]
		Relationship
	}
}

using System.Runtime.Serialization;

namespace Facebook.Models
{
	public enum Gender
	{
		Unknown,
		[EnumMember(Value = "male")]
		Male,
		[EnumMember(Value = "female")]
		Female
	}
}

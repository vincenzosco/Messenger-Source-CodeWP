using System.Runtime.Serialization;

namespace Facebook.Models
{
	public enum PhoneType
	{
		Unknown,
		[EnumMember(Value = "cell")]
		Cell,
		[EnumMember(Value = "work")]
		Work,
		[EnumMember(Value = "home")]
		Home
	}
}

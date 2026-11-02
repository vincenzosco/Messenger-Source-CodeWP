using System.Runtime.Serialization;

namespace Facebook.Models
{
	public enum SearchResultType
	{
		Page,
		App,
		User,
		Group,
		[EnumMember(Value = "hashtag_exact")]
		HashtagExact
	}
}

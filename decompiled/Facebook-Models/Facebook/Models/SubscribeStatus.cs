using System.Runtime.Serialization;

namespace Facebook.Models;

public enum SubscribeStatus
{
	Unknown,
	[EnumMember(Value = "can_subscribe")]
	CanSubscribe,
	[EnumMember(Value = "is_subscribed")]
	IsSubscribed,
	[EnumMember(Value = "cannot_subscribe")]
	CannotSubscribe
}

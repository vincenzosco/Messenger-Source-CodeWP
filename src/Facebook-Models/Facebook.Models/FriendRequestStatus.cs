using System.Runtime.Serialization;

namespace Facebook.Models
{
	public enum FriendRequestStatus
	{
		Unknown,
		[EnumMember(Value = "cannot_request")]
		CannotRequest,
		[EnumMember(Value = "are_friends")]
		AreFriends,
		[EnumMember(Value = "incoming_request")]
		IncomingRequest,
		[EnumMember(Value = "outgoing_request")]
		OutgoingRequest,
		[EnumMember(Value = "can_request")]
		CanRequest
	}
}

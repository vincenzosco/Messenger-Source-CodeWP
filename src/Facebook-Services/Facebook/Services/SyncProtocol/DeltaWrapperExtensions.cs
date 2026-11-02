using System;
using Facebook.Models.SyncProtocol;

namespace Facebook.Services.SyncProtocol
{
	public static class DeltaWrapperExtensions
	{
		public static DeltaType GetDeltaType(this DeltaWrapper wrapper)
		{
			if (wrapper.__isset.deltaAdminTextMessage)
			{
				return DeltaType.ADMIN_TEXT;
			}
			if (wrapper.__isset.deltaForcedFetch)
			{
				return DeltaType.FORCED_FETCH;
			}
			if (wrapper.__isset.deltaMarkRead)
			{
				return DeltaType.MARK_READ;
			}
			if (wrapper.__isset.deltaMarkUnread)
			{
				return DeltaType.MARK_UNREAD;
			}
			if (wrapper.__isset.deltaMessageDelete)
			{
				return DeltaType.MESSAGE_DELETE;
			}
			if (wrapper.__isset.deltaNewGroupThread)
			{
				return DeltaType.NEW_GROUP_THREAD;
			}
			if (wrapper.__isset.deltaNewMessage)
			{
				return DeltaType.NEW_MESSAGE;
			}
			if (wrapper.__isset.deltaNoOp)
			{
				return DeltaType.NOOP;
			}
			if (wrapper.__isset.deltaParticipantLeftGroupThread)
			{
				return DeltaType.PARTICIPANT_LEFT_GROUP_THREAD;
			}
			if (wrapper.__isset.deltaParticipantsAddedToGroupThread)
			{
				return DeltaType.PARTICIPANTS_ADDED_TO_GROUP_THREAD;
			}
			if (wrapper.__isset.deltaReadReceipt)
			{
				return DeltaType.READ_RECEIPT;
			}
			if (wrapper.__isset.deltaThreadAction)
			{
				return DeltaType.THREAD_ACTION;
			}
			if (wrapper.__isset.deltaThreadDelete)
			{
				return DeltaType.THREAD_DELETE;
			}
			if (wrapper.__isset.deltaThreadFolder)
			{
				return DeltaType.THREAD_FOLDER;
			}
			if (wrapper.__isset.deltaThreadImage)
			{
				return DeltaType.THREAD_IMAGE;
			}
			if (wrapper.__isset.deltaThreadMuteSettings)
			{
				return DeltaType.THREAD_MUTE_SETTINGS;
			}
			if (wrapper.__isset.deltaThreadName)
			{
				return DeltaType.THREAD_NAME;
			}
			if (wrapper.__isset.deltaVideoCall)
			{
				return DeltaType.VIDEO_CALL;
			}
			if (wrapper.__isset.deltaRTCEventLog)
			{
				return DeltaType.RTC_EVENT_LOG;
			}
			if (wrapper.__isset.deltaBroadcastMessage)
			{
				return DeltaType.BROADCAST_MESSAGE;
			}
			if (wrapper.__isset.deltaSentMessage)
			{
				return DeltaType.SENT_MESSAGE;
			}
			if (wrapper.__isset.deltaDeliveryReceipt)
			{
				return DeltaType.DELIVERY_RECEIPT;
			}
			throw new InvalidOperationException();
		}

		public static bool IsEmptyForcedFetchDelta(this DeltaWrapper wrapper)
		{
			if (wrapper.__isset.deltaForcedFetch && wrapper.DeltaForcedFetch.MessageId == null)
			{
				return wrapper.DeltaForcedFetch.ThreadKey == null;
			}
			return false;
		}
	}
}

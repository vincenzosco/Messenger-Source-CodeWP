using System.Collections.Generic;
using System.Threading.Tasks;
using Facebook.Models;
using Facebook.Models.SyncProtocol;

namespace Facebook.Services.SyncProtocol.Handler
{
	public class DeltaParticipantLeftGroupThreadHandler : DeltaHandler
	{
		public DeltaParticipantLeftGroupThreadHandler()
			: base(DeltaType.PARTICIPANT_LEFT_GROUP_THREAD)
		{
		}

		public override Task<List<FetchRequired>> Ensure(DeltaHandlerContext context, DeltaWrapper wrapper)
		{
			DeltaParticipantLeftGroupThread deltaParticipantLeftGroupThread = wrapper.DeltaParticipantLeftGroupThread;
			if (LoggedInUserLeftGroup(context, deltaParticipantLeftGroupThread.LeftParticipantFbId))
			{
				return FetchRequired.NoFetchRequiredTaskResult;
			}
			ThreadKey threadKey = wrapper.DeltaParticipantLeftGroupThread.MessageMetadata.ThreadKey;
			return ThreadDeltaUtil.CreateFetchForThreadIfMissing(context.ThreadCache, threadKey);
		}

		public override async Task Handle(DeltaHandlerContext context, DeltaWrapper wrapper, bool shouldNotify)
		{
			DeltaParticipantLeftGroupThread delta = wrapper.DeltaParticipantLeftGroupThread;
			if (LoggedInUserLeftGroup(context, delta.LeftParticipantFbId))
			{
				string threadId = await ThreadDeltaUtil.GetThreadId(context.ThreadCache, delta.MessageMetadata.ThreadKey);
				if (!string.IsNullOrEmpty(threadId))
				{
					await context.ThreadCache.RemoveThreadAndPinnedThreadByThreadId(threadId);
				}
			}
			else
			{
				MailMessage message = await MessageDeltaUtil.CreateMessageForThread(context.ThreadCache, delta.MessageMetadata, MailMessage.Type.ParticipantLeft);
				await context.ThreadCache.AddMessageAndUpdateThread(message, shouldNotify);
				await context.ThreadCache.RemoveThreadParticipants(message.ThreadId, new string[1] { delta.LeftParticipantFbId.ToString() });
			}
		}

		private bool LoggedInUserLeftGroup(DeltaHandlerContext context, long leftParticipantFbid)
		{
			return context.LoggedInUserUtils.IsLoggedInUser(leftParticipantFbid);
		}
	}
}

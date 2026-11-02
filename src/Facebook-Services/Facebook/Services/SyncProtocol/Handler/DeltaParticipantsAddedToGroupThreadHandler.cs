using System.Collections.Generic;
using System.Threading.Tasks;
using Facebook.Models;
using Facebook.Models.SyncProtocol;

namespace Facebook.Services.SyncProtocol.Handler
{
	public class DeltaParticipantsAddedToGroupThreadHandler : DeltaHandler
	{
		public DeltaParticipantsAddedToGroupThreadHandler()
			: base(DeltaType.PARTICIPANTS_ADDED_TO_GROUP_THREAD)
		{
		}

		public override Task<List<FetchRequired>> Ensure(DeltaHandlerContext context, DeltaWrapper wrapper)
		{
			ThreadKey threadKey = wrapper.DeltaParticipantsAddedToGroupThread.MessageMetadata.ThreadKey;
			return Task.FromResult(FetchRequired.FetchThread(threadKey).ToList());
		}

		public override async Task Handle(DeltaHandlerContext context, DeltaWrapper wrapper, bool shouldNotify)
		{
			MailMessage message = await MessageDeltaUtil.CreateMessageForThread(metadata: wrapper.DeltaParticipantsAddedToGroupThread.MessageMetadata, cache: context.ThreadCache, messageType: MailMessage.Type.ParticipantAdded);
			await context.ThreadCache.AddMessageAndUpdateThread(message, shouldNotify);
		}
	}
}

using System.Collections.Generic;
using System.Threading.Tasks;
using Facebook.Models;
using Facebook.Models.SyncProtocol;
using Facebook.Services.Api.Cache;

namespace Facebook.Services.SyncProtocol.Handler;

public class DeltaNewMessageHandler : DeltaHandler
{
	public DeltaNewMessageHandler()
		: base(DeltaType.NEW_MESSAGE)
	{
	}

	public override Task<List<FetchRequired>> Ensure(DeltaHandlerContext context, DeltaWrapper wrapper)
	{
		ThreadKey threadKey = wrapper.DeltaNewMessage.MessageMetadata.ThreadKey;
		return ThreadDeltaUtil.CreateFetchForThreadIfMissing(context.ThreadCache, threadKey);
	}

	public override async Task Handle(DeltaHandlerContext context, DeltaWrapper wrapper, bool shouldNotify)
	{
		MailMessage message = await CreateMessageFromDelta(delta: wrapper.DeltaNewMessage, cache: context.ThreadCache);
		bool isOtherSender = message.Sender != null && !context.LoggedInUserUtils.IsLoggedInUser(message.Sender.Id);
		if (!isOtherSender)
		{
			await context.ThreadCache.UpdateThreadLastMessageState(message.ThreadId, null, MailMessage.MessageState.Sent);
		}
		await context.ThreadCache.AddMessageAndUpdateThread(message, shouldNotify, isOtherSender);
	}

	private static async Task<MailMessage> CreateMessageFromDelta(ThreadCache cache, DeltaNewMessage delta)
	{
		MailMessage message = await MessageDeltaUtil.CreateMessageForThread(cache, delta.MessageMetadata, MailMessage.Type.Regular);
		if (!string.IsNullOrWhiteSpace(delta.Body))
		{
			message.Body = delta.Body;
		}
		if (delta.__isset.stickerId)
		{
			message.Shares = MessageDeltaUtil.CreateStickerShares(delta.StickerId);
		}
		message.Coordinates = MessageDeltaUtil.CreateCoordinates(delta.Coordinates);
		message.Attachments = MessageDeltaUtil.CreateAttachments(delta.Attachments);
		return message;
	}
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Models;
using Facebook.Models.SyncProtocol;
using Facebook.Services.Api.Cache;

namespace Facebook.Services.SyncProtocol.Handler;

public class DeltaBroadcastMessageHandler : DeltaHandler
{
	public DeltaBroadcastMessageHandler()
		: base(DeltaType.BROADCAST_MESSAGE)
	{
	}

	public override Task<List<FetchRequired>> Ensure(DeltaHandlerContext context, DeltaWrapper wrapper)
	{
		IEnumerable<ThreadKey> threadKeys = wrapper.DeltaBroadcastMessage.MessageMetadatas.Select((MessageMetadata m) => m.ThreadKey);
		return ThreadDeltaUtil.CreateFetchesForThreadsIfMissing(context.ThreadCache, threadKeys);
	}

	public override async Task Handle(DeltaHandlerContext context, DeltaWrapper wrapper, bool shouldNotify)
	{
		DeltaBroadcastMessage delta = wrapper.DeltaBroadcastMessage;
		foreach (MessageMetadata metadata in delta.MessageMetadatas)
		{
			await context.ThreadCache.AddMessageAndUpdateThread(await CreateMessageFromDelta(context.ThreadCache, metadata, delta), shouldNotify);
		}
	}

	private static async Task<MailMessage> CreateMessageFromDelta(ThreadCache cache, MessageMetadata metadata, DeltaBroadcastMessage delta)
	{
		MailMessage message = await MessageDeltaUtil.CreateMessageForThread(cache, metadata, MailMessage.Type.Regular);
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

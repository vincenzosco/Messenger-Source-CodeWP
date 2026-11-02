using System.Collections.Generic;
using System.Threading.Tasks;
using Facebook.Models;
using Facebook.Models.SyncProtocol;

namespace Facebook.Services.SyncProtocol.Handler
{
	public class DeltaThreadImageHandler : DeltaHandler
	{
		public DeltaThreadImageHandler()
			: base(DeltaType.THREAD_IMAGE)
		{
		}

		public override Task<List<FetchRequired>> Ensure(DeltaHandlerContext context, DeltaWrapper wrapper)
		{
			ThreadKey threadKey = wrapper.DeltaThreadImage.MessageMetadata.ThreadKey;
			return ThreadDeltaUtil.CreateFetchForThreadIfMissing(context.ThreadCache, threadKey);
		}

		public override async Task Handle(DeltaHandlerContext context, DeltaWrapper wrapper, bool shouldNotify)
		{
			DeltaThreadImage delta = wrapper.DeltaThreadImage;
			MailMessage.Type messageType = GetMessageType(delta);
			MailMessage message = await MessageDeltaUtil.CreateMessageForThread(context.ThreadCache, delta.MessageMetadata, messageType);
			if (messageType == MailMessage.Type.ThreadImageSet)
			{
				message.Attachments = MessageDeltaUtil.CreateAttachments(new Facebook.Models.SyncProtocol.Attachment[1] { delta.Image });
			}
			string pictureHash = (delta.__isset.image ? delta.Image.Filename.GetHashCode().ToString() : null);
			await context.ThreadCache.SetThreadPicture(message.ThreadId, pictureHash);
			await context.ThreadCache.AddMessageAndUpdateThread(message, shouldNotify);
		}

		private static MailMessage.Type GetMessageType(DeltaThreadImage delta)
		{
			if (delta.Image != null)
			{
				return MailMessage.Type.ThreadImageSet;
			}
			return MailMessage.Type.ThreadImageRemoved;
		}
	}
}

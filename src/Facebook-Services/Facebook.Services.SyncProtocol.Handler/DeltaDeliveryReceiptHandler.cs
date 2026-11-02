using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Base.Logging;
using Facebook.Models;
using Facebook.Models.SyncProtocol;

namespace Facebook.Services.SyncProtocol.Handler
{
	public class DeltaDeliveryReceiptHandler : DeltaHandler
	{
		public DeltaDeliveryReceiptHandler()
			: base(DeltaType.DELIVERY_RECEIPT)
		{
		}

		public override Task<List<FetchRequired>> Ensure(DeltaHandlerContext context, DeltaWrapper wrapper)
		{
			return FetchRequired.NoFetchRequiredTaskResult;
		}

		public override async Task Handle(DeltaHandlerContext context, DeltaWrapper wrapper, bool shouldNotify)
		{
			DeltaDeliveryReceipt delta = wrapper.DeltaDeliveryReceipt;
			string threadId = await ThreadDeltaUtil.GetThreadId(context.ThreadCache, delta.ThreadKey);
			if (threadId == null)
			{
				Logger.Warning(this, string.Format("Received a Delivery Receipt delta, but the thread was not in the cache.  ThreadFbId: {0}, OtherUserFbId: {1}, First MessageId: {2}", new object[3]
				{
					delta.ThreadKey.ThreadFbId,
					delta.ThreadKey.OtherUserFbId,
					delta.MessageIds.FirstOrDefault()
				}), "Handle", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\SyncProtocol\\Handler\\DeltaDeliveryReceiptHandler.cs", 28);
				return;
			}
			if (delta.DeliveredWatermarkTimestampMs > 0)
			{
				await context.ThreadCache.MarkMessagesDeliveredByTimestamp(threadId, delta.DeliveredWatermarkTimestampMs);
			}
			else if (delta.MessageIds.Any())
			{
				List<string> preparedMessageIds = delta.MessageIds.Select((string id) => ThreadDeltaUtil.PrepareMessageId(id)).ToList();
				await context.ThreadCache.MarkMessagesDeliveredByIds(threadId, preparedMessageIds);
			}
			DeliveryReceipt deliveryReceipt = CreateDeliveryReceipt(delta);
			await context.ThreadCache.UpdateDeliveryReceipt(threadId, deliveryReceipt);
		}

		private static DeliveryReceipt CreateDeliveryReceipt(DeltaDeliveryReceipt delta)
		{
			long num = (delta.ThreadKey.__isset.otherUserFbId ? delta.ThreadKey.OtherUserFbId : delta.ActorFbId);
			DeliveryReceipt deliveryReceipt = new DeliveryReceipt();
			deliveryReceipt.Id = num.ToString();
			deliveryReceipt.Timestamp = delta.DeliveredWatermarkTimestampMs;
			return deliveryReceipt;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Facebook.Base.Logging;
using Facebook.Models.SyncProtocol;

namespace Facebook.Services.SyncProtocol.Handler
{
	public class DeltaThreadFolderHandler : DeltaHandler
	{
		public DeltaThreadFolderHandler()
			: base(DeltaType.THREAD_FOLDER)
		{
		}

		public override Task<List<FetchRequired>> Ensure(DeltaHandlerContext context, DeltaWrapper wrapper)
		{
			DeltaThreadFolder deltaThreadFolder = wrapper.DeltaThreadFolder;
			switch (deltaThreadFolder.Folder)
			{
			case ThreadFolder.FOLDER_INBOX:
				return Task.FromResult(FetchRequired.FetchThread(deltaThreadFolder.ThreadKey).ToList());
			case ThreadFolder.FOLDER_INVALID:
			case ThreadFolder.FOLDER_OTHER:
			case ThreadFolder.FOLDER_SPAM:
				return FetchRequired.NoFetchRequiredTaskResult;
			default:
				throw new NotImplementedException("New folder type added but no handler");
			}
		}

		public override async Task Handle(DeltaHandlerContext context, DeltaWrapper wrapper, bool shouldNotify)
		{
			DeltaThreadFolder delta = wrapper.DeltaThreadFolder;
			string threadId = await ThreadDeltaUtil.GetThreadId(context.ThreadCache, delta.ThreadKey);
			switch (delta.Folder)
			{
			case ThreadFolder.FOLDER_INVALID:
			case ThreadFolder.FOLDER_OTHER:
				if (threadId == null)
				{
					Logger.Info(this, "Attempted to move thread to other but no thread is found", "Handle", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\SyncProtocol\\Handler\\DeltaThreadFolderHandler.cs", 45);
				}
				else
				{
					await context.ThreadCache.RemoveThreadByThreadId(threadId);
				}
				break;
			case ThreadFolder.FOLDER_SPAM:
				if (threadId == null)
				{
					Logger.Info(this, "Attempted to move thread to spam but no thread is found", "Handle", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\SyncProtocol\\Handler\\DeltaThreadFolderHandler.cs", 53);
				}
				else
				{
					await context.ThreadCache.RemoveThreadAndPinnedThreadByThreadId(threadId);
				}
				break;
			default:
				throw new NotImplementedException("New action type added but no handler");
			case ThreadFolder.FOLDER_INBOX:
				break;
			}
		}
	}
}

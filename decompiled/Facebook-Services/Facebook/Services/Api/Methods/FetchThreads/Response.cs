using System.Collections.Generic;
using Facebook.Base;
using Facebook.Models;

namespace Facebook.Services.Api.Methods.FetchThreads;

public class Response : Facebook.Base.Response
{
	public List<MailThread> Threads { get; set; }

	public MailFolderCount Counts { get; set; }

	public List<string> ThreadIdsWithFailedSentMessages { get; set; }

	public bool ShouldPurgeThreadCache { get; set; }

	public bool ReceivedThreadsFromWebOrSync { get; set; }
}

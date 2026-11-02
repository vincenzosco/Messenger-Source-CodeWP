using System.Collections.Generic;
using Facebook.Base;
using Facebook.Models;

namespace Facebook.Services.Api.Methods.FetchGroupThreads;

public class Response : Facebook.Base.Response
{
	public List<MailThread> Threads;
}

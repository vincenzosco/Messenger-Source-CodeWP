using Facebook.Base;
using Facebook.Models;

namespace Facebook.Services.Api.Methods.FetchThread;

public class Response : Facebook.Base.Response
{
	public MailThread Thread { get; set; }
}

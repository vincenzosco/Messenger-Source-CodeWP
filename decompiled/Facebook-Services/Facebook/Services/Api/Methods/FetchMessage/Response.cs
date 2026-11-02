using Facebook.Models;

namespace Facebook.Services.Api.Methods.FetchMessage;

public class Response : FacebookApiResponse
{
	public MailMessage Message { get; set; }
}

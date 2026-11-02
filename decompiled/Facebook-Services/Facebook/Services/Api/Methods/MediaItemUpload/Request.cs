using Facebook.Services.Api.Methods.SendMessage;

namespace Facebook.Services.Api.Methods.MediaItemUpload;

[Queue(QueueType.Web)]
public class Request : FacebookServiceRequest
{
	public MediaType Type { get; set; }

	public Attachment MediaAttachment { get; set; }

	public static Request CreateFrom(Facebook.Services.Api.Methods.SendMessage.Request request)
	{
		Request request2 = new Request();
		request2.AccessToken = request.AccessToken;
		request2.CancelToken = request.CancelToken;
		return request2;
	}
}

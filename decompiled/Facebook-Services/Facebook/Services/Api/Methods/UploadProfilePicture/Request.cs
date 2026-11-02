namespace Facebook.Services.Api.Methods.UploadProfilePicture;

[Queue(QueueType.Web)]
public class Request : FacebookServiceRequest
{
	public string ImageUrl { get; set; }
}

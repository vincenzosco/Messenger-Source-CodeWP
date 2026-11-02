namespace Facebook.Services.Api.Methods.FetchStickerPackIds;

[Queue(QueueType.Web)]
public class Request : FacebookServiceRequest
{
	public enum SourceType
	{
		DownloadedPacks,
		OwnedPacks,
		StorePacks
	}

	public SourceType Type { get; set; }
}

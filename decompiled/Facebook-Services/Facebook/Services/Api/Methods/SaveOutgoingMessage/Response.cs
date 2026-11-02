using Facebook.Base;

namespace Facebook.Services.Api.Methods.SaveOutgoingMessage;

public class Response : Facebook.Base.Response
{
	public string GeneratedLocalThreadId { get; set; }

	public bool DidGenerateLocalThreadId()
	{
		return !string.IsNullOrWhiteSpace(GeneratedLocalThreadId);
	}
}

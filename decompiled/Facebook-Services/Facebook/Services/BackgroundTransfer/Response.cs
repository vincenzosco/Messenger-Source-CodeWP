using Facebook.Base.BackgroundTransfer;
using Facebook.Services.Http;

namespace Facebook.Services.BackgroundTransfer;

public class Response : Facebook.Services.Http.Response
{
	public AbstractBackgroundTransferOperation Operation { get; set; }
}

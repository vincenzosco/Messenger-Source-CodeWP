using Facebook.Models;

namespace Facebook.Services.Api.Methods.FetchSticker
{
	public class Response : FacebookApiResponse
	{
		public Sticker Sticker { get; set; }
	}
}

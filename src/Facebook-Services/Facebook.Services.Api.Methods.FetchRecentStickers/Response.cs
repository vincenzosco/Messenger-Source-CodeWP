using System.Collections.Generic;
using Facebook.Base;
using Facebook.Models;

namespace Facebook.Services.Api.Methods.FetchRecentStickers
{
	public class Response : Facebook.Base.Response
	{
		public List<Sticker> RecentStickers { get; set; }
	}
}

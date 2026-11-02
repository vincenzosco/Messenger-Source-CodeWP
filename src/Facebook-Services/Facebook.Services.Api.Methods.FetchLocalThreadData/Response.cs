using System.Collections.Generic;
using Facebook.Base;

namespace Facebook.Services.Api.Methods.FetchLocalThreadData
{
	public class Response : Facebook.Base.Response
	{
		public List<string> ThreadIdsWithFailedUnsentMessages { get; private set; }

		public Response()
		{
			ThreadIdsWithFailedUnsentMessages = new List<string>();
		}
	}
}

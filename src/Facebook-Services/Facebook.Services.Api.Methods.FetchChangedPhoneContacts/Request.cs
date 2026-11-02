using System.Collections.Generic;
using Facebook.Models.PhoneContacts;

namespace Facebook.Services.Api.Methods.FetchChangedPhoneContacts
{
	[Queue(QueueType.Web)]
	public class Request : FacebookServiceRequest
	{
		public List<FBPhoneContact> PhoneContacts;
	}
}

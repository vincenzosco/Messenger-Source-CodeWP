using System.Collections.Generic;
using Facebook.Models.PhoneContacts;

namespace Facebook.Services.Api.Methods.FetchChangedPhoneContacts;

public class Response : FacebookApiResponse
{
	public List<FBPhoneContact> ChangedContacts;
}

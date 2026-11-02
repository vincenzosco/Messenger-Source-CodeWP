using System.Collections.Generic;
using Facebook.Base;
using Facebook.Models;

namespace Facebook.Services.Api.Methods.FetchFriends;

public class Response : Facebook.Base.Response
{
	public FacebookUser Friend { get; set; }

	public List<FacebookUser> Friends { get; set; }
}

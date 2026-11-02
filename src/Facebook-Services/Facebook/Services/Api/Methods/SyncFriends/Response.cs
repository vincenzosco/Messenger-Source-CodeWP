using System.Collections.Generic;
using Facebook.Base;
using Facebook.Models;

namespace Facebook.Services.Api.Methods.SyncFriends
{
	public class Response : Facebook.Base.Response
	{
		public List<FacebookUser> NewFriends { get; set; }

		public List<FacebookUser> DeletedFriends { get; set; }
	}
}

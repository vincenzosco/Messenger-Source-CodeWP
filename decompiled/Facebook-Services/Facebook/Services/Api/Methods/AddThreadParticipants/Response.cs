using System.Collections.Generic;
using Facebook.Models;
using Facebook.Services.Common;

namespace Facebook.Services.Api.Methods.AddThreadParticipants;

public class Response : BoolResponse
{
	public List<FacebookUser> AddedParticipants;
}

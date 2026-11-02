using System.Collections.Generic;
using Facebook.Base;
using Facebook.Base.Logging;
using Facebook.Models;

namespace Facebook.Services.Api.Methods.Login;

public class Response : Facebook.Base.Response
{
	public enum State
	{
		Failed,
		Unauthorized,
		NotAcceptable,
		Succeeded
	}

	public State LoginState { get; set; }

	public FacebookSession Session { get; set; }

	public string MachineId { get; set; }

	public List<PhoneInfo> PhoneInfos { get; set; }

	public static Response CreateFromAuthAndProfile(AuthResponse authResponse, FacebookUser profile)
	{
		if (authResponse == null || profile == null || string.IsNullOrEmpty(profile.Id))
		{
			Logger.Warning("Authentication response or profile was null", "CreateFromAuthAndProfile", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Methods\\Login\\Request.cs", 61);
			Response response = new Response();
			response.LoginState = State.Failed;
			response.Error = new Error();
			return response;
		}
		Response response2 = new Response();
		response2.MachineId = authResponse.MachineId;
		response2.LoginState = State.Succeeded;
		response2.Session = new FacebookSession
		{
			UserId = authResponse.UserId,
			Credentials = new FacebookCredentials
			{
				AccessToken = authResponse.AccessToken,
				SessionKey = authResponse.SessionKey,
				Secret = authResponse.Secret,
				Id = authResponse.UserId
			},
			Profile = profile
		};
		response2.PhoneInfos = profile.Phones;
		return response2;
	}
}

using System.Linq;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Services.Api.Methods.FetchUser;

namespace Facebook.Services.Api.Methods.Login
{
	public abstract class AuthFilter<TRequest> : FacebookApiFilter<TRequest, Response> where TRequest : FacebookServiceRequest
	{
		protected AuthFilter(ServiceContext context)
			: base(context)
		{
		}

		public async Task<Response> ParseAuthResponse(AuthResponse authResponse)
		{
			if (authResponse.HasError())
			{
				Response.State loginState = Response.State.Failed;
				int? errorCode = authResponse.ErrorCode;
				int valueOrDefault = errorCode.GetValueOrDefault();
				if (errorCode.HasValue)
				{
					switch (valueOrDefault)
					{
					case 401:
						loginState = Response.State.Unauthorized;
						break;
					case 406:
						loginState = Response.State.NotAcceptable;
						break;
					}
				}
				Error error = authResponse.Error;
				if (error.Code == 0)
				{
					error.Code = authResponse.ErrorCode ?? 0;
				}
				Response response = new Response();
				response.LoginState = loginState;
				response.Error = error;
				return response;
			}
			Facebook.Services.Api.Methods.FetchUser.Request fetchUserRequest = new Facebook.Services.Api.Methods.FetchUser.Request
			{
				AccessToken = authResponse.AccessToken
			};
			Facebook.Services.Api.Methods.FetchUser.Response users = await ExecuteApiRequest<Facebook.Services.Api.Methods.FetchUser.Request, Facebook.Services.Api.Methods.FetchUser.Response>(fetchUserRequest);
			if (users.HasError())
			{
				Response response2 = new Response();
				response2.LoginState = Response.State.Failed;
				response2.Error = users.Error;
				return response2;
			}
			if (users.UserData != null && users.UserData.Any())
			{
				return Response.CreateFromAuthAndProfile(authResponse, users.UserData.First());
			}
			return new Response
			{
				LoginState = Response.State.Failed,
				Error = new Error
				{
					Message = "Unexpected error has occurred"
				}
			};
		}
	}
}

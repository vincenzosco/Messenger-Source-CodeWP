using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Models;
using Facebook.Services.Api.Methods;

namespace Facebook.Services.Api;

internal class AuthCheckFilter : IFilter
{
	private const int GraphApiSessionErrorCode = 190;

	private const int RestApiSessionErrorCode = 102;

	private readonly FacebookContext _context;

	private readonly IDispatcher _dispatcher;

	public AuthCheckFilter(FacebookContext context, IDispatcher dispatcher)
	{
		_context = context;
		_dispatcher = dispatcher;
	}

	public async Task<TResponse> ExecuteAsync<TRequest, TResponse>(TRequest request, IHandler next) where TRequest : Request where TResponse : Response
	{
		TResponse response = await next.ExecuteAsync<TRequest, TResponse>(request);
		if (response is FacebookApiResponse)
		{
			FacebookApiResponse facebookApiResponse = response as FacebookApiResponse;
			if (facebookApiResponse.ApiError != null)
			{
				long errorCode = facebookApiResponse.ApiError.ErrorCode;
				if (errorCode == 190 || errorCode == 102)
				{
					facebookApiResponse.SessionExpired = true;
					_dispatcher.Schedule(() =>
					{
						_context.Logout(expiredSession: true);
					});
				}
			}
		}
		return response;
	}
}

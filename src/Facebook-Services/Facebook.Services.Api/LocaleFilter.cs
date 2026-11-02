using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Services.Api.Methods;

namespace Facebook.Services.Api
{
	internal class LocaleFilter : IFilter
	{
		private readonly ITranslator _translator;

		public LocaleFilter(ITranslator translator)
		{
			_translator = translator;
		}

		public async Task<TResponse> ExecuteAsync<TRequest, TResponse>(TRequest request, IHandler next) where TRequest : Request where TResponse : Response
		{
			FacebookServiceRequest fbRequest = request as FacebookServiceRequest;
			if (fbRequest != null)
			{
				fbRequest.Locale = _translator.GetCurrentFBLocale();
			}
			return await next.ExecuteAsync<TRequest, TResponse>(request);
		}
	}
}

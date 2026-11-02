using System.Collections.Generic;
using System.Threading.Tasks;

namespace Facebook.Base
{
	public interface IPendingRequestStorage<TRequest, TResponse> where TRequest : Request where TResponse : Response
	{
		Task MarkSuccessfulResponses();

		void SavePendingRequests(List<TRequest> requests);

		TResponse GetSuccessfulResponse(TRequest request);
	}
}

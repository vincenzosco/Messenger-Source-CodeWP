using System.Collections.Generic;

namespace Facebook.Services.Api.Methods.FetchGatekeeperSettings;

[Queue(QueueType.Web)]
public class Request : FacebookServiceRequest
{
	public string LastQueryHash { get; set; }

	public IEnumerable<string> ProjectNames { get; set; }
}

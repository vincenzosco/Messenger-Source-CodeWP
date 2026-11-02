using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Models;
using Facebook.Services.Api;
using Facebook.Services.Api.Methods.FetchGatekeeperSettings;

namespace Facebook.Services.ScheduledTasksService.Tasks
{
	public class GatekeeperSync : ScheduledTask
	{
		private const string TaskName = "GatekeeperSync";

		private const int TaskIntervalMs = 3600000;

		public GatekeeperSync()
			: base("GatekeeperSync", 3600000L)
		{
		}

		public override async Task<Result> Execute(Facebook.Services.Api.Service apiService, FacebookContext context)
		{
			if (!GatekeeperProjects.Set.Any())
			{
				return Result.Success;
			}
			Request request = new Request
			{
				AccessToken = context.Session.Credentials.AccessToken,
				ProjectNames = GatekeeperProjects.Set,
				LastQueryHash = context.GetLastSuccessfulGatekeeperQueryHash()
			};
			Response response = await apiService.ExecuteAsync<Request, Response>(request);
			if (response.HasError())
			{
				return Result.Failure;
			}
			if (response.IsResponseValid())
			{
				IReadOnlyDictionary<string, bool> gatekeepers = response.CalculateDictionaryFromResponse();
				context.SetGatekeepers(gatekeepers);
				context.SetLastSuccessfulGatekeeperQueryHash(response.QueryHash);
			}
			return Result.Success;
		}
	}
}

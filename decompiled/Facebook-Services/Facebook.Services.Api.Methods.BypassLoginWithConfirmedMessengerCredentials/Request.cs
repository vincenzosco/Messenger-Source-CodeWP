namespace Facebook.Services.Api.Methods.BypassLoginWithConfirmedMessengerCredentials
{
	[Queue(QueueType.Web)]
	public class Request : FacebookServiceRequest
	{
		public string DeviceId { get; private set; }

		public string AccountId { get; private set; }

		public string MachineId { get; private set; }

		public Request(string deviceId, string accountId, string machineId = null)
		{
			DeviceId = deviceId;
			AccountId = accountId;
			MachineId = machineId;
		}
	}
}

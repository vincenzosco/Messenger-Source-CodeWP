namespace Facebook.Services.Api.Methods.Login
{
	[Queue(QueueType.Web)]
	public class Request : FacebookServiceRequest
	{
		public string UserName { get; private set; }

		public string Password { get; private set; }

		public string MachineId { get; private set; }

		public Request(string name, string password, string machineId = null)
		{
			UserName = name;
			Password = password;
			MachineId = machineId;
		}
	}
}

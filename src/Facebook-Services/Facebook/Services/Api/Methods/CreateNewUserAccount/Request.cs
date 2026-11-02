namespace Facebook.Services.Api.Methods.CreateNewUserAccount
{
	[Queue(QueueType.Web)]
	public class Request : FacebookServiceRequest
	{
		public string FirstName { get; private set; }

		public string LastName { get; private set; }

		public string PhoneNumber { get; private set; }

		public string DeviceId { get; private set; }

		public string MachineId { get; private set; }

		public Request(string firstName, string lastName, string phoneNumber, string deviceId, string machineId = null)
		{
			FirstName = firstName;
			LastName = lastName;
			PhoneNumber = phoneNumber;
			DeviceId = deviceId;
			MachineId = machineId;
		}
	}
}

using System.Collections.Generic;

namespace Facebook.Services.Api.Methods.SyncContacts;

[Queue(QueueType.Web)]
public class Request : FacebookServiceRequest
{
	public List<ContactChange> ContactChanges;
}

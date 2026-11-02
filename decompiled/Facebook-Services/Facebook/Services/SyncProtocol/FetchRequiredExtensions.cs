using System.Collections.Generic;

namespace Facebook.Services.SyncProtocol;

public static class FetchRequiredExtensions
{
	public static List<FetchRequired> ToList(this FetchRequired fetchRequired)
	{
		List<FetchRequired> list = new List<FetchRequired>();
		list.Add(fetchRequired);
		return list;
	}
}

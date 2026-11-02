using System;
using System.Net;

namespace Facebook.Models
{
	public static class MachineId
	{
		public static void AddCookie(CookieContainer cookies, string machineId)
		{
			cookies.Add(new Uri("http://facebook.com"), new Cookie("datr", machineId, "", ".facebook.com"));
		}
	}
}

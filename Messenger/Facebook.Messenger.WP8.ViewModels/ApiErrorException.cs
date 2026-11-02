using System;
using Facebook.Base;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class ApiErrorException : Exception
	{
		public Error Error { get; set; }

		public ApiErrorException(Error error)
		{
			Error = error;
		}
	}
}

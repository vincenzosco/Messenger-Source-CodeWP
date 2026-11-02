using System;

namespace Facebook.Base
{
	public class Error
	{
		private string _title;

		private string _message;

		public string Title
		{
			get
			{
				if (string.IsNullOrEmpty(_title))
				{
					return string.Empty;
				}
				return _title;
			}
			set
			{
				_title = value;
			}
		}

		public string Message
		{
			get
			{
				if (string.IsNullOrEmpty(_message))
				{
					return string.Empty;
				}
				return _message;
			}
			set
			{
				_message = value;
			}
		}

		public Exception Exception { get; set; }

		public long Code { get; set; }

		public static Error FromResponse(Response response, string defaultMessage = "", string defaultTitle = "")
		{
			Error error = new Error();
			error.Title = ((response != null && response.Error != null && !string.IsNullOrEmpty(response.Error.Title)) ? response.Error.Title : defaultTitle);
			error.Message = ((response != null && response.Error != null && !string.IsNullOrEmpty(response.Error.Message)) ? response.Error.Message : defaultMessage);
			error.Code = ((response != null && response.Error != null) ? response.Error.Code : 0);
			return error;
		}

		public static Error FromException(Exception e)
		{
			Error error = new Error();
			error.Message = e.Message;
			error.Exception = e;
			return error;
		}
	}
}

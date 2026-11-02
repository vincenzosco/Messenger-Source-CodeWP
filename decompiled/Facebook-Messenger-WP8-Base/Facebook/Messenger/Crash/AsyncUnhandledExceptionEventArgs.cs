using System;

namespace Facebook.Messenger.Crash;

public class AsyncUnhandledExceptionEventArgs
{
	public Exception Exception { get; private set; }

	public bool Handled { get; set; }

	public string Message
	{
		get
		{
			if (Exception == null)
			{
				return string.Empty;
			}
			return Exception.Message;
		}
	}

	public AsyncUnhandledExceptionEventArgs(Exception e)
	{
		Exception = e;
	}
}

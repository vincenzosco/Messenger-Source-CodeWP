using System;
using System.Threading;

namespace Facebook.Messenger.Crash
{
	public class ExceptionSynchronizationContext : SynchronizationContext
	{
		private readonly SynchronizationContext _context;

		public event EventHandler<AsyncUnhandledExceptionEventArgs> UnhandledException;

		public static void EnableAsyncExceptionHandling(EventHandler<AsyncUnhandledExceptionEventArgs> handler)
		{
			SynchronizationContext current = SynchronizationContext.Current;
			if (SynchronizationContext.Current != null)
			{
				ExceptionSynchronizationContext exceptionSynchronizationContext = current as ExceptionSynchronizationContext;
				if (exceptionSynchronizationContext == null)
				{
					exceptionSynchronizationContext = new ExceptionSynchronizationContext(current);
					SynchronizationContext.SetSynchronizationContext(exceptionSynchronizationContext);
				}
				exceptionSynchronizationContext.UnhandledException += handler;
			}
		}

		public ExceptionSynchronizationContext(SynchronizationContext context)
		{
			_context = context;
		}

		public override SynchronizationContext CreateCopy()
		{
			return _context.CreateCopy();
		}

		public override void OperationCompleted()
		{
			_context.OperationCompleted();
		}

		public override void OperationStarted()
		{
			_context.OperationStarted();
		}

		public override void Post(SendOrPostCallback d, object state)
		{
			SendOrPostCallback d2 = (object s) =>
			{
				try
				{
					d(s);
				}
				catch (Exception e)
				{
					if (UnhandledException == null)
					{
						throw;
					}
					AsyncUnhandledExceptionEventArgs e2 = new AsyncUnhandledExceptionEventArgs(e);
					UnhandledException(this, e2);
					if (!e2.Handled)
					{
						throw;
					}
				}
			};
			_context.Post(d2, state);
		}

		public override void Send(SendOrPostCallback d, object state)
		{
			_context.Send(d, state);
		}
	}
}

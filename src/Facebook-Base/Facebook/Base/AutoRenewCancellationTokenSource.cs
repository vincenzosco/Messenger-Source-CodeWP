using System.Threading;

namespace Facebook.Base
{
	public class AutoRenewCancellationTokenSource
	{
		private CancellationTokenSource _source;

		public CancellationToken Token => _source.Token;

		public bool IsCancellationRequested => _source.IsCancellationRequested;

		public AutoRenewCancellationTokenSource()
		{
			_source = new CancellationTokenSource();
		}

		public void CancelAndRenew()
		{
			_source.Cancel();
			_source = new CancellationTokenSource();
		}
	}
}

using System.Runtime.Serialization;
using System.Threading;
using Newtonsoft.Json;

namespace Facebook.Base
{
	[JsonObject(MemberSerialization.OptIn)]
	[DataContract]
	public class Request
	{
		private readonly CancellationTokenSource _tokenSource;

		public CancellationToken CancelToken { get; set; }

		public bool ExtendTimeout { get; set; }

		public Request()
		{
			_tokenSource = new CancellationTokenSource();
			CancelToken = _tokenSource.Token;
		}

		public void Cancel()
		{
			if (CancelToken == _tokenSource.Token)
			{
				_tokenSource.Cancel();
			}
		}
	}
}

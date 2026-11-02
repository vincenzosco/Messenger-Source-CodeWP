using System.Collections.Generic;

namespace Facebook.Base.BackgroundTransfer
{
	public interface IResponseInformation
	{
		IReadOnlyDictionary<string, string> ResponseHeaders { get; set; }

		string Error { get; set; }

		uint StatusCode { get; set; }
	}
}

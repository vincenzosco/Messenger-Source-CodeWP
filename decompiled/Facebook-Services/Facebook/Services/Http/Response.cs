using System.Text;
using Facebook.Base;

namespace Facebook.Services.Http;

public class Response : Facebook.Base.Response
{
	public long UploadSizeBytes { get; set; }

	public byte[] Data { get; set; }

	public long DownloadSizeBytes
	{
		get
		{
			if (Data == null)
			{
				return 0L;
			}
			return Data.Length;
		}
	}

	public string DataAsString()
	{
		if (Data == null)
		{
			return null;
		}
		return Encoding.UTF8.GetString(Data, 0, Data.Length);
	}
}

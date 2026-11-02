using System.Collections.Generic;
using System.Linq;
using System.Text;
using Facebook.Base;

namespace Facebook.Services.Http
{
	public class Request : Facebook.Base.Request
	{
		public class Param
		{
			public const string MimeTypeTextPlain = "text/plain";

			public const string MimeTypeBinary = "application/octet-stream";

			public string Name { get; private set; }

			public string Value { get; private set; }

			public string MimeType { get; private set; }

			public byte[] Buffer { get; private set; }

			public string FileName { get; private set; }

			public string FilePath { get; private set; }

			public Param(string name, string value)
				: this(name, Encoding.UTF8.GetBytes(value), "text/plain")
			{
				Value = value;
			}

			public Param(string name, byte[] data, string mimeType = "application/octet-stream")
			{
				Name = name;
				Buffer = data;
				MimeType = mimeType;
			}

			public Param(string name, string filePath, string mimeType, string fileName = "")
			{
				Name = name;
				FilePath = filePath;
				MimeType = mimeType;
				FileName = fileName;
			}
		}

		public Method Method { get; set; }

		public string Url { get; private set; }

		public Dictionary<string, string> Headers { get; private set; }

		public Dictionary<string, Param> Params { get; private set; }

		public Request(string url, Method method = Method.Get)
		{
			Method = method;
			Url = url;
			Headers = new Dictionary<string, string>();
			Params = new Dictionary<string, Param>();
		}

		public Request AddParam(string name, string value)
		{
			Params.Add(name, new Param(name, value));
			return this;
		}

		public Request AddParam(string name, byte[] data, string mimeType = "application/octet-stream")
		{
			Params.Add(name, new Param(name, data, mimeType));
			return this;
		}

		public Request AddParam(string name, string filePath, string mimeType, string fileName = "")
		{
			Params.Add(name, new Param(name, filePath, mimeType, fileName));
			return this;
		}

		public Request SetHttpMethod(Method method)
		{
			Method = method;
			return this;
		}

		public string ToPercentEscapedParameterString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<string, Param> item in Params.Where((KeyValuePair<string, Param> param) => param.Value != null))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append("&");
				}
				stringBuilder.Append(string.Format("{0}={1}", new object[2]
				{
					Facebook.Base.Url.Encode(item.Key),
					Facebook.Base.Url.Encode(item.Value.Value)
				}));
			}
			return stringBuilder.ToString();
		}
	}
}

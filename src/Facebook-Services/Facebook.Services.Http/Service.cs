using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Network;
using Facebook.Models;

namespace Facebook.Services.Http
{
	public class Service : Facebook.Base.Service
	{
		private readonly IFileAccess _fileAccess;

		private readonly INetworkStatus _networkStatus;

		private readonly HttpClient _httpClient;

		private readonly HttpClient _httpUploadClient;

		private readonly HttpClientHandler _httpClientHandler;

		private readonly HttpClientHandler _httpUploadClientHandler;

		private readonly TimeSpan _defaultTimeout = TimeSpan.FromSeconds(30.0);

		private readonly TimeSpan _extendedTimeout = TimeSpan.FromMinutes(5.0);

		public Service(FacebookContext context, IFileAccess access, INetworkStatus networkStatus, IUserAgentGenerator generator)
		{
			_fileAccess = access;
			_networkStatus = networkStatus;
			_httpClientHandler = new HttpClientHandler
			{
				AllowAutoRedirect = true,
				AutomaticDecompression = (DecompressionMethods.GZip | DecompressionMethods.Deflate)
			};
			_httpUploadClientHandler = new HttpClientHandler
			{
				AllowAutoRedirect = true,
				AutomaticDecompression = (DecompressionMethods.GZip | DecompressionMethods.Deflate)
			};
			_httpClient = new HttpClient(_httpClientHandler)
			{
				Timeout = _defaultTimeout
			};
			_httpUploadClient = new HttpClient(_httpUploadClientHandler)
			{
				Timeout = _extendedTimeout
			};
			string userAgent = generator.GetUserAgent();
			HttpRequestHeaders defaultRequestHeaders = _httpClient.DefaultRequestHeaders;
			defaultRequestHeaders.TryAddWithoutValidation("User-Agent", userAgent);
			defaultRequestHeaders = _httpUploadClient.DefaultRequestHeaders;
			defaultRequestHeaders.TryAddWithoutValidation("User-Agent", userAgent);
			if (!string.IsNullOrWhiteSpace(context.GlobalSettings.FacebookMachineId))
			{
				SetMachineId(context.GlobalSettings.FacebookMachineId);
			}
			Register<Request, Response>(CreateHttpTask);
		}

		public void SetMachineId(string machineId)
		{
			MachineId.AddCookie(_httpClientHandler.CookieContainer, machineId);
			MachineId.AddCookie(_httpUploadClientHandler.CookieContainer, machineId);
		}

		private HttpMethod GetHttpMethod(Request request)
		{
			switch (request.Method)
			{
			case Method.Get:
				return HttpMethod.Get;
			case Method.Post:
				return HttpMethod.Post;
			case Method.Put:
				return HttpMethod.Put;
			case Method.Delete:
				return HttpMethod.Delete;
			default:
				throw new ArgumentOutOfRangeException(request.Method.ToString());
			}
		}

		private async Task<Response> CreateHttpTask(Request request)
		{
			Response response = new Response();
			using (HttpRequestMessage message = new HttpRequestMessage(GetHttpMethod(request), GetHttpUrl(request)))
			{
				foreach (KeyValuePair<string, string> header in request.Headers)
				{
					message.Headers.TryAddWithoutValidation(header.Key, header.Value);
				}
				using (HttpContent content = await GetHttpContent(request))
				{
					message.Content = content;
					if (message.Content != null)
					{
						byte[] messageByteArray = await message.Content.ReadAsByteArrayAsync();
						if (messageByteArray != null)
						{
							response.UploadSizeBytes = messageByteArray.Length;
						}
					}
					HttpClient httpClient = (request.ExtendTimeout ? _httpUploadClient : _httpClient);
					using (HttpResponseMessage httpResponse = await httpClient.SendAsync(message, request.CancelToken))
					{
						response.Data = await httpResponse.Content.ReadAsByteArrayAsync();
						if (httpResponse.StatusCode == HttpStatusCode.NotFound && (response.Data == null || response.Data.Length == 0))
						{
							WebException e = new WebException(string.Empty, WebExceptionStatus.ConnectFailure);
							response.Error = Error.FromException(e);
						}
					}
				}
			}
			return response;
		}

		protected static Uri GetHttpUrl(Request request)
		{
			if (request.Method != Method.Get)
			{
				return new Uri(request.Url);
			}
			if (!request.Params.All((KeyValuePair<string, Request.Param> param) => param.Value.MimeType == "text/plain"))
			{
				throw new ArgumentException("Http Get request had arguments which were not string types");
			}
			string text = request.ToPercentEscapedParameterString();
			string uriString = string.Format("{0}?{1}", new object[2] { request.Url, text });
			return new Uri(uriString, UriKind.Absolute);
		}

		private async Task<HttpContent> GetHttpContent(Request request)
		{
			if (request.Method == Method.Get)
			{
				return null;
			}
			if (request.Params.All((KeyValuePair<string, Request.Param> keyValuePair9) => keyValuePair9.Value.MimeType == "text/plain"))
			{
				Dictionary<string, string> nameValueCollection = request.Params.ToDictionary((KeyValuePair<string, Request.Param> keyValuePair9) => keyValuePair9.Key, (KeyValuePair<string, Request.Param> keyValuePair9) => keyValuePair9.Value.Value);
				return new FormUrlEncodedContent(nameValueCollection);
			}
			MultipartFormDataContent multiContent = new MultipartFormDataContent();
			foreach (KeyValuePair<string, Request.Param> param in request.Params)
			{
				KeyValuePair<string, Request.Param> keyValuePair = param;
				HttpContent content;
				if (!string.IsNullOrWhiteSpace(keyValuePair.Value.FilePath))
				{
					IFileAccess fileAccess = _fileAccess;
					KeyValuePair<string, Request.Param> keyValuePair2 = param;
					content = new StreamContent(await fileAccess.GetFileStream(keyValuePair2.Value.FilePath));
				}
				else
				{
					KeyValuePair<string, Request.Param> keyValuePair3 = param;
					content = new ByteArrayContent(keyValuePair3.Value.Buffer);
				}
				HttpContentHeaders headers = content.Headers;
				KeyValuePair<string, Request.Param> keyValuePair4 = param;
				headers.Add("Content-Type", keyValuePair4.Value.MimeType);
				KeyValuePair<string, Request.Param> keyValuePair5 = param;
				if (keyValuePair5.Value.FileName != null)
				{
					HttpContent content2 = content;
					KeyValuePair<string, Request.Param> keyValuePair6 = param;
					string key = keyValuePair6.Key;
					KeyValuePair<string, Request.Param> keyValuePair7 = param;
					multiContent.Add(content2, key, keyValuePair7.Value.FileName);
				}
				else
				{
					HttpContent content3 = content;
					KeyValuePair<string, Request.Param> keyValuePair8 = param;
					multiContent.Add(content3, keyValuePair8.Key);
				}
			}
			return multiContent;
		}
	}
}

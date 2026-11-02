using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Services.Analytics.Logging;
using Facebook.Services.Http;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods
{
	public abstract class FacebookApiFilter<TRequest, TResponse> : Filter<TRequest, TResponse> where TRequest : Facebook.Base.Request where TResponse : Facebook.Base.Response
	{
		protected enum SecretType
		{
			LoginSecret,
			AppSecret
		}

		protected void PopulateBatchGraphRequest(Facebook.Services.Http.Request request, FacebookServiceRequest facebookServiceRequest, ICollection<Facebook.Services.Http.Request> requests)
		{
			AddCommonParams(facebookServiceRequest, request);
			request.Method = Method.Post;
			request.AddParam("method", "POST");
			request.CancelToken = facebookServiceRequest.CancelToken;
			List<GraphApiRequestObject> list = new List<GraphApiRequestObject>();
			foreach (Facebook.Services.Http.Request request2 in requests)
			{
				GraphApiRequestObject graphApiRequestObject = new GraphApiRequestObject();
				graphApiRequestObject.method = request2.Method.ToString();
				string relative_url = request2.Url.Replace(Context.Config.GraphApiUrl, "");
				graphApiRequestObject.relative_url = relative_url;
				foreach (KeyValuePair<string, Facebook.Services.Http.Request.Param> item in request2.Params)
				{
					if (item.Key.Equals("name"))
					{
						graphApiRequestObject.name = Url.Encode(item.Value.Value);
					}
					else if (item.Key.Equals("attached_files"))
					{
						graphApiRequestObject.attached_files = Url.Encode(item.Value.Value);
					}
					else if (item.Key.Equals("relative_url"))
					{
						graphApiRequestObject.relative_url = Url.Encode(item.Value.Value);
					}
					else if (item.Key.Equals("method"))
					{
						graphApiRequestObject.method = Url.Encode(item.Value.Value);
					}
					else if (item.Key.Equals("depends_on"))
					{
						graphApiRequestObject.depends_on = Url.Encode(item.Value.Value);
					}
					else if (item.Key.Equals("body"))
					{
						graphApiRequestObject.body = item.Value.Value;
					}
					else if (item.Key.Equals("omit_response_on_success"))
					{
						bool result;
						if (bool.TryParse(item.Value.Value, out result))
						{
							graphApiRequestObject.omit_response_on_success = result;
						}
					}
					else if (item.Key.Equals("q"))
					{
						graphApiRequestObject.relative_url += string.Format("?{0}={1}", new object[2]
						{
							item.Key,
							item.Value.Value
						});
					}
					else if (!item.Key.Equals("access_token"))
					{
						if (!string.IsNullOrWhiteSpace(graphApiRequestObject.body))
						{
							graphApiRequestObject.body += "&";
						}
						if (item.Key.Equals("source"))
						{
							graphApiRequestObject.body += string.Format("{0}={1}", new object[2]
							{
								Url.Encode(item.Key),
								Url.Encode(item.Value.FilePath)
							});
						}
						else
						{
							graphApiRequestObject.body += string.Format("{0}={1}", new object[2]
							{
								Url.Encode(item.Key),
								Url.Encode(item.Value.Value)
							});
						}
					}
				}
				list.Add(graphApiRequestObject);
			}
			string value = JsonConvert.SerializeObject(list, Formatting.None);
			request.AddParam("batch", value);
		}

		protected Facebook.Services.Http.Request CreateBatchGraphRequest(FacebookServiceRequest request, ICollection<Facebook.Services.Http.Request> requests)
		{
			Facebook.Services.Http.Request request2 = new Facebook.Services.Http.Request(Context.Config.GraphApiUrl);
			PopulateBatchGraphRequest(request2, request, requests);
			return request2;
		}

		protected Facebook.Services.Http.Request CreateGraphRequest(FacebookServiceRequest request, string path = "")
		{
			return CreateGraphRequest(Context, request, path);
		}

		protected Facebook.Services.Http.Request CreateGraphQLRequest(FacebookServiceRequest request, string graphQuery, IDictionary<string, string> graphQlParams = null)
		{
			Facebook.Services.Http.Request request2 = CreateGraphRequest(request, "graphql").AddParam("q", graphQuery);
			if (graphQlParams != null)
			{
				request2 = request2.AddParam("query_params", JsonConvert.SerializeObject(graphQlParams));
			}
			return request2;
		}

		public static Facebook.Services.Http.Request CreateGraphRequest(ServiceContext context, FacebookServiceRequest request, string path = "")
		{
			string url = string.Format("{0}/{1}", new object[2]
			{
				context.Config.GraphApiUrl,
				path
			});
			Facebook.Services.Http.Request request2 = new Facebook.Services.Http.Request(url);
			request2.CancelToken = request.CancelToken;
			Facebook.Services.Http.Request request3 = request2;
			AddCommonParams(request, request3);
			return request3;
		}

		public static BatchGraphResponse ParseBatchGraphResponse(Facebook.Services.Http.Response response, Type[] listTypesToParse = null)
		{
			if (response.HasError())
			{
				BatchGraphResponse batchGraphResponse = new BatchGraphResponse();
				batchGraphResponse.Error = response.Error;
				return batchGraphResponse;
			}
			JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
			jsonSerializerSettings.Converters.Add(new GraphResponseConverter(listTypesToParse));
			JsonSerializerSettings settings = jsonSerializerSettings;
			return JsonConvert.DeserializeObject<BatchGraphResponse>(response.DataAsString(), settings);
		}

		protected Facebook.Services.Http.Request CreateBatchRestRequest(FacebookServiceRequest request, Facebook.Services.Http.Request[] requests)
		{
			Facebook.Services.Http.Request request2 = new Facebook.Services.Http.Request(Context.Config.RestApiUrl);
			AddCommonParams(request, request2);
			request2.AddParam("method", "batch.run");
			request2.AddParam("api_key", Context.Config.ApplicationId);
			request2.AddParam("format", "JSON");
			request2.AddParam("v", "1.0");
			request2.CancelToken = request.CancelToken;
			StringWriter stringWriter = new StringWriter();
			JsonTextWriter jsonTextWriter = new JsonTextWriter(stringWriter);
			jsonTextWriter.WriteStartArray();
			foreach (Facebook.Services.Http.Request request3 in requests)
			{
				jsonTextWriter.WriteValue(request3.ToPercentEscapedParameterString());
			}
			jsonTextWriter.WriteEndArray();
			request2.AddParam("method_feed", stringWriter.ToString());
			return request2;
		}

		protected Facebook.Services.Http.Request CreateRestRequest(FacebookServiceRequest request, string method)
		{
			Facebook.Services.Http.Request request2 = new Facebook.Services.Http.Request(Context.Config.RestApiUrl);
			AddCommonParams(request, request2);
			request2.AddParam("method", method);
			request2.AddParam("api_key", Context.Config.ApplicationId);
			request2.AddParam("format", "JSON");
			request2.AddParam("v", "1.0");
			request2.CancelToken = request.CancelToken;
			return request2;
		}

		protected void AddRestSignature(Facebook.Services.Http.Request request, SecretType secretType = SecretType.LoginSecret)
		{
			if (request.Params.ContainsKey("access_token"))
			{
				return;
			}
			List<string> list = request.Params.Keys.ToList();
			list.Sort();
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string item in list)
			{
				Facebook.Services.Http.Request.Param param = request.Params[item];
				if (param.MimeType == "text/plain")
				{
					stringBuilder.AppendFormat("{0}={1}", new object[2] { item, param.Value });
				}
			}
			switch (secretType)
			{
			case SecretType.LoginSecret:
				stringBuilder.Append(Context.Config.LoginSecret);
				break;
			case SecretType.AppSecret:
				stringBuilder.Append(Context.Config.AppSecret);
				break;
			default:
				throw new Exception("Unhandled " + typeof(SecretType).Name);
			}
			string data = stringBuilder.ToString();
			string value = Context.Crypto.ToMD5HexString(data);
			request.AddParam("sig", value);
		}

		protected FacebookApiFilter(ServiceContext context)
			: base(context)
		{
		}

		private static void AddCommonParams(FacebookServiceRequest request, Facebook.Services.Http.Request http)
		{
			if (!string.IsNullOrWhiteSpace(request.AccessToken))
			{
				http.AddParam("access_token", request.AccessToken);
			}
			if (!string.IsNullOrWhiteSpace(request.Locale))
			{
				http.AddParam("locale", request.Locale);
			}
		}

		protected async Task<Facebook.Services.Http.Response> ExecuteHttpRequest(Facebook.Services.Http.Request request)
		{
			Facebook.Services.Http.Response response = await Context.HttpService.ExecuteAsync<Facebook.Services.Http.Request, Facebook.Services.Http.Response>(request);
			DataConsumptionLogger.LogDataEvent(Context.AnalyticsService, DataType.Api, response.DownloadSizeBytes, response.UploadSizeBytes);
			return response;
		}

		protected async Task<T> ExecuteHttpRequestAndParseResponse<T>(Facebook.Services.Http.Request request, JsonSerializerSettings settings = null) where T : Facebook.Base.Response, new()
		{
			return ParseResponse<T>(await ExecuteHttpRequest(request), settings);
		}

		protected T ParseResponse<T>(Facebook.Services.Http.Response response, JsonSerializerSettings settings = null) where T : Facebook.Base.Response, new()
		{
			if (response.HasError())
			{
				return new T
				{
					Error = response.Error
				};
			}
			if (response.Data == null || response.Data.Length == 0)
			{
				return new T
				{
					Error = Error.FromException(new NoDataResponseException())
				};
			}
			return JsonConvert.DeserializeObject<T>(response.DataAsString(), settings);
		}

		protected Task<TNewResponse> ExecuteApiRequest<TNewRequest, TNewResponse>(TNewRequest request) where TNewRequest : Facebook.Base.Request where TNewResponse : Facebook.Base.Response, new()
		{
			return Context.ApiService.ExecuteAsync<TNewRequest, TNewResponse>(request);
		}

		protected T ReturnError<T>(Facebook.Base.Response response) where T : Facebook.Base.Response, new()
		{
			return new T
			{
				Error = response.Error
			};
		}
	}
}

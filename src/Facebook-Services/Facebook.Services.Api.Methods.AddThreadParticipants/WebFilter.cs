using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Api.Fql;
using Facebook.Services.Common;
using Facebook.Services.Http;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Methods.AddThreadParticipants
{
	[Bundle(BundleType.Web, typeof(Request), typeof(Response))]
	public class WebFilter : FacebookApiFilter<Request, Response>
	{
		public WebFilter(ServiceContext context)
			: base(context)
		{
		}

		public override async Task<Response> ExecuteAsync(Request request, IHandler next)
		{
			Facebook.Services.Http.Request addParticipants = CreateGraphRequest(request, "participants").SetHttpMethod(Method.Post).AddParam("id", request.ThreadId).AddParam("to", CreateAddList(request.UserIds));
			Facebook.Services.Http.Request fetchParticipants = CreateGraphRequest(request, "fql").AddParam("q", UnifiedThread.GetUserById(request.UserIds));
			Facebook.Services.Http.Request query = CreateBatchGraphRequest(request, new Facebook.Services.Http.Request[2] { addParticipants, fetchParticipants });
			Facebook.Services.Http.Response httpResponse = await ExecuteHttpRequest(query);
			if (httpResponse.HasError())
			{
				return new Response
				{
					Succeeded = false,
					Error = httpResponse.Error
				};
			}
			BatchGraphResponse response = FacebookApiFilter<Request, Response>.ParseBatchGraphResponse(httpResponse, new Type[2]
			{
				typeof(BoolResponse),
				typeof(FetchParticipantsResponse)
			});
			if (response.HasError() || response.Responses.Count < 2 || !((BoolResponse)response.Responses[0].Body).Succeeded)
			{
				return new Response
				{
					Succeeded = false,
					Error = new Error()
				};
			}
			FetchParticipantsResponse fetchParticipantsResponse = (FetchParticipantsResponse)response.Responses[1].Body;
			return new Response
			{
				Succeeded = true,
				AddedParticipants = fetchParticipantsResponse.Participants
			};
		}

		private string CreateAddList(IEnumerable<string> ids)
		{
			StringWriter stringWriter = new StringWriter();
			JsonTextWriter jsonTextWriter = new JsonTextWriter(stringWriter);
			jsonTextWriter.WriteStartArray();
			foreach (string id in ids)
			{
				jsonTextWriter.WriteValue(id);
			}
			jsonTextWriter.WriteEndArray();
			return stringWriter.ToString();
		}
	}
}

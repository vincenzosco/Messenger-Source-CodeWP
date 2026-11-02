using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Http;

namespace Facebook.Services.Api.Methods.FetchContactsById;

[Bundle(BundleType.Web, typeof(Request), typeof(Response))]
public class WebFilter : FacebookApiFilter<Request, Response>
{
	private const string ContactQueryFieldsFormat = "nodes({0}){{added_time,id,graph_api_write_id,email_entries {{is_verified,primary_field}},phone_entries {{is_verified,primary_field {{label,phone {{country_code,display_number,national_number,universal_number}}}}}},phonetic_name{{parts{{offset,length,part}},text}},profile_picture.size({1},{1}).media_type(image/webp) as small_profile_picture{{uri}},profile_picture.size({2},{2}).media_type(image/webp) as large_profile_picture{{uri}},represented_profile{{id,url,friendship_status,is_mobile_pushable,is_messenger_user,is_partial,is_viewer_friend,messenger_version_max,name_search_tokens,is_memorialized,messenger_install_time,viewer_affinity.method(communication) as rank}},structured_name{{parts{{offset,length,part}},text}}}}";

	private const int SmallProfilePictureSize = 88;

	private const int LargeProfilePictureSize = 160;

	public WebFilter(ServiceContext context)
		: base(context)
	{
	}

	public override async Task<Response> ExecuteAsync(Request request, IHandler next)
	{
		string query = CreateFetchQuery(request.ContactIds);
		return await FetchContacts(request, query);
	}

	private static string CreateFetchQuery(IEnumerable<string> contactIds)
	{
		string text = string.Join(",", contactIds);
		return string.Format("nodes({0}){{added_time,id,graph_api_write_id,email_entries {{is_verified,primary_field}},phone_entries {{is_verified,primary_field {{label,phone {{country_code,display_number,national_number,universal_number}}}}}},phonetic_name{{parts{{offset,length,part}},text}},profile_picture.size({1},{1}).media_type(image/webp) as small_profile_picture{{uri}},profile_picture.size({2},{2}).media_type(image/webp) as large_profile_picture{{uri}},represented_profile{{id,url,friendship_status,is_mobile_pushable,is_messenger_user,is_partial,is_viewer_friend,messenger_version_max,name_search_tokens,is_memorialized,messenger_install_time,viewer_affinity.method(communication) as rank}},structured_name{{parts{{offset,length,part}},text}}}}", new object[3] { text, 88, 160 });
	}

	private async Task<Response> FetchContacts(Request request, string query)
	{
		Facebook.Services.Http.Request graphQlRequest = CreateGraphQLRequest(request, query);
		Facebook.Services.Http.Request httpRequest = CreateBatchGraphRequest(request, new Facebook.Services.Http.Request[1] { graphQlRequest });
		BatchGraphResponse response = FacebookApiFilter<Request, Response>.ParseBatchGraphResponse(await ExecuteHttpRequest(httpRequest), new Type[1] { typeof(Response) });
		if (response.HasError())
		{
			return new Response
			{
				Error = response.Error
			};
		}
		if (response.Responses == null || response.Responses.Count != 1)
		{
			Error error = new Error();
			error.Message = "Graph Batch API didn't return a valid number of responses";
			Error error2 = error;
			Response response2 = new Response();
			response2.Error = error2;
			return response2;
		}
		return response.Responses[0].Body as Response;
	}
}

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Models;
using Facebook.Services.Api.Attributes;
using Facebook.Services.Http;

namespace Facebook.Services.Api.Methods.SyncStickerPacks
{
	[Bundle(BundleType.Web, typeof(Request), typeof(Response))]
	public class WebFilter : FacebookApiFilter<Request, Response>
	{
		private const string OwnedPacksFields = "id";

		private const int NumberOfExpectedResponses = 3;

		private static readonly Type[] ResponseTypes = new Type[3]
		{
			typeof(StickerPacksResponse),
			typeof(StickerPacksResponse),
			typeof(StickerPacksResponse)
		};

		public WebFilter(ServiceContext context)
			: base(context)
		{
		}

		public override async Task<Response> ExecuteAsync(Request request, IHandler next)
		{
			string storePacksQuery = CreateFetchQuery(StickerPackType.Available, "id,name,artist,description,thumbnail_image,is_featured,is_promoted,copyrights,price,preview_image,tray_button{normal.scale(1)},stickers{nodes{id,thread_image.scale(1).media_type(image/png),composer_image.scale(2),animated_image.media_type(image/webp)}}");
			string trayPacksQuery = CreateFetchQuery(StickerPackType.Tray, "id,name,artist,description,thumbnail_image,is_featured,is_promoted,copyrights,price,preview_image,tray_button{normal.scale(1)},stickers{nodes{id,thread_image.scale(1).media_type(image/png),composer_image.scale(2),animated_image.media_type(image/webp)}}");
			string ownedPacksQuery = CreateFetchQuery(StickerPackType.Owned, "id");
			Facebook.Services.Http.Request storePacksRequest = CreateGraphQLRequest(request, storePacksQuery);
			Facebook.Services.Http.Request trayPacksRequest = CreateGraphQLRequest(request, trayPacksQuery);
			Facebook.Services.Http.Request ownedPacksRequest = CreateGraphQLRequest(request, ownedPacksQuery);
			Facebook.Services.Http.Request[] queries = new Facebook.Services.Http.Request[3] { storePacksRequest, trayPacksRequest, ownedPacksRequest };
			Facebook.Services.Http.Request httpRequest = CreateBatchGraphRequest(request, queries);
			Facebook.Services.Http.Response httpResponse = await ExecuteHttpRequest(httpRequest);
			if (httpResponse.HasError())
			{
				Response response = new Response();
				response.Error = httpResponse.Error;
				return response;
			}
			BatchGraphResponse batchResponse = FacebookApiFilter<Request, Response>.ParseBatchGraphResponse(httpResponse, ResponseTypes);
			Response response2 = CreateResponse(batchResponse);
			if (!response2.HasError() && !response2.TrayStickerPacks.Any())
			{
				await PatchResponseWithDefaultPacks(request, response2);
			}
			return response2;
		}

		private static string CreateFetchQuery(StickerPackType stickerPackType, string fields)
		{
			StringBuilder stringBuilder = new StringBuilder("viewer(){sticker_store{");
			string value;
			switch (stickerPackType)
			{
			case StickerPackType.Available:
				value = "available_packs";
				break;
			case StickerPackType.Tray:
				value = "tray_packs";
				break;
			case StickerPackType.Owned:
				value = "owned_packs";
				break;
			default:
				throw new InvalidOperationException();
			}
			stringBuilder.Append(value);
			stringBuilder.Append("{nodes{");
			stringBuilder.Append(fields);
			stringBuilder.Append("}}}}");
			return stringBuilder.ToString();
		}

		private static Response CreateResponse(BatchGraphResponse batchResponse)
		{
			if (batchResponse.HasError())
			{
				Response response = new Response();
				response.Error = batchResponse.Error;
				return response;
			}
			if (batchResponse.Responses == null || batchResponse.Responses.Count != 3)
			{
				int num = ((batchResponse.Responses != null) ? batchResponse.Responses.Count : 0);
				Error error = new Error();
				error.Message = string.Format("Graph Batch API didn't return valid number of responses. Expected {0}, received {1}", new object[2] { 3, num });
				Error error2 = error;
				Response response2 = new Response();
				response2.Error = error2;
				return response2;
			}
			StickerPacksResponse stickerPacksResponse = batchResponse.Responses[0].Body as StickerPacksResponse;
			StickerPacksResponse stickerPacksResponse2 = batchResponse.Responses[1].Body as StickerPacksResponse;
			StickerPacksResponse stickerPacksResponse3 = batchResponse.Responses[2].Body as StickerPacksResponse;
			if (stickerPacksResponse == null || stickerPacksResponse2 == null || stickerPacksResponse3 == null)
			{
				Error error3 = new Error();
				error3.Message = string.Format("Graph Batch API returned null responses. Store Packs null: {0}, Tray Packs null: {1}, Owned Packs null: {2}", new object[3]
				{
					stickerPacksResponse == null,
					stickerPacksResponse2 == null,
					stickerPacksResponse3 == null
				});
				Error error4 = error3;
				Response response3 = new Response();
				response3.Error = error4;
				return response3;
			}
			Response response4 = new Response();
			response4.AvailableStickerPacks = stickerPacksResponse.StickerPacks;
			response4.TrayStickerPacks = stickerPacksResponse2.StickerPacks;
			response4.OwnedStickerPacks = stickerPacksResponse3.StickerPacks;
			return response4;
		}

		private async Task PatchResponseWithDefaultPacks(Request request, Response response)
		{
			string defaultStickerPacksQuery = string.Format("nodes({0}){{{1}}}", new object[2]
			{
				string.Join(",", StickerPack.DefaultStickerPackIds),
				"id,name,artist,description,thumbnail_image,is_featured,is_promoted,copyrights,price,preview_image,tray_button{normal.scale(1)},stickers{nodes{id,thread_image.scale(1).media_type(image/png),composer_image.scale(2),animated_image.media_type(image/webp)}}"
			});
			Facebook.Services.Http.Request defaultStickerPacksRequest = CreateGraphQLRequest(request, defaultStickerPacksQuery);
			StickerPacksResponse defaultStickerPacksResponse = await ExecuteHttpRequestAndParseResponse<StickerPacksResponse>(defaultStickerPacksRequest);
			if (defaultStickerPacksResponse.HasError())
			{
				response.Error = defaultStickerPacksResponse.Error;
			}
			else
			{
				response.TrayStickerPacks = defaultStickerPacksResponse.StickerPacks;
			}
		}
	}
}

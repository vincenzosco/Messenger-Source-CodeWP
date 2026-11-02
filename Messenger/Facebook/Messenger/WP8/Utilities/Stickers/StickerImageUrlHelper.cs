using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Models;
using Facebook.Services.Api.Methods.FetchSticker;
using Facebook.Services.Stickers;

namespace Facebook.Messenger.WP8.Utilities.Stickers
{
	public class StickerImageUrlHelper
	{
		private readonly FacebookContext _context;

		private readonly Facebook.Services.Stickers.Service _stickerService;

		private readonly Dictionary<string, StickerImageUris> _stickerToUrlMap;

		private readonly DisposableMutex _lock = new DisposableMutex();

		public StickerImageUrlHelper(FacebookContext context, Facebook.Services.Stickers.Service stickerService)
		{
			_context = context;
			_stickerService = stickerService;
			_stickerToUrlMap = new Dictionary<string, StickerImageUris>();
		}

		public async Task Reset()
		{
			await _lock.WaitAsync();
			_stickerToUrlMap.Clear();
			_lock.Dispose();
		}

		public async Task<StickerImageUris> GetStickerImageUris(string stickerId, Uri defaultUri)
		{
			try
			{
				await _lock.WaitAsync();
				if (_stickerToUrlMap.ContainsKey(stickerId))
				{
					return _stickerToUrlMap[stickerId];
				}
				Facebook.Services.Api.Methods.FetchSticker.Request request = new Facebook.Services.Api.Methods.FetchSticker.Request
				{
					AccessToken = _context.Session.Credentials.AccessToken,
					StickerId = stickerId
				};
				Facebook.Services.Api.Methods.FetchSticker.Response response = await _stickerService.ExecuteAsync<Facebook.Services.Api.Methods.FetchSticker.Request, Facebook.Services.Api.Methods.FetchSticker.Response>(request);
				if (response.HasError() || response.Sticker == null)
				{
					StickerImageUris stickerImageUris = new StickerImageUris();
					stickerImageUris.StickerUri = defaultUri;
					return stickerImageUris;
				}
				Sticker sticker = response.Sticker;
				Uri stickerUri = ((sticker.ThreadImage != null) ? sticker.ThreadImage.Uri : defaultUri);
				Uri animatedStickerUri = ((sticker.AnimatedImage != null) ? sticker.AnimatedImage.Uri : null);
				StickerImageUris stickerImageUris2 = new StickerImageUris
				{
					StickerUri = stickerUri,
					AnimatedStickerUri = animatedStickerUri
				};
				_stickerToUrlMap.Add(stickerId, stickerImageUris2);
				return stickerImageUris2;
			}
			finally
			{
				_lock.Dispose();
			}
		}
	}
}

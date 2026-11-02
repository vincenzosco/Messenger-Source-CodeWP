using System;
using System.Threading;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Models;
using Facebook.Services.Api;
using Facebook.Services.Api.Methods.SetGlobalMute;
using Facebook.Services.Api.Methods.SetThreadMute;
using Facebook.Services.Common;

namespace Facebook.Messenger.WP8.Utilities
{
	public static class MuteHelper
	{
		private static bool _isMuteUpdating;

		private static CancellationTokenSource _tokenSource;

		public static bool IsThreadMuted(long muteUntil)
		{
			if (muteUntil != -1)
			{
				if (muteUntil > 0)
				{
					return muteUntil > DateTime.Now.ToUnixTimeSeconds();
				}
				return false;
			}
			return true;
		}

		public static bool IsMuted(FacebookContext context)
		{
			long num = DateTime.UtcNow.ToUnixTimeSeconds();
			long globalMuteDuration = context.GetGlobalMuteDuration();
			switch (globalMuteDuration)
			{
			default:
				return globalMuteDuration > num;
			case 0L:
				return false;
			case -1L:
				return true;
			}
		}

		public static bool IsMutedUntil8AM(FacebookContext context)
		{
			long num = DateTime.UtcNow.ToUnixTimeSeconds();
			return context.GetGlobalMuteDuration() - num > 3600;
		}

		public static async Task<BoolResponse> SetGlobalMuteState(MuteDuration muteDuration, FacebookContext context, Facebook.Services.Api.Service apiService)
		{
			CancellationTokenSource tokenSource = GetToken();
			_isMuteUpdating = true;
			long duration = DetermineDuration(muteDuration);
			Facebook.Services.Api.Methods.SetGlobalMute.Request request = new Facebook.Services.Api.Methods.SetGlobalMute.Request
			{
				AccessToken = context.Session.Credentials.AccessToken,
				MuteUntil = duration,
				CancelToken = tokenSource.Token
			};
			long oldDuration = context.GetGlobalMuteDuration();
			context.SetGlobalMuteDuration(duration);
			BoolResponse response = await apiService.ExecuteAsync<Facebook.Services.Api.Methods.SetGlobalMute.Request, BoolResponse>(request);
			if (response.HasError())
			{
				context.SetGlobalMuteDuration(oldDuration);
			}
			_isMuteUpdating = false;
			return response;
		}

		public static async Task<Response> SetThreadMuteState(MuteDuration muteDuration, FacebookContext context, Facebook.Services.Api.Service apiService, string threadId)
		{
			int num = default(int);
			int num2 = num;
			int num3 = 0;
			try
			{
				CancellationTokenSource tokenSource = GetToken();
				_isMuteUpdating = true;
				long duration = DetermineDuration(muteDuration);
				Facebook.Services.Api.Methods.SetThreadMute.Request request = new Facebook.Services.Api.Methods.SetThreadMute.Request
				{
					AccessToken = context.Session.Credentials.AccessToken,
					ThreadId = threadId,
					MuteUntil = duration,
					CancelToken = tokenSource.Token
				};
				return await apiService.ExecuteAsync<Facebook.Services.Api.Methods.SetThreadMute.Request, BoolResponse>(request);
			}
			finally
			{
				_isMuteUpdating = false;
			}
		}

		private static CancellationTokenSource GetToken()
		{
			if (_isMuteUpdating)
			{
				_tokenSource.Cancel();
				_tokenSource = null;
			}
			if (_tokenSource == null)
			{
				_tokenSource = new CancellationTokenSource();
			}
			return _tokenSource;
		}

		private static long DetermineDuration(MuteDuration muteDuration)
		{
			switch (muteDuration)
			{
			case MuteDuration.Off:
				return 0L;
			case MuteDuration.Forever:
				return -1L;
			case MuteDuration.Hour:
				return DetermineMuteDurationForOneHour();
			case MuteDuration.EightAM:
				return DetermineMuteDurationFor8Am();
			default:
				throw new NotImplementedException();
			}
		}

		private static long DetermineMuteDurationForOneHour()
		{
			return DateTime.UtcNow.AddHours(1.0).ToUnixTimeSeconds(false);
		}

		private static long DetermineMuteDurationFor8Am()
		{
			DateTime dateTime = DateTime.Now;
			if (dateTime.Hour >= 8)
			{
				dateTime = dateTime.AddDays(1.0);
			}
			DateTime date = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 8, 0, 0);
			return date.ToUnixTimeSeconds();
		}
	}
}

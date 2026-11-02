using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Extensions;
using Facebook.Base.Logging;
using Facebook.Models;
using Facebook.Models.Signals;
using Facebook.Services.Analytics.Logging;
using Facebook.Services.Api;
using Facebook.Services.Api.Cache;
using Facebook.Services.Api.Methods.SendAnalyticsEvents;
using Facebook.Services.Common;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.ServiceLocation;

namespace Facebook.Services.Analytics
{
	public class Service : BackgroundService
	{
		public class ForceUploadRequest : Facebook.Base.Request
		{
		}

		public class ForceUploadResponse : Facebook.Base.Response
		{
		}

		private const int EventCountMinimumLimitBeforeUploading = 50;

		private const int EventAgeMinimumLimitBeforeUploadingMs = 300000;

		private FacebookContext _context;

		private readonly IEnvironmentInformation _environmentInformation;

		private readonly AnalyticsCache _analyticsCache;

		private readonly DataUsageTracker _dataUsageTracker;

		private readonly ApplicationState _applicationState;

		private string _analyticsSessionId;

		protected override bool ShouldLogExecution => false;

		public Service(IDispatcher ui, IEnvironmentInformation environmentInformation, AnalyticsCache analyticsCache, ApplicationState applicationState)
			: base(ui)
		{
			_environmentInformation = environmentInformation;
			_analyticsCache = analyticsCache;
			_dataUsageTracker = new DataUsageTracker();
			_applicationState = applicationState;
			Register<ForceUploadRequest, ForceUploadResponse>(ForceUpload);
			Func<AddEventRequest, Task<BoolResponse>> creator = (AddEventRequest r) => AddEvent(r.Event);
			Register(creator);
			Register<DataEventRequest, BoolResponse>(LogData);
		}

		public Task<ForceUploadResponse> ForceUpload(ForceUploadRequest request)
		{
			MaybeUploadLogs(true).DoNotWait();
			return Task.FromResult(new ForceUploadResponse());
		}

		public Task<BoolResponse> LogData(DataEventRequest request)
		{
			_dataUsageTracker.LogData(request.DataType, request.DownloadedBytes, request.UploadedBytes);
			BoolResponse boolResponse = new BoolResponse();
			boolResponse.Succeeded = true;
			return Task.FromResult(boolResponse);
		}

		public async Task<BoolResponse> AddEvent(AnalyticsEvent e)
		{
			int num = default(int);
			int num2 = num;
			int num3 = 0;
			try
			{
				e.SessionId = _analyticsSessionId;
				await _analyticsCache.AddEvent(e);
				return new BoolResponse
				{
					Succeeded = true
				};
			}
			catch (Exception ex)
			{
				Logger.Error(this, ex.ToString(), "AddEvent", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Analytics\\Service.cs", 81);
				BoolResponse boolResponse = new BoolResponse();
				boolResponse.Succeeded = false;
				return boolResponse;
			}
		}

		private async Task MaybeUploadLogs(bool force = false)
		{
			IEnumerable<AnalyticsEvent> logs = await _analyticsCache.GetEvents(1000);
			if (ShouldUploadLogs(logs, force))
			{
				await SaveDataConsumptionEvents();
				await DeleteLogs(await UploadLogs(logs));
			}
		}

		private bool ShouldUploadLogs(IEnumerable<AnalyticsEvent> logs, bool force)
		{
			bool flag = _applicationState.CurrentRuntype == ApplicationState.Runtype.BackgroundSending;
			if (logs == null || !logs.Any() || flag)
			{
				return false;
			}
			long num = DateTime.Now.ToUnixTimeMilliseconds() - logs.Max((AnalyticsEvent e) => e.Time);
			if (!force && logs.Count() < 50)
			{
				return num >= 300000;
			}
			return true;
		}

		private async Task SaveDataConsumptionEvents()
		{
			IEnumerable<AnalyticsEvent> events = _dataUsageTracker.CreateEventsAndReset();
			await Task.WhenAll(events.Select(AddEvent));
		}

		private string GetVersion()
		{
			return _environmentInformation.GetApplicationVersionAsThreePartString();
		}

		private string GetDeviceId()
		{
			return _context.GlobalSettings.DeviceIdentifier;
		}

		private string GetUserId()
		{
			if (_context != null && _context.Session != null)
			{
				return _context.Session.UserId;
			}
			return string.Empty;
		}

		private async Task<int> UploadLogs(IEnumerable<AnalyticsEvent> logs)
		{
			string version = GetVersion();
			string deviceId = GetDeviceId();
			string userId = GetUserId();
			Facebook.Services.Api.Methods.SendAnalyticsEvents.Request request = new Facebook.Services.Api.Methods.SendAnalyticsEvents.Request
			{
				GeneratedTime = DateTime.Now.ToUnixTimeMilliseconds(),
				ApplicationVersion = version,
				BuildNumber = _environmentInformation.GetBuildNumber(),
				UserId = userId,
				ApplicationId = _environmentInformation.GetFacebookApplicationId(),
				DeviceId = deviceId,
				Carrier = _environmentInformation.GetNetworkDescription(),
				DeviceModelName = _environmentInformation.GetDeviceModelDescription(),
				OperatingSystemVersion = _environmentInformation.GetOperatingSystemDescription(),
				ConnectionType = _environmentInformation.GetNetworkConnectionType(),
				Data = logs
			};
			Facebook.Services.Api.Service apiService = ServiceLocator.Current.GetInstance<Facebook.Services.Api.Service>();
			return (await apiService.ExecuteAsync<Facebook.Services.Api.Methods.SendAnalyticsEvents.Request, Facebook.Services.Api.Methods.SendAnalyticsEvents.Response>(request)).LastUploadedEventLogId;
		}

		private async Task<BoolResponse> DeleteLogs(int lastUploadedLogId)
		{
			int num = default(int);
			int num2 = num;
			int num3 = 0;
			try
			{
				await _analyticsCache.DeleteEvents(lastUploadedLogId);
				return new BoolResponse
				{
					Succeeded = true
				};
			}
			catch (Exception ex)
			{
				Logger.Error(this, ex.ToString(), "DeleteLogs", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Analytics\\Service.cs", 170);
				BoolResponse boolResponse = new BoolResponse();
				boolResponse.Succeeded = false;
				return boolResponse;
			}
		}

		private void GenerateNewSession()
		{
			_analyticsSessionId = Guid.NewGuid().ToString();
		}

		public override void StartService(FacebookContext context)
		{
			_context = context;
			GenerateNewSession();
			Messenger.Default.Register(this, (LoginSignal s) =>
			{
				GenerateNewSession();
			});
			Messenger.Default.Register(this, (LogoutSignal s) =>
			{
				GenerateNewSession();
			});
			MaybeUploadLogs().DoNotWait();
		}

		public override void StopService()
		{
			_context = null;
			Messenger.Default.Unregister(this);
		}

		public override void PrepareStopService()
		{
			SaveDataConsumptionEvents().Wait();
		}
	}
}

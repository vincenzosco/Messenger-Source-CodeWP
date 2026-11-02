using System;
using Facebook.Base.Extensions;
using Facebook.Services.Common;

namespace Facebook.Services.Analytics.Logging
{
	public class PageNavigationLogger
	{
		public enum NuxStep
		{
			Login,
			LoginApproval,
			PhoneNumber,
			VerifyPhoneNumber,
			OnMessenger
		}

		public enum PageType
		{
			MainPage,
			ThreadView,
			Other
		}

		public const string NavigationStartKey = "navigation_start";

		public const string NavigationFinishedKey = "navigation_finish";

		public const string SessionStartedKey = "session_started";

		public const string PageKey = "page";

		public const string SessionKey = "session_id";

		public const string ColdNavigation = "cold_navigation";

		public const string OriginPage = "origin_page";

		private const string NavigationModule = "navigation";

		private static string _appSessionId;

		private static bool _coldNavigation;

		public static void LogApplicationStarted(Service service)
		{
			AddEventRequest request = new AddEventRequest("application_started", "navigation");
			service.ExecuteAsync<AddEventRequest, BoolResponse>(request).DoNotWait();
		}

		public static void LogApplicatoinResumed(Service service)
		{
			AddEventRequest request = new AddEventRequest("application_resumed", "navigation");
			service.ExecuteAsync<AddEventRequest, BoolResponse>(request).DoNotWait();
		}

		public static async void LogNuxStep(NuxStep nuxStep, Service analyticsService)
		{
			AddEventRequest e = new AddEventRequest("page_navigated", "navigation").AddData("page", nuxStep.ToString());
			await analyticsService.ExecuteAsync<AddEventRequest, BoolResponse>(e);
			Service.ForceUploadRequest request = new Service.ForceUploadRequest();
			analyticsService.ExecuteAsync<Service.ForceUploadRequest, Service.ForceUploadResponse>(request).DoNotWait();
		}

		public static void LogSessionStarted(bool isSessionPreserved, Service service)
		{
			_appSessionId = Guid.NewGuid().ToString();
			_coldNavigation = !isSessionPreserved;
			AddEventRequest request = new AddEventRequest("session_started", "navigation").AddData("session_id", _appSessionId).AddData("cold_navigation", _coldNavigation.ToString());
			service.ExecuteAsync<AddEventRequest, BoolResponse>(request);
		}

		public static void LogNavigationStart(string page, string originPage, Service service)
		{
			AddEventRequest request = new AddEventRequest("navigation_start", "navigation").AddData("page", page).AddData("session_id", _appSessionId).AddData("cold_navigation", _coldNavigation.ToString())
				.AddData("origin_page", originPage);
			service.ExecuteAsync<AddEventRequest, BoolResponse>(request);
		}

		public static void LogNavigationFinish(string page, Service service)
		{
			AddEventRequest request = new AddEventRequest("navigation_finish", "navigation").AddData("page", page).AddData("session_id", _appSessionId).AddData("cold_navigation", _coldNavigation.ToString());
			RemoveColdFlag();
			service.ExecuteAsync<AddEventRequest, BoolResponse>(request);
		}

		public static void RemoveColdFlag()
		{
			_coldNavigation = false;
		}
	}
}

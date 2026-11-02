using System;
using System.Collections.Generic;
using Facebook.Base;
using Facebook.Base.Network;
using Facebook.Messenger.Crash;
using Facebook.Messenger.Localization.Resources;
using Facebook.Messenger.WP8.Navigation;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Models;
using Facebook.Services.Api;
using Facebook.Services.Api.Methods.SendFlytrapReport;
using Facebook.Services.Common;
using GalaSoft.MvvmLight.Command;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class ReportProblemViewModel : FacebookMessengerViewModel
	{
		private readonly IEnvironmentInformation _environmentInformation;

		private string _reportText;

		private RelayCommand _showDataUsePolicyCommand;

		private bool _successfulSend;

		private RelayCommand _sendReport;

		public string ReportText
		{
			get
			{
				return _reportText;
			}
			set
			{
				if (!(_reportText == value))
				{
					_reportText = value;
					Context.Settings.SaveSetting("ReportText", _reportText);
					RaisePropertyChanged("ReportText");
				}
			}
		}

		public RelayCommand ShowDataUsePolicyCommand
		{
			get
			{
				return _showDataUsePolicyCommand ?? (_showDataUsePolicyCommand = new RelayCommand(ExecuteShowDataUsePolicyCommand));
			}
		}

		public bool SuccessfulSend
		{
			get
			{
				return _successfulSend;
			}
			set
			{
				Set(ref _successfulSend, value, false, "SuccessfulSend");
			}
		}

		public RelayCommand SendReportCommand
		{
			get
			{
				return _sendReport ?? (_sendReport = new RelayCommand(ExecuteSendReport));
			}
		}

		public ReportProblemViewModel(Facebook.Services.Api.Service service, FacebookContext context, Navigator navigator, IDispatcher dispatcher, INetworkStatus networkStatus, IEnvironmentInformation environmentInformation)
			: base(service, context, navigator, dispatcher, networkStatus)
		{
			Title = T("report-problem-title");
			_environmentInformation = environmentInformation;
			_reportText = Context.Settings.GetSetting<string>("ReportText") ?? string.Empty;
		}

		public override bool CanUpdate(Uri uri)
		{
			return UriPathMatches(uri);
		}

		private void ExecuteShowDataUsePolicyCommand()
		{
			Navigator.Navigate(FacebookAppDeepLinkUtil.GetWebViewDeepLinkUri(FacebookAppDeepLinkUtil.Page.DataUsePolicies));
		}

		private async void ExecuteSendReport()
		{
			if (IsLoading || string.IsNullOrWhiteSpace(ReportText))
			{
				return;
			}
			IsLoading = true;
			List<string> logs = App.ExceptionHelper.Log.LogMessages;
			logs.Reverse();
			string log = CrashSaver.CreateUserLog("app", ReportText, logs);
			Info info = new Info
			{
				GitHash = _environmentInformation.GetBuildGitHash(),
				GitBranch = _environmentInformation.GetBuildGitBranch(),
				BuildTime = _environmentInformation.GetBuildTime(),
				BuildNumber = _environmentInformation.GetBuildNumber(),
				OSVersion = _environmentInformation.GetOperatingSystemVersion(),
				Manufacturer = _environmentInformation.GetDeviceManufacturer(),
				Model = _environmentInformation.GetDeviceModelName(),
				Locale = string.Empty,
				Zombies = string.Empty
			};
			Facebook.Services.Api.Methods.SendFlytrapReport.Request request = new Facebook.Services.Api.Methods.SendFlytrapReport.Request
			{
				Description = ReportText,
				BuildNumber = _environmentInformation.GetBuildNumber(),
				Logs = log,
				Info = info
			};
			if (Context.IsAuthenticated())
			{
				request.AccessToken = Context.Session.Credentials.AccessToken;
			}
			BoolResponse response = await ApiService.ExecuteAsync<Facebook.Services.Api.Methods.SendFlytrapReport.Request, BoolResponse>(request);
			if (response.HasError() || !response.Succeeded)
			{
				Error error2;
				if (!HasConnection)
				{
					Error error = new Error();
					error.Message = StringLibrary.MessageBox_Label_NoInternet;
					error2 = error;
				}
				else
				{
					error2 = Error.FromResponse(response, StringLibrary.FlyTrap_Error_Generic);
				}
				Error error3 = error2;
				OnError(error3);
			}
			else
			{
				SuccessfulSend = true;
			}
			IsLoading = false;
		}
	}
}

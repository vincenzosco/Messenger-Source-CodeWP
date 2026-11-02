using System;
using Facebook.Base;
using Facebook.Models;
using Facebook.Services.Api;

namespace Facebook.Messenger.WP8.ViewModels
{
	public class UpgradePromptViewModel
	{
		private const string LastPromptTimeSettingKey = "AppUpgradeSetting_LastPromptTime";

		public readonly Uri StoreUri = new Uri("https://www.windowsphone.com/s?appid=3219d30d-4a23-4f58-a91c-c44b04e6a0c7");

		private readonly FacebookContext _context;

		private readonly UpgradePromptEnabledProvider _upgradePromptEnabledProvider;

		public UpgradePromptViewModel(FacebookContext context, UpgradePromptEnabledProvider upgradePromptEnabledProvider)
		{
			_context = context;
			_upgradePromptEnabledProvider = upgradePromptEnabledProvider;
		}

		public bool ShouldShowUpgradePrompt()
		{
			if (DebuggingUtility.IsDogfoodBuild)
			{
				return false;
			}
			if (!_upgradePromptEnabledProvider.Enabled)
			{
				return false;
			}
			DateTime setting = _context.Settings.GetSetting<DateTime>("AppUpgradeSetting_LastPromptTime");
			return setting.IsEmptyValue() || setting.TotalMillisecondsSinceNow() >= 604800000.0;
		}

		public void SetUpgradeNagShown()
		{
			_context.Settings.SaveSetting("AppUpgradeSetting_LastPromptTime", DateTime.Now);
		}
	}
}

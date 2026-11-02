using Facebook.Base;
using Facebook.Base.Network;
using Windows.Graphics.Display;

namespace Facebook.Messenger.Network
{
	public class UserAgentGenerator : IUserAgentGenerator
	{
		private readonly IEnvironmentInformation _env;

		private readonly ITranslator _translator;

		public UserAgentGenerator(IEnvironmentInformation env, ITranslator translator)
		{
			_env = env;
			_translator = translator;
		}

		public string GetUserAgent()
		{
			string systemInformation = GetSystemInformation();
			return string.Format("Facebook Messenger ({0}) for Windows Phone ({1})", _env.GetApplicationVersion(), systemInformation);
		}

		public string GetSystemInformation()
		{
			object[] args = new object[9]
			{
				_env.GetApplicationVersion(),
				_env.GetDeviceModelDescription(),
				GetScreenScale(),
				_env.GetOperatingSystemVersion(),
				_env.GetNetworkDescription(),
				_env.GetBuildNumber(),
				_env.GetDeviceModelName(),
				_env.GetDeviceManufacturer(),
				_translator.GetCurrentFBLocale()
			};
			return string.Format("FBAN/MSGR4WP;FBAV/{0};FBDV/{1};FBSS/{2};FBSN/WinPhone;FBSV/{3};FBCR/{4};FBBV/{5};FBMD/{6};FBMF/{7};FBLC/{8};", args);
		}

		private string GetScreenScale()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Invalid comparison between Unknown and I4
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Invalid comparison between Unknown and I4
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Invalid comparison between Unknown and I4
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Invalid comparison between Unknown and I4
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Invalid comparison between Unknown and I4
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Invalid comparison between Unknown and I4
			ResolutionScale resolutionScale = DisplayProperties.ResolutionScale;
			if ((int)resolutionScale <= 140)
			{
				if ((int)resolutionScale == 100)
				{
					return "1.0";
				}
				if ((int)resolutionScale == 140)
				{
					return "1.4";
				}
			}
			else
			{
				if ((int)resolutionScale == 150)
				{
					return "1.5";
				}
				if ((int)resolutionScale == 160)
				{
					return "1.6";
				}
				if ((int)resolutionScale == 180)
				{
					return "1.8";
				}
			}
			return "1.0";
		}
	}
}

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Facebook.Messenger.Localization.Resources
{
	[CompilerGenerated]
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[DebuggerNonUserCode]
	public class InternalStringsLibrary
	{
		private static ResourceManager resourceMan;

		private static CultureInfo resourceCulture;

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(resourceMan, null))
				{
					ResourceManager resourceManager = new ResourceManager("Facebook.Messenger.Localization.Resources.InternalStringsLibrary", typeof(InternalStringsLibrary).GetTypeInfo().Assembly);
					resourceMan = resourceManager;
				}
				return resourceMan;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return resourceCulture;
			}
			set
			{
				resourceCulture = value;
			}
		}

		public static string Crash_Dialog_Body => ResourceManager.GetString("Crash_Dialog_Body", resourceCulture);

		public static string Crash_Dialog_Existing_Body => ResourceManager.GetString("Crash_Dialog_Existing_Body", resourceCulture);

		public static string Crash_Dialog_Existing_Title => ResourceManager.GetString("Crash_Dialog_Existing_Title", resourceCulture);

		public static string Crash_Dialog_Ignore => ResourceManager.GetString("Crash_Dialog_Ignore", resourceCulture);

		public static string Crash_Dialog_Report => ResourceManager.GetString("Crash_Dialog_Report", resourceCulture);

		public static string Crash_Dialog_Title => ResourceManager.GetString("Crash_Dialog_Title", resourceCulture);

		public static string FeatureNotImplemented => ResourceManager.GetString("FeatureNotImplemented", resourceCulture);

		public static string Internal_Audio_Settings_Title => ResourceManager.GetString("Internal_Audio_Settings_Title", resourceCulture);

		public static string Internal_Details_Settings_Cache => ResourceManager.GetString("Internal_Details_Settings_Cache", resourceCulture);

		public static string Internal_Details_Settings_Debug => ResourceManager.GetString("Internal_Details_Settings_Debug", resourceCulture);

		public static string Internal_Details_Settings_Dogfood => ResourceManager.GetString("Internal_Details_Settings_Dogfood", resourceCulture);

		public static string Internal_Details_Settings_Employee => ResourceManager.GetString("Internal_Details_Settings_Employee", resourceCulture);

		public static string Internal_Details_Settings_Quality_Assurance => ResourceManager.GetString("Internal_Details_Settings_Quality_Assurance", resourceCulture);

		public static string Internal_Details_Settings_Title => ResourceManager.GetString("Internal_Details_Settings_Title", resourceCulture);

		public static string Internal_Network_Settings_Bandwidth => ResourceManager.GetString("Internal_Network_Settings_Bandwidth", resourceCulture);

		public static string Internal_Network_Settings_ConnectionState => ResourceManager.GetString("Internal_Network_Settings_ConnectionState", resourceCulture);

		public static string Internal_Network_Settings_InterfaceName => ResourceManager.GetString("Internal_Network_Settings_InterfaceName", resourceCulture);

		public static string Internal_Network_Settings_InterfaceType => ResourceManager.GetString("Internal_Network_Settings_InterfaceType", resourceCulture);

		public static string Internal_Network_Settings_Subtype => ResourceManager.GetString("Internal_Network_Settings_Subtype", resourceCulture);

		public static string Internal_Network_Settings_Title => ResourceManager.GetString("Internal_Network_Settings_Title", resourceCulture);

		public static string Internal_Settings_Audio_Copy => ResourceManager.GetString("Internal_Settings_Audio_Copy", resourceCulture);

		public static string Internal_Settings_Audio_Copy_Completed => ResourceManager.GetString("Internal_Settings_Audio_Copy_Completed", resourceCulture);

		public static string Internal_Settings_Audio_Play => ResourceManager.GetString("Internal_Settings_Audio_Play", resourceCulture);

		public static string Internal_Settings_Audio_Volume => ResourceManager.GetString("Internal_Settings_Audio_Volume", resourceCulture);

		public static string Internal_Settings_Audio_Volume_Error => ResourceManager.GetString("Internal_Settings_Audio_Volume_Error", resourceCulture);

		public static string Internal_Settings_Sync_ClearInboxCache => ResourceManager.GetString("Internal_Settings_Sync_ClearInboxCache", resourceCulture);

		public static string Internal_Settings_Sync_SimulateError => ResourceManager.GetString("Internal_Settings_Sync_SimulateError", resourceCulture);

		public static string Internal_Settings_Title => ResourceManager.GetString("Internal_Settings_Title", resourceCulture);

		public static string Internal_Storage_Settings_File_Options => ResourceManager.GetString("Internal_Storage_Settings_File_Options", resourceCulture);

		public static string Internal_Storage_Settings_File_Options_Report_Problem => ResourceManager.GetString("Internal_Storage_Settings_File_Options_Report_Problem", resourceCulture);

		public static string Internal_Storage_Settings_Title => ResourceManager.GetString("Internal_Storage_Settings_Title", resourceCulture);

		public static string Internal_Sync_Protocol_Title => ResourceManager.GetString("Internal_Sync_Protocol_Title", resourceCulture);

		public static string Rage_Shake_Body => ResourceManager.GetString("Rage_Shake_Body", resourceCulture);

		public static string Rage_Shake_Button_Cancel => ResourceManager.GetString("Rage_Shake_Button_Cancel", resourceCulture);

		public static string Rage_Shake_Button_Email => ResourceManager.GetString("Rage_Shake_Button_Email", resourceCulture);

		public static string Rage_Shake_Button_Message => ResourceManager.GetString("Rage_Shake_Button_Message", resourceCulture);

		public static string Rage_Shake_HintText => ResourceManager.GetString("Rage_Shake_HintText", resourceCulture);

		public static string Rage_Shake_Title => ResourceManager.GetString("Rage_Shake_Title", resourceCulture);

		public static string Settings_Crash => ResourceManager.GetString("Settings_Crash", resourceCulture);

		public static string Settings_Internal_Settings => ResourceManager.GetString("Settings_Internal_Settings", resourceCulture);

		public static string Settings_Logout => ResourceManager.GetString("Settings_Logout", resourceCulture);

		public static string Temporary_StickerStore => ResourceManager.GetString("Temporary_StickerStore", resourceCulture);

		internal InternalStringsLibrary()
		{
		}
	}
}

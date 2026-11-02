using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Logging;
using Facebook.Models;
using Microsoft.Practices.ServiceLocation;
using Newtonsoft.Json;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.Storage;

namespace Facebook.Messenger.Crash
{
	public static class CrashSaver
	{
		public enum ErrorLogType
		{
			StartupError,
			ShutdownError,
			BadChannelState,
			PushRegistrationError,
			PayloadFormatError,
			BackgroundSendingError,
			BeginStoppingServicesStep,
			ExecutePrepareStoppingServicesStep,
			ExecuteStoppingServicesStep,
			ExecuteBackgroundSendingStep
		}

		private const string CrashFolderName = "Crashes";

		private const int ApproxMaxLogOutputSize = 2097152;

		public static async Task<List<StorageFile>> GetPersistedCrashes()
		{
			List<StorageFile> crashes = null;
			try
			{
				StorageFolder local = ApplicationData.Current.LocalFolder;
				crashes = (await (await local.CreateFolderAsync("Crashes", (CreationCollisionOption)3)).GetFilesAsync()).ToList();
			}
			catch (Exception ex)
			{
				Logger.Error(ex.ToString(), "GetPersistedCrashes", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\Crash\\CrashSaver.cs", 58);
			}
			return crashes ?? new List<StorageFile>();
		}

		public static async Task DeleteExceptionFile(StorageFile file)
		{
			int num = default(int);
			int num2 = num;
			int num3 = 0;
			try
			{
				if (file != null)
				{
					await file.DeleteAsync((StorageDeleteOption)1);
				}
			}
			catch (Exception ex)
			{
				Logger.Error("Error deleting crash logs", "DeleteExceptionFile", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\Crash\\CrashSaver.cs", 72);
				Logger.Error(ex.ToString(), "DeleteExceptionFile", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\Crash\\CrashSaver.cs", 73);
			}
		}

		public static async Task DeleteExceptionFiles(IEnumerable<StorageFile> files)
		{
			foreach (StorageFile file in files)
			{
				await DeleteExceptionFile(file);
			}
		}

		private static string CreateLog(string process, bool observed, string reportType, IEnumerable<string> logEntries, Action<JsonTextWriter> more)
		{
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			FacebookContext instance = ServiceLocator.Current.GetInstance<FacebookContext>();
			IEnvironmentInformation instance2 = ServiceLocator.Current.GetInstance<IEnvironmentInformation>();
			StringWriter stringWriter = new StringWriter();
			JsonTextWriter jsonTextWriter = new JsonTextWriter(stringWriter);
			jsonTextWriter.WriteStartObject();
			string value = Guid.NewGuid().ToString();
			if (instance.IsAuthenticated())
			{
				jsonTextWriter.WriteProperty("uid", instance.Session.UserId);
			}
			jsonTextWriter.WriteProperty("id", value);
			jsonTextWriter.WriteProperty("process", process);
			jsonTextWriter.WriteProperty("device-id", instance.GlobalSettings.DeviceIdentifier);
			jsonTextWriter.WriteProperty("os-v", instance2.GetOperatingSystemVersion());
			jsonTextWriter.WriteProperty("model", instance2.GetDeviceModelDescription());
			jsonTextWriter.WriteProperty("observed", observed.ToString().ToLower());
			jsonTextWriter.WriteProperty("report-type", reportType);
			jsonTextWriter.WriteProperty("package-name", GetAppPackageName(instance2));
			jsonTextWriter.WriteProperty("client-timestamp", DateTime.UtcNow.ToUnixTimeMilliseconds().ToString());
			jsonTextWriter.WriteProperty("device-scale", ((object)DisplayProperties.ResolutionScale).ToString());
			jsonTextWriter.WriteProperty("app-version", instance2.GetApplicationVersionAsThreePartString());
			jsonTextWriter.WriteProperty("network-connections", instance2.GetInterfaceDetailsLogString());
			jsonTextWriter.WriteProperty("v", "1");
			jsonTextWriter.WritePropertyName("gatekeeper-states");
			jsonTextWriter.WriteStartArray();
			foreach (string gatekeeperState in GetGatekeeperStates(instance))
			{
				jsonTextWriter.WriteValue(gatekeeperState);
			}
			jsonTextWriter.WriteEndArray();
			more(jsonTextWriter);
			jsonTextWriter.WritePropertyName("log");
			jsonTextWriter.WriteStartArray();
			foreach (string logEntry in logEntries)
			{
				if (stringWriter.GetStringBuilder().Length <= 2097152)
				{
					jsonTextWriter.WriteValue(logEntry);
					continue;
				}
				break;
			}
			jsonTextWriter.WriteEndArray();
			jsonTextWriter.WriteEndObject();
			return stringWriter.ToString();
		}

		public static string CreateUserLog(string process, string report, IEnumerable<string> logs)
		{
			return CreateLog(process, true, "user", logs, (JsonTextWriter writer) =>
			{
				writer.WriteProperty("report", report);
			});
		}

		public static string CreateErrorLog(string process, ErrorLogType error, string message, IEnumerable<string> logs)
		{
			logs = logs.Concat(new string[1] { message });
			return CreateLog(process, true, "error_state", logs, (JsonTextWriter writer) =>
			{
				writer.WriteProperty("error_details", error.ToString());
			});
		}

		public static void SaveErrorLog(string process, ErrorLogType error, string message, IEnumerable<string> logs)
		{
			string message2 = CreateErrorLog(process, error, message, logs);
			WriteToStorageFile(message2);
		}

		public static Tuple<StorageFile, string> SaveLog(string process, Exception e, bool observed, IEnumerable<string> logs)
		{
			string text = CreateLog(process, observed, "exception", logs, (JsonTextWriter writer) =>
			{
				writer.WriteProperty("stacktrace", e.ToString());
			});
			StorageFile val = WriteToStorageFile(text);
			if (val != null)
			{
				return Tuple.Create<StorageFile, string>(val, text);
			}
			return null;
		}

		private static IEnumerable<string> GetGatekeeperStates(FacebookContext context)
		{
			IReadOnlyDictionary<string, bool> gatekeepers = context.GetGatekeepers();
			List<string> list = new List<string>();
			foreach (string key in gatekeepers.Keys)
			{
				bool flag = gatekeepers[key];
				string item = string.Format("{0} : {1}", key, flag ? "enabled" : "disabled");
				list.Add(item);
			}
			return list;
		}

		private static StorageFile WriteToStorageFile(string message)
		{
			try
			{
				StorageFolder localFolder = ApplicationData.Current.LocalFolder;
				StorageFolder val = ExecuteSynchronously<StorageFolder>(localFolder.CreateFolderAsync("Crashes", (CreationCollisionOption)3));
				string text = $"{DateTime.UtcNow.Ticks}.crash";
				StorageFile val2 = ExecuteSynchronously<StorageFile>(val.CreateFileAsync(text, (CreationCollisionOption)1));
				byte[] bytes = Encoding.UTF8.GetBytes(message);
				using (Stream stream = ExecuteSynchronously(((IStorageFile)(object)val2).OpenStreamForWriteAsync()))
				{
					stream.Write(bytes, 0, bytes.Length);
					stream.Flush();
				}
				return val2;
			}
			catch (Exception ex)
			{
				Logger.Error("Error saving crash log", "WriteToStorageFile", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\Crash\\CrashSaver.cs", 209);
				Logger.Error(ex.ToString(), "WriteToStorageFile", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\Crash\\CrashSaver.cs", 210);
			}
			return null;
		}

		private static string GetAppPackageName(IEnvironmentInformation env)
		{
			return env.GetApplicationPackageId();
		}

		private static T ExecuteSynchronously<T>(IAsyncOperation<T> op)
		{
			Task<T> task = op.AsTask<T>();
			task.Wait();
			return task.Result;
		}

		private static T ExecuteSynchronously<T>(Task<T> task)
		{
			task.Wait();
			return task.Result;
		}
	}
}

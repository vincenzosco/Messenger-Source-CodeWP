using System.Collections.Generic;
using System.Text;
using Facebook.Services.Api.Methods.SendMessage;
using Newtonsoft.Json;

namespace Facebook.Messenger.Utilities
{
	public class BackgroundRequestUtil
	{
		private const string OutgoingMessageRequestsFile = "OutgoingMessageRequestsFile.txt";

		private const string SuccessfulBackgroundResponseFile = "SuccessfulBackgroundResponseFile.txt";

		public static void SaveOutstandingRequestsInIsolatedStorage(List<Request> outstandingRequests)
		{
			OverwriteFileWithObject(outstandingRequests, "OutgoingMessageRequestsFile.txt");
		}

		public static List<Request> GetOutstandingRequestsFromIsolatedStorage()
		{
			return GetObjectFromFile<List<Request>>("OutgoingMessageRequestsFile.txt");
		}

		public static void DeleteOutstandingRequestsFromIsolatedStorage()
		{
			ErrorIgnoringStorageUtils.DeleteFileInIsolateStorage("OutgoingMessageRequestsFile.txt");
		}

		public static void SaveSuccessfulResponsesInIsolatedStorage(List<Response> successfulResponses)
		{
			OverwriteFileWithObject(successfulResponses, "SuccessfulBackgroundResponseFile.txt");
		}

		public static List<Response> GetSuccessfulResponsesFromIsolatedStorage()
		{
			return GetObjectFromFile<List<Response>>("SuccessfulBackgroundResponseFile.txt");
		}

		public static void DeleteSuccessfulResponsesFromIsolatedStorage()
		{
			ErrorIgnoringStorageUtils.DeleteFileInIsolateStorage("SuccessfulBackgroundResponseFile.txt");
		}

		private static void OverwriteFileWithObject(object objectToSave, string filename)
		{
			string s = JsonConvert.SerializeObject(objectToSave);
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			ErrorIgnoringStorageUtils.OverwriteFile(filename, bytes);
		}

		private static T GetObjectFromFile<T>(string filename)
		{
			byte[] array = ErrorIgnoringStorageUtils.ReadFile(filename);
			if (array == null)
			{
				return default(T);
			}
			string value = Encoding.UTF8.GetString(array, 0, array.Length);
			if (string.IsNullOrWhiteSpace(value))
			{
				return default(T);
			}
			return JsonConvert.DeserializeObject<T>(value);
		}
	}
}

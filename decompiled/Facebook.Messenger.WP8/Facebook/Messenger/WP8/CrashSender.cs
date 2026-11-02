using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Facebook.Base.Logging;
using Facebook.Messenger.Crash;
using Facebook.Messenger.WP8.Utilities;
using Facebook.Services.Api;
using Facebook.Services.Api.Methods.ReportCrash;
using Facebook.Services.Common;
using Microsoft.Practices.ServiceLocation;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Facebook.Messenger.WP8;

public class CrashSender
{
	private const string PivotalReportEmailAddress = "labs-fbmessenger@pivotal.io";

	private const string PivotalReportEmailSubject = "Messenger Crash Report";

	public static async Task PostToEmail(string inputText, string logs)
	{
		string subject = string.Format("{0} - {1}", "Messenger Crash Report", inputText);
		await EmailSender.AttemptEmail("labs-fbmessenger@pivotal.io", subject, string.Empty, logs);
	}

	public static async Task PostToEmail(string logs)
	{
		await PostToEmail(string.Empty, logs);
	}

	public static async Task<bool> PostToServer(string result, string accessToken = "")
	{
		Service api = ServiceLocator.Current.GetInstance<Service>();
		Request request = new Request
		{
			Payload = result,
			AccessToken = accessToken
		};
		BoolResponse response = await api.ExecuteAsync<Request, BoolResponse>(request);
		return !response.HasError() || response.Succeeded;
	}

	public static async Task<bool> PostToServerAndDeleteLogFile(string result, StorageFile file, string accessToken = "")
	{
		bool postedToServer = await PostToServer(result, accessToken);
		if (postedToServer)
		{
			await CrashSaver.DeleteExceptionFile(file);
		}
		return postedToServer;
	}

	public static async Task PostReportsToServer(IEnumerable<StorageFile> files, string accessToken = "")
	{
		foreach (StorageFile file in files)
		{
			try
			{
				string crash = await ReadTextAsync(file);
				if (!string.IsNullOrWhiteSpace(crash))
				{
					await PostToServerAndDeleteLogFile(crash, file, accessToken);
				}
				else
				{
					await CrashSaver.DeleteExceptionFile(file);
				}
			}
			catch (Exception ex)
			{
				string message = $"Couldn't post crash log: {file.Path}";
				Logger.Error(message, "PostReportsToServer", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\CrashSender.cs", 76);
				Logger.Error(ex.ToString(), "PostReportsToServer", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\CrashSender.cs", 77);
			}
		}
	}

	private static async Task<string> ReadTextAsync(StorageFile file)
	{
		int num = default;
		_ = num;
		_ = 0;
		try
		{
			IRandomAccessStreamWithContentType inputStream = await file.OpenReadAsync();
			try
			{
				using StreamReader streamReader = new StreamReader(((IInputStream)(object)inputStream).AsStreamForRead());
				return streamReader.ReadToEnd();
			}
			finally
			{
				((IDisposable)inputStream)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			Logger.Error("Error reading crash log", "ReadTextAsync", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\CrashSender.cs", 95);
			Logger.Error(ex.ToString(), "ReadTextAsync", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\CrashSender.cs", 96);
		}
		return null;
	}
}

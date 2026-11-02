using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Facebook.Base;
using Facebook.Base.Logging;
using Newtonsoft.Json;

namespace Facebook.Services.Api.Cache;

public static class SaveCacheUtil
{
	private const string SettingPrefix = "saved_cache_data_";

	public static async Task Save<T>(IUserDataStorage storage, string key, T data)
	{
		StopwatchLogger stopwatchLogger = StopwatchLogger.StartNew($"Save Cache: {typeof(T)}");
		try
		{
			int num = default;
			_ = num;
			_ = 0;
			try
			{
				StringBuilder stringBuilder = new StringBuilder();
				using StringWriter stringWriter = new StringWriter(stringBuilder);
				JsonSerializer serializer = new JsonSerializer();
				stopwatchLogger.LogStep($"About to serialize {typeof(T)}");
				serializer.Serialize(stringWriter, data);
				stopwatchLogger.LogStep($"Finished serializing {typeof(T)}");
				string dataAsString = stringBuilder.ToString();
				string settingKey = "saved_cache_data_" + key;
				stopwatchLogger.LogStep($"About to write {typeof(T)}");
				await storage.SetData(settingKey, dataAsString);
				stopwatchLogger.LogStep($"Finished writing {typeof(T)}");
			}
			catch (Exception ex)
			{
				Logger.Critical("Could not save cache: " + ex, "Save", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\SaveCacheUtil.cs", 38);
			}
		}
		finally
		{
			stopwatchLogger.End();
		}
	}

	public static async Task<T> Load<T>(IUserDataStorage storage, string key)
	{
		StopwatchLogger stopwatchLogger = StopwatchLogger.StartNew($"Load Cache: {typeof(T)}");
		try
		{
			string settingKey = "saved_cache_data_" + key;
			if (await storage.DataExists(settingKey))
			{
				string data = await storage.GetData(settingKey);
				stopwatchLogger.LogStep("Read");
				if (!string.IsNullOrWhiteSpace(data))
				{
					T result = JsonConvert.DeserializeObject<T>(data);
					stopwatchLogger.LogStep("Deserialized");
					return result;
				}
			}
		}
		catch (Exception ex)
		{
			Logger.Critical("Could not deserialize cache: " + ex, "Load", "c:\\Open\\messenger\\Facebook\\Facebook-Services\\Api\\Cache\\SaveCacheUtil.cs", 69);
		}
		finally
		{
			stopwatchLogger.End();
		}
		return default;
	}
}

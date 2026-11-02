using System.Collections.Generic;
using System.Diagnostics;
using Facebook.Base.Logging;

namespace Facebook.Base;

public class StopwatchLogger
{
	private const string Start = "START";

	private const string Finish = "FINISH";

	private readonly string _title;

	private readonly Stack<KeyValuePair<string, long>> _subtasks;

	private readonly Stopwatch _stopwatch;

	private StopwatchLogger(string title)
	{
		_title = title;
		_subtasks = new Stack<KeyValuePair<string, long>>();
		_stopwatch = Stopwatch.StartNew();
		Log("START");
	}

	public void End()
	{
		_stopwatch.Stop();
		Log("FINISH");
	}

	public void LogStep(string step)
	{
		Log(step);
	}

	public void LogSubtaskStart(string subtask)
	{
		_subtasks.Push(new KeyValuePair<string, long>(subtask, _stopwatch.ElapsedMilliseconds));
		Log(string.Format("{0} {1}", new object[2] { "START", subtask }));
	}

	public void LogSubtaskEnd()
	{
		KeyValuePair<string, long> keyValuePair = _subtasks.Pop();
		long num = _stopwatch.ElapsedMilliseconds - keyValuePair.Value;
		Log(string.Format("{0} {1} - subtask time taken {2}ms", new object[3] { "FINISH", keyValuePair.Key, num }));
	}

	private void Log(string message)
	{
		Logger.Info(string.Format("[{0}] [{1}ms] {2}", new object[3] { _title, _stopwatch.ElapsedMilliseconds, message }), "Log", "c:\\Open\\messenger\\Facebook\\Facebook-Base\\StopwatchLogger.cs", 50);
	}

	public static StopwatchLogger StartNew(string title)
	{
		return new StopwatchLogger(title);
	}
}

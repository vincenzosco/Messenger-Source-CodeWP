using System;
using System.Linq;
using System.Windows.Media;
using Facebook.Base.Logging;

namespace Facebook.Messenger.WP8.Controls.Debugging
{
	public class LogDisplayInfo
	{
		private static readonly SolidColorBrush CriticalColorBrush = new SolidColorBrush(Colors.Red);

		private static readonly SolidColorBrush ErrorColorBrush = new SolidColorBrush(Colors.Orange);

		private static readonly SolidColorBrush WarningColorBrush = new SolidColorBrush(Colors.Yellow);

		private static readonly SolidColorBrush DebugColorBrush = new SolidColorBrush(Colors.Green);

		private static readonly SolidColorBrush UnknownColorBrush = new SolidColorBrush(Colors.DarkGray);

		public readonly string Description;

		private readonly LogLevel _level;

		public SolidColorBrush ColorBrush => GetTitleDisplayColor(_level);

		public string Title => _level.ToString();

		public LogDisplayInfo(LogLevel level, Type type, string message)
		{
			_level = level;
			Description = FormatDescription(type, message);
		}

		private static string FormatDescription(Type type, string message)
		{
			string arg = string.Empty;
			if ((object)type != null)
			{
				string fullName = type.FullName;
				string arg2 = string.Join(".", fullName.Split('.').Reverse().Take(2));
				arg = $"Type: {arg2}\n({fullName})\n\n";
			}
			return $"{arg}Message:\n{message}";
		}

		private static SolidColorBrush GetTitleDisplayColor(LogLevel level)
		{
			switch (level)
			{
			case LogLevel.Critical:
				return CriticalColorBrush;
			case LogLevel.Error:
				return ErrorColorBrush;
			case LogLevel.Warning:
				return WarningColorBrush;
			case LogLevel.Debug:
				return DebugColorBrush;
			default:
				return UnknownColorBrush;
			}
		}

		static LogDisplayInfo()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Expected Obj, but got Unknown
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected Obj, but got Unknown
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Expected Obj, but got Unknown
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected Obj, but got Unknown
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Expected Obj, but got Unknown
		}
	}
}

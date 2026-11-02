using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Facebook.Messenger.WP8.Navigation
{
	public class UriMapper
	{
		private struct InternalMapping
		{
			public Regex Source;

			public string ResultTemplate;
		}

		private const string DoubleSlashReplacement = "$1/";

		private const RegexOptions DefaultRegexOptions = RegexOptions.Compiled | RegexOptions.CultureInvariant;

		private readonly List<InternalMapping> _mappings;

		private readonly Regex _variablePattern;

		private readonly Regex _doubleSlashPattern;

		public UriMapper()
		{
			_mappings = new List<InternalMapping>();
			_variablePattern = new Regex("\\{([^/]+?)\\}", RegexOptions.Compiled | RegexOptions.CultureInvariant);
			_doubleSlashPattern = new Regex("([^:])(\\/\\/+)", RegexOptions.Compiled | RegexOptions.CultureInvariant);
		}

		public string GetMapping(Uri uri)
		{
			return GetMapping(uri.ToString());
		}

		public string GetMapping(string uri)
		{
			if (string.IsNullOrWhiteSpace(uri))
			{
				throw new ArgumentException("When mapping a uri we need a non-null, non-empty string");
			}
			uri = _doubleSlashPattern.Replace(uri, "$1/");
			uri = uri.TrimEnd('/');
			foreach (InternalMapping mapping in _mappings)
			{
				MatchCollection matchCollection = mapping.Source.Matches(uri);
				if (matchCollection.Count == 0)
				{
					continue;
				}
				string[] groupNames = mapping.Source.GetGroupNames();
				Dictionary<string, string> dict = new Dictionary<string, string>();
				bool flag = false;
				for (int i = 0; i < groupNames.Length; i++)
				{
					if (string.Compare(groupNames[i], i.ToString(), StringComparison.InvariantCulture) != 0)
					{
						flag = true;
						dict[groupNames[i]] = matchCollection[0].Groups[i].Value;
					}
				}
				if (!flag && groupNames.Length == 2)
				{
					return mapping.ResultTemplate + matchCollection[0].Groups[1].Value;
				}
				return _variablePattern.Replace(mapping.ResultTemplate, (Match m) =>
				{
					string key = m.Groups[1].ToString();
					string value;
					dict.TryGetValue(key, out value);
					return value;
				});
			}
			return null;
		}

		public void Map(Uri fb, string page)
		{
			Map(fb.ToString(), page);
		}

		public void Map(string fb, string page)
		{
			HashSet<string> variableNames = GetVariableNames(fb);
			HashSet<string> variableNames2 = GetVariableNames(page);
			if (!variableNames.SetEquals(variableNames2))
			{
				string message = string.Format("The number of arguments differ {0} != {1}", string.Join(",", variableNames), string.Join(",", variableNames2));
				throw new ArgumentException(message);
			}
			InternalMapping item = new InternalMapping
			{
				Source = ConvertToRegex(fb),
				ResultTemplate = page
			};
			InternalMapping item2 = new InternalMapping
			{
				Source = ConvertToRegex(page),
				ResultTemplate = fb
			};
			_mappings.Add(item);
			_mappings.Add(item2);
		}

		private HashSet<string> GetVariableNames(string uri)
		{
			HashSet<string> hashSet = new HashSet<string>();
			MatchCollection matchCollection = _variablePattern.Matches(uri);
			foreach (Match item in matchCollection)
			{
				hashSet.Add(item.Groups[1].Value);
			}
			return hashSet;
		}

		private Regex ConvertToRegex(string value)
		{
			bool flag = value.Contains("?");
			value = _doubleSlashPattern.Replace(value, "$1/");
			value = value.TrimEnd('/');
			value = Regex.Escape(value);
			value = value.Replace("\\{", "{");
			value = _variablePattern.Replace(value, "(?<$1>[^/]+)");
			value = ((!flag) ? string.Format("^{0}/?(\\?(?:[^=&]+=[^=&]+&?)*)?$", value) : string.Format("^{0}$", value));
			return new Regex(value, RegexOptions.Compiled | RegexOptions.CultureInvariant);
		}
	}
}

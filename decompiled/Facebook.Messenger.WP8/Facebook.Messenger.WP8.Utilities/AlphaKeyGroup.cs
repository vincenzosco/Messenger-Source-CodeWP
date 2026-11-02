using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using Microsoft.Phone.Globalization;

namespace Facebook.Messenger.WP8.Utilities
{
	public class AlphaKeyGroup<T> : ObservableCollection<T>
	{
		public delegate string GetKeyDelegate(T item);

		public string Key { get; private set; }

		public AlphaKeyGroup(string key)
		{
			Key = key;
		}

		public AlphaKeyGroup(string key, IEnumerable<T> list)
			: base(list)
		{
			Key = key;
		}

		private static ObservableCollection<AlphaKeyGroup<T>> CreateGroups(SortedLocaleGrouping slg)
		{
			ObservableCollection<AlphaKeyGroup<T>> observableCollection = new ObservableCollection<AlphaKeyGroup<T>>();
			foreach (string groupDisplayName in slg.GroupDisplayNames)
			{
				if (groupDisplayName.Equals("..."))
				{
					observableCollection.Add(new AlphaKeyGroup<T>("\ud83c\udf10"));
				}
				else
				{
					observableCollection.Add(new AlphaKeyGroup<T>(groupDisplayName));
				}
			}
			return observableCollection;
		}

		public static ObservableCollection<AlphaKeyGroup<T>> CreateGroups(IEnumerable<T> items, CultureInfo ci, GetKeyDelegate getKey, bool sort, IEnumerable<T> prependList = null, string prependKey = null)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected Obj, but got Unknown
			if (items == null)
			{
				return null;
			}
			SortedLocaleGrouping val = new SortedLocaleGrouping(ci);
			ObservableCollection<AlphaKeyGroup<T>> observableCollection = CreateGroups(val);
			List<T> list = new List<T>(items);
			if (sort)
			{
				list.Sort((T c0, T c1) => ci.CompareInfo.Compare(getKey(c0), getKey(c1)));
			}
			foreach (T item in list)
			{
				int groupIndex = val.GetGroupIndex(getKey(item));
				if (groupIndex >= 0 && groupIndex < observableCollection.Count)
				{
					observableCollection[groupIndex].Add(item);
				}
			}
			if (prependList != null)
			{
				if (prependKey == null)
				{
					prependKey = "★";
				}
				AlphaKeyGroup<T> alphaKeyGroup = new AlphaKeyGroup<T>(prependKey);
				alphaKeyGroup.AddRange(prependList);
				observableCollection.Insert(0, alphaKeyGroup);
			}
			return observableCollection;
		}

		public void AddRange(IEnumerable<T> list)
		{
			foreach (T item in list)
			{
				Add(item);
			}
		}
	}
}

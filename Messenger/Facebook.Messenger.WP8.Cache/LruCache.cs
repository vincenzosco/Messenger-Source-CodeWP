using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Facebook.Messenger.WP8.Cache
{
	public class LruCache<K, V>
	{
		[DataContract]
		public struct Entry
		{
			[DataMember]
			public readonly K Key;

			[DataMember]
			public readonly V Obj;

			[DataMember]
			public readonly long Cost;

			public Entry(K key, V obj, long cost)
			{
				Key = key;
				Obj = obj;
				Cost = cost;
			}
		}

		public delegate void EvictionDelegate(V obj);

		private readonly long _maxCost;

		private long _currentCost;

		private readonly LinkedList<Entry> _cache = new LinkedList<Entry>();

		private readonly Dictionary<K, Entry> _map = new Dictionary<K, Entry>();

		public Entry[] SerializedState
		{
			get
			{
				return _cache.ToArray();
			}
		}

		public event EvictionDelegate ItemEvicted;

		public LruCache(long maxCost)
			: this(maxCost, (IEnumerable<Entry>)null)
		{
		}

		public LruCache(long maxCost, IEnumerable<Entry> savedState)
		{
			_maxCost = maxCost;
			if (savedState == null)
			{
				return;
			}
			foreach (Entry item in savedState.Reverse())
			{
				Add(item.Key, item.Obj, item.Cost);
			}
		}

		public void Add(K key, V obj, long cost)
		{
			if (cost <= _maxCost)
			{
				Entry value = new Entry(key, obj, cost);
				_map.Add(key, value);
				_cache.AddFirst(value);
				_currentCost += cost;
				TrimCacheIfNecessary();
			}
		}

		public void Remove(K key)
		{
			Entry value;
			if (_map.TryGetValue(key, out value))
			{
				RemoveEntry(value);
			}
		}

		private void RemoveEntry(Entry entry)
		{
			if (_cache.Contains(entry))
			{
				_cache.Remove(entry);
			}
			_currentCost -= entry.Cost;
			_map.Remove(entry.Key);
			EvictionDelegate itemEvicted = ItemEvicted;
			if (itemEvicted != null)
			{
				itemEvicted(entry.Obj);
			}
		}

		public bool Lookup(K key, out V obj)
		{
			Entry value;
			if (_map.TryGetValue(key, out value))
			{
				MarkItemAsUsed(value);
				obj = value.Obj;
				return true;
			}
			obj = default(V);
			return false;
		}

		private void MarkItemAsUsed(Entry entry)
		{
			_cache.Remove(entry);
			_cache.AddFirst(entry);
		}

		private void TrimCacheIfNecessary()
		{
			while (_currentCost > _maxCost)
			{
				Entry value = _cache.Last.Value;
				RemoveEntry(value);
			}
		}
	}
}

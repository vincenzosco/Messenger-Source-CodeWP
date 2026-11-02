using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Facebook.Messenger.WP8.Cache
{
	public class FetchingEntry<T> where T : class
	{
		private readonly object _targetsLock = new object();

		public Uri SourceUri { get; private set; }

		public int Hash { get; private set; }

		public List<WeakReference<T>> TargetControls { get; private set; }

		public CancellationToken CancelToken { get; private set; }

		public bool FailedToRetrieve { get; set; }

		public bool IsCancelled
		{
			get
			{
				return CancelToken.IsCancellationRequested;
			}
		}

		public bool AreTargetControlsStillAlive
		{
			get
			{
				lock (_targetsLock)
				{
					PruneTargets();
					return TargetControls.Count > 0;
				}
			}
		}

		public FetchingEntry(Uri uri, CancellationToken token)
		{
			SourceUri = uri;
			Hash = uri.GetHashCode();
			CancelToken = token;
			TargetControls = new List<WeakReference<T>>();
		}

		public int RemoveTargetElement(T element)
		{
			lock (_targetsLock)
			{
				PruneTargets(element);
				return TargetControls.Count;
			}
		}

		public void AddTargetElement(T element)
		{
			lock (_targetsLock)
			{
				TargetControls.Add(new WeakReference<T>(element));
			}
		}

		private void PruneTargets(T element = null)
		{
			TargetControls.RemoveAll((WeakReference<T> x) =>
			{
				T target;
				return !x.TryGetTarget(out target) || target == element;
			});
		}

		public List<T> GetActiveTargets()
		{
			lock (_targetsLock)
			{
				return (from x in TargetControls.Select((WeakReference<T> x) =>
					{
						T target = null;
						x.TryGetTarget(out target);
						return target;
					})
					where x != null
					select x).ToList();
			}
		}
	}
}

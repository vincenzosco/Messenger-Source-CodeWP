using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Facebook.Messenger.WP8.Controls.Base
{
	public class LayoutGenerator
	{
		private const int MaxStackCount = 10;

		private readonly Dictionary<Type, Stack<object>> _queue;

		public LayoutGenerator()
		{
			_queue = new Dictionary<Type, Stack<object>>();
		}

		private T Dequeue<T>() where T : new()
		{
			Type typeFromHandle = typeof(T);
			Stack<object> value;
			if (_queue.TryGetValue(typeFromHandle, out value) && value.Any())
			{
				return (T)value.Pop();
			}
			return new T();
		}

		private void Enqueue<T>(T item)
		{
			Type type = item.GetType();
			Stack<object> value;
			if (!_queue.TryGetValue(type, out value))
			{
				value = new Stack<object>();
				_queue[type] = value;
			}
			if (value.Count < 10)
			{
				_queue[type].Push(item);
			}
		}

		public void Reset<T>(bool recycled, ContentPresenter presenter, Action<T> onDetach = null) where T : class, IResetableView, new()
		{
			((UIElement)presenter).Visibility = (Visibility)1;
			T val = (T)presenter.Content;
			if (val == null)
			{
				return;
			}
			if (recycled)
			{
				presenter.Content = null;
			}
			val.ResetView(recycled);
			if (recycled)
			{
				if (onDetach != null)
				{
					onDetach(val);
				}
				Enqueue(val);
			}
		}

		public T Load<T>(ContentPresenter presenter, Action<T> onUpdate, Action<T> onAttach = null) where T : new()
		{
			T val;
			if (presenter.Content == null)
			{
				val = Dequeue<T>();
				if (onAttach != null)
				{
					onAttach(val);
				}
				presenter.Content = val;
			}
			else
			{
				val = (T)presenter.Content;
			}
			onUpdate(val);
			((UIElement)presenter).Visibility = (Visibility)0;
			return val;
		}
	}
}

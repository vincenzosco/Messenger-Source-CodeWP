using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Facebook.Messenger.WP8.Controls.Base;

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
		if (_queue.TryGetValue(typeFromHandle, out var value) && value.Any())
		{
			return (T)value.Pop();
		}
		return new T();
	}

	private void Enqueue<T>(T item)
	{
		Type type = item.GetType();
		if (!_queue.TryGetValue(type, out var value))
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
		if (val != null)
		{
			if (recycled)
			{
				presenter.Content = null;
			}
			val.ResetView(recycled);
			if (recycled)
			{
				onDetach?.Invoke(val);
				Enqueue(val);
			}
		}
	}

	public T Load<T>(ContentPresenter presenter, Action<T> onUpdate, Action<T> onAttach = null) where T : new()
	{
		T val;
		if (presenter.Content == null)
		{
			val = Dequeue<T>();
			onAttach?.Invoke(val);
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

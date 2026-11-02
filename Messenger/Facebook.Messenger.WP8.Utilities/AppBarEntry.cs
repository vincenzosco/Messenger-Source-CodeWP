using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Phone.Shell;

namespace Facebook.Messenger.WP8.Utilities
{
	public class AppBarEntry<TStateType, TControlType> : AppBarEntryBase where TStateType : struct where TControlType : IApplicationBarMenuItem
	{
		private readonly Dictionary<TStateType, StateValues> _stateValues;

		private readonly AppBarHelper _helper;

		private readonly Action<AppBarEntry<TStateType, TControlType>, StateValues> _action;

		private TStateType? _state;

		private bool _isVisible = true;

		protected TControlType Item { get; private set; }

		public TStateType State
		{
			[MethodImpl(MethodImplOptions.NoOptimization)]
			get
			{
				return _state ?? default(TStateType);
			}
			[MethodImpl(MethodImplOptions.NoOptimization)]
			set
			{
				if (!_state.HasValue || !value.Equals(_state))
				{
					_state = value;
					StateValues arg = _stateValues[value];
					_action(this, arg);
				}
			}
		}

		public bool IsEnabled
		{
			get
			{
				return ((IApplicationBarMenuItem)Item).IsEnabled;
			}
			set
			{
				foreach (KeyValuePair<TStateType, StateValues> stateValue in _stateValues)
				{
					stateValue.Value.IsEnabled = value;
				}
				TControlType item = Item;
				((IApplicationBarMenuItem)item).IsEnabled = value;
			}
		}

		public IReadOnlyDictionary<TStateType, StateValues> StateValues
		{
			get
			{
				return _stateValues;
			}
		}

		public EventHandler ActiveHandler
		{
			get
			{
				return _stateValues[_state.Value].EventHandler;
			}
		}

		public bool IsVisible
		{
			get
			{
				return _isVisible;
			}
			set
			{
				if (value != _isVisible)
				{
					_isVisible = value;
					_helper.UpdateVisibility(Item, Position, value);
				}
			}
		}

		public void ResetCurrentState()
		{
			TStateType key = _state ?? default(TStateType);
			StateValues arg = _stateValues[key];
			_action(this, arg);
		}

		public AppBarEntry(AppBarHelper helper, TControlType item, Action<AppBarEntry<TStateType, TControlType>, StateValues> action)
		{
			_helper = helper;
			Item = item;
			_action = action;
			_stateValues = new Dictionary<TStateType, StateValues>();
		}

		public void AddState(TStateType state, string text, EventHandler onClick, Uri iconUri, bool enabled)
		{
			_stateValues[state] = new StateValues(text, iconUri, onClick, enabled);
		}
	}
}

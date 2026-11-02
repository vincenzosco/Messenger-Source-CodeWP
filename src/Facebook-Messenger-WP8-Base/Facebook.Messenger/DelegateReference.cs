using System;
using System.Reflection;

namespace Facebook.Messenger
{
	public class DelegateReference : IDelegateReference
	{
		private readonly Delegate _delegate;

		private readonly WeakReference _weakReference;

		private readonly MethodInfo _method;

		private readonly Type _delegateType;

		public Delegate Target
		{
			get
			{
				if ((object)_delegate != null)
				{
					return _delegate;
				}
				return TryGetDelegate();
			}
		}

		public DelegateReference(Delegate @delegate, bool keepReferenceAlive)
		{
			if ((object)@delegate == null)
			{
				throw new ArgumentNullException("delegate");
			}
			if (keepReferenceAlive)
			{
				_delegate = @delegate;
				return;
			}
			_weakReference = new WeakReference(@delegate.Target);
			_method = @delegate.GetMethodInfo();
			_delegateType = @delegate.GetType();
		}

		private Delegate TryGetDelegate()
		{
			if (_method.IsStatic)
			{
				return _method.CreateDelegate(_delegateType, null);
			}
			object target = _weakReference.Target;
			if (target != null)
			{
				return _method.CreateDelegate(_delegateType, target);
			}
			return null;
		}
	}
}

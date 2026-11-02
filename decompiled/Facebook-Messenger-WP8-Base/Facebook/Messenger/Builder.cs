using System;

namespace Facebook.Messenger;

internal class Builder<T> : IBuilder<T>
{
	private readonly Lazy<T> _singleton;

	private readonly Func<T> _builder;

	private readonly bool _isSingleton;

	public Builder(Func<T> builder, bool singleton = false)
	{
		_builder = builder;
		_isSingleton = singleton;
		if (_isSingleton)
		{
			_singleton = new Lazy<T>(builder, isThreadSafe: true);
		}
	}

	public T Get()
	{
		if (!_isSingleton)
		{
			return _builder();
		}
		return _singleton.Value;
	}
}

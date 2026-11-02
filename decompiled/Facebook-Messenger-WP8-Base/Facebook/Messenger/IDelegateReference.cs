using System;

namespace Facebook.Messenger;

public interface IDelegateReference
{
	Delegate Target { get; }
}

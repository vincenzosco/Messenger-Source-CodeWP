using System.Runtime.CompilerServices;
using Windows.Foundation;

namespace Facebook.Messenger.Extensions;

public static class AsyncOperationExtensions
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void DoNotWait<T>(this IAsyncOperation<T> op)
	{
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void DoNotWait(this IAsyncAction op)
	{
	}
}

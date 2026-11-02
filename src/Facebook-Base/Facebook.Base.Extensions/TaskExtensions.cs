using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Facebook.Base.Extensions
{
	public static class TaskExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DoNotWait(this Task task)
		{
		}

		public static async Task<bool> RunAndDetermineIfCancelled(this Task task)
		{
			int num = default(int);
			int num2 = num;
			int num3 = 0;
			try
			{
				await task;
			}
			catch (TaskCanceledException)
			{
				return true;
			}
			return task.IsCanceled;
		}
	}
}

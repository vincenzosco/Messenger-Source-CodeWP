namespace Facebook.Base.Strategy
{
	public class StrategyResponse<TReturnValue>
	{
		public readonly ExecutionResult ExecutionResult;

		public readonly TReturnValue ReturnValue;

		public StrategyResponse(ExecutionResult executionResult, TReturnValue returnValue)
		{
			ExecutionResult = executionResult;
			ReturnValue = returnValue;
		}
	}
	public class StrategyResponse
	{
		public static StrategyResponse<T> Create<T>(ExecutionResult executionResult, T response)
		{
			return new StrategyResponse<T>(executionResult, response);
		}

		public static StrategyResponse<NullResponse> Create(ExecutionResult executionResult)
		{
			return Create(executionResult, NullResponse.Null);
		}
	}
}

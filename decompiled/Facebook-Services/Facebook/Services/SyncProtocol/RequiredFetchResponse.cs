using Facebook.Base.Strategy;

namespace Facebook.Services.SyncProtocol;

public class RequiredFetchResponse
{
	public readonly ExecutionResult MessageExecutionResult;

	public readonly ExecutionResult ThreadExecutionResult;

	public readonly bool SkipHandle;

	public RequiredFetchResponse(ExecutionResult messageExecutionResult, ExecutionResult threadExecutionResult, bool skipHandle)
	{
		MessageExecutionResult = messageExecutionResult;
		ThreadExecutionResult = threadExecutionResult;
		SkipHandle = skipHandle;
	}
}

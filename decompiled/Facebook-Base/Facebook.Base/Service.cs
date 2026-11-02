using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Facebook.Base.Logging;

namespace Facebook.Base
{
	public abstract class Service
	{
		private readonly Dictionary<string, object> _creatorMap = new Dictionary<string, object>();

		protected virtual bool ShouldLogExecution
		{
			get
			{
				return true;
			}
		}

		private string GetHash<TRequest, TResponse>()
		{
			Type typeFromHandle = typeof(TRequest);
			Type typeFromHandle2 = typeof(TResponse);
			return string.Format("{0}_{1}", new object[2]
			{
				typeFromHandle.GetHashCode(),
				typeFromHandle2.GetHashCode()
			});
		}

		protected void Register<TRequest, TResponse>(Func<TRequest, Task<TResponse>> creator)
		{
			_creatorMap[GetHash<TRequest, TResponse>()] = creator;
		}

		protected virtual Task<TResponse> CreateTask<TRequest, TResponse>(TRequest request)
		{
			string hash = GetHash<TRequest, TResponse>();
			if (_creatorMap.ContainsKey(hash))
			{
				Func<TRequest, Task<TResponse>> func = (Func<TRequest, Task<TResponse>>)_creatorMap[hash];
				return func(request);
			}
			throw new Exception(string.Format("Unsupported request {0} and response {1}", new object[2]
			{
				typeof(TRequest),
				typeof(TResponse)
			}));
		}

		protected virtual Task<TResponse> InnerExecuteAsync<TRequest, TResponse>(TRequest request) where TRequest : Request where TResponse : Response
		{
			return CreateTask<TRequest, TResponse>(request);
		}

		protected void LogExecutionStart(Type requestType, Type responseType)
		{
			Logger.Info(this, string.Format("Starting execution of request {0}, {1}", new object[2] { requestType, responseType }), "LogExecutionStart", "c:\\Open\\messenger\\Facebook\\Facebook-Base\\Service.cs", 156);
		}

		protected void LogExecutionEnd(Type requestType, Type responseType, long elapsedTime)
		{
			Logger.Info(this, string.Format("Finished execution of request {0}, {1} in {2}ms", new object[3] { requestType, responseType, elapsedTime }), "LogExecutionEnd", "c:\\Open\\messenger\\Facebook\\Facebook-Base\\Service.cs", 160);
		}

		public async Task<TResponse> ExecuteAsync<TRequest, TResponse>(TRequest request) where TRequest : Request where TResponse : Response, new()
		{
			if (request.CancelToken.IsCancellationRequested)
			{
				return CancelResponse<TResponse>();
			}
			return await Task.Run(async () =>
			{
				int num = default(int);
				int num2 = num;
				int num3 = 0;
				try
				{
					Stopwatch timing = (ShouldLogExecution ? Stopwatch.StartNew() : null);
					if (timing != null)
					{
						LogExecutionStart(typeof(TRequest), typeof(TResponse));
					}
					TResponse result = await InnerExecuteAsync<TRequest, TResponse>(request);
					if (timing != null)
					{
						LogExecutionEnd(typeof(TRequest), typeof(TResponse), timing.ElapsedMilliseconds);
					}
					return result;
				}
				catch (TaskCanceledException ex)
				{
					Logger.Warning(this, string.Format("Executing request {0}, {1} failed due to task canceled exception\n{2}", new object[3]
					{
						typeof(TRequest),
						typeof(TResponse),
						ex
					}), "ExecuteAsync", "c:\\Open\\messenger\\Facebook\\Facebook-Base\\Service.cs", 194);
					return new TResponse
					{
						Error = Error.FromException(ex)
					};
				}
				catch (Exception ex2)
				{
					Logger.Error(this, string.Format("Executing request {0}, {1} failed due to exception\n{2}", new object[3]
					{
						typeof(TRequest),
						typeof(TResponse),
						ex2
					}), "ExecuteAsync", "c:\\Open\\messenger\\Facebook\\Facebook-Base\\Service.cs", 199);
					return new TResponse
					{
						Error = Error.FromException(ex2)
					};
				}
			});
		}

		private static TResponse CancelResponse<TResponse>() where TResponse : Response, new()
		{
			return new TResponse
			{
				Error = new Error
				{
					Message = "Request was cancelled"
				}
			};
		}
	}
}

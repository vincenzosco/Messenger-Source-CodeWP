using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Facebook.Services.Api.Methods.SendMessage;

namespace Facebook.Services.MessageSender;

public class SendContext
{
	private readonly object _lock = new object();

	private readonly SemaphoreSlim _sendingSemaphore;

	private readonly SemaphoreSlim _savingSemaphore;

	private readonly List<Request> _requests;

	private string _localThreadIdOverride;

	private string _serverThreadIdOverride;

	public string Id { get; set; }

	public bool IdIsLocal { get; set; }

	public bool IsSending { get; set; }

	public SendContext(string id, bool isThreadIdLocal)
	{
		_sendingSemaphore = new SemaphoreSlim(1);
		_savingSemaphore = new SemaphoreSlim(1);
		_requests = new List<Request>();
		IsSending = false;
		_localThreadIdOverride = null;
		_serverThreadIdOverride = null;
		Id = id;
		IdIsLocal = isThreadIdLocal;
	}

	public bool HasOutstandingRequests()
	{
		return GetNumOutstandingRequests() > 0;
	}

	public int GetNumOutstandingRequests()
	{
		lock (_lock)
		{
			return _requests.Count;
		}
	}

	internal void EnqueueRequest(Request request)
	{
		lock (_lock)
		{
			if (_localThreadIdOverride != null)
			{
				request.LocalThreadId = _localThreadIdOverride;
			}
			if (_serverThreadIdOverride != null)
			{
				request.ServerThreadId = _serverThreadIdOverride;
			}
			_requests.Add(request);
			IsSending = true;
		}
	}

	internal async Task WaitToSendAsync()
	{
		await _sendingSemaphore.WaitAsync();
	}

	internal async Task WaitToSaveAsync()
	{
		await _savingSemaphore.WaitAsync();
	}

	internal void OverrideLocalThreadId(string id)
	{
		_localThreadIdOverride = id;
		lock (_lock)
		{
			foreach (Request request in _requests)
			{
				request.LocalThreadId = id;
			}
		}
	}

	internal void OverrideServerThreadId(string id)
	{
		_serverThreadIdOverride = id;
		lock (_lock)
		{
			foreach (Request request in _requests)
			{
				request.ServerThreadId = id;
			}
		}
	}

	internal void ReleaseSend(Request request)
	{
		lock (_lock)
		{
			_requests.Remove(request);
			if (_requests.Count == 0)
			{
				IsSending = false;
			}
		}
		_sendingSemaphore.Release();
	}

	internal void ReleaseSave()
	{
		_savingSemaphore.Release();
	}

	public List<Request> GetOutstandingRequests()
	{
		lock (_lock)
		{
			return new List<Request>(_requests);
		}
	}
}

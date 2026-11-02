using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;

namespace Thrift.Transport
{
	public class THttpClient : TTransport, IDisposable
	{
		private class FlushAsyncResult : IAsyncResult
		{
			private volatile bool _isCompleted;

			private ManualResetEvent _evt;

			private readonly AsyncCallback _cbMethod;

			private readonly object _state;

			private readonly object _locker = new object();

			internal byte[] Data { get; set; }

			internal HttpWebRequest Connection { get; set; }

			internal TTransportException AsyncException { get; set; }

			public object AsyncState => _state;

			public WaitHandle AsyncWaitHandle => GetEvtHandle();

			public bool CompletedSynchronously => false;

			public bool IsCompleted => _isCompleted;

			public FlushAsyncResult(AsyncCallback cbMethod, object state)
			{
				_cbMethod = cbMethod;
				_state = state;
			}

			private ManualResetEvent GetEvtHandle()
			{
				lock (_locker)
				{
					if (_evt == null)
					{
						_evt = new ManualResetEvent(false);
					}
					if (_isCompleted)
					{
						_evt.Set();
					}
				}
				return _evt;
			}

			internal void UpdateStatusToComplete()
			{
				_isCompleted = true;
				lock (_locker)
				{
					if (_evt != null)
					{
						_evt.Set();
					}
				}
			}

			internal void NotifyCallbackWhenAvailable()
			{
				if (_cbMethod != null)
				{
					_cbMethod(this);
				}
			}
		}

		private readonly Uri uri;

		private Stream inputStream;

		private MemoryStream outputStream = new MemoryStream();

		private int connectTimeout = 30000;

		private int readTimeout = 30000;

		private IDictionary<string, string> customHeaders = new Dictionary<string, string>();

		private HttpWebRequest connection;

		private bool _IsDisposed;

		public int ConnectTimeout
		{
			set
			{
				connectTimeout = value;
			}
		}

		public int ReadTimeout
		{
			set
			{
				readTimeout = value;
			}
		}

		public IDictionary<string, string> CustomHeaders => customHeaders;

		public override bool IsOpen => true;

		public THttpClient(Uri u)
		{
			uri = u;
			connection = CreateRequest();
		}

		public override void Open()
		{
		}

		public override void Close()
		{
			if (inputStream != null)
			{
				inputStream.Dispose();
				inputStream = null;
			}
			if (outputStream != null)
			{
				outputStream.Dispose();
				outputStream = null;
			}
		}

		public override int Read(byte[] buf, int off, int len)
		{
			if (inputStream == null)
			{
				throw new TTransportException(TTransportException.ExceptionType.NotOpen, "No request has been sent");
			}
			try
			{
				int num = inputStream.Read(buf, off, len);
				if (num == -1)
				{
					throw new TTransportException(TTransportException.ExceptionType.EndOfFile, "No more data available");
				}
				return num;
			}
			catch (IOException ex)
			{
				throw new TTransportException(TTransportException.ExceptionType.Unknown, ex.ToString());
			}
		}

		public override void Write(byte[] buf, int off, int len)
		{
			outputStream.Write(buf, off, len);
		}

		public override void Flush()
		{
			try
			{
				SendRequest();
			}
			finally
			{
				outputStream = new MemoryStream();
			}
		}

		private void SendRequest()
		{
			try
			{
				HttpWebRequest connection = CreateRequest();
				byte[] array = outputStream.ToArray();
				AutoResetEvent evt = new AutoResetEvent(false);
				Stream stream = null;
				connection.BeginGetRequestStream((IAsyncResult res) =>
				{
					stream = connection.EndGetRequestStream(res);
					evt.Set();
				}, null);
				evt.WaitOne();
				stream.Write(array, 0, array.Length);
				WebResponse response = null;
				connection.BeginGetResponse((IAsyncResult res) =>
				{
					response = connection.EndGetResponse(res);
					evt.Set();
				}, null);
				evt.WaitOne();
				inputStream = response.GetResponseStream();
				stream.Dispose();
			}
			catch (IOException ex)
			{
				throw new TTransportException(TTransportException.ExceptionType.Unknown, ex.ToString());
			}
			catch (WebException ex2)
			{
				throw new TTransportException(TTransportException.ExceptionType.Unknown, "Couldn't connect to server: " + ex2);
			}
		}

		private HttpWebRequest CreateRequest()
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
			httpWebRequest.ContentType = "application/x-thrift";
			httpWebRequest.Accept = "application/x-thrift";
			httpWebRequest.Method = "POST";
			foreach (KeyValuePair<string, string> customHeader in customHeaders)
			{
				httpWebRequest.Headers[customHeader.Key] = customHeader.Value;
			}
			return httpWebRequest;
		}

		public override IAsyncResult BeginFlush(AsyncCallback callback, object state)
		{
			byte[] data = outputStream.ToArray();
			try
			{
				FlushAsyncResult flushAsyncResult = new FlushAsyncResult(callback, state);
				flushAsyncResult.Connection = CreateRequest();
				flushAsyncResult.Data = data;
				flushAsyncResult.Connection.BeginGetRequestStream(GetRequestStreamCallback, flushAsyncResult);
				return flushAsyncResult;
			}
			catch (IOException ex)
			{
				throw new TTransportException(ex.ToString());
			}
		}

		public override void EndFlush(IAsyncResult asyncResult)
		{
			try
			{
				FlushAsyncResult flushAsyncResult = (FlushAsyncResult)asyncResult;
				if (!flushAsyncResult.IsCompleted)
				{
					WaitHandle asyncWaitHandle = flushAsyncResult.AsyncWaitHandle;
					asyncWaitHandle.WaitOne();
					asyncWaitHandle.Dispose();
				}
				if (flushAsyncResult.AsyncException != null)
				{
					throw flushAsyncResult.AsyncException;
				}
			}
			finally
			{
				outputStream = new MemoryStream();
			}
		}

		private void GetRequestStreamCallback(IAsyncResult asynchronousResult)
		{
			FlushAsyncResult flushAsyncResult = (FlushAsyncResult)asynchronousResult.AsyncState;
			try
			{
				Stream stream = flushAsyncResult.Connection.EndGetRequestStream(asynchronousResult);
				stream.Write(flushAsyncResult.Data, 0, flushAsyncResult.Data.Length);
				stream.Flush();
				stream.Dispose();
				flushAsyncResult.Connection.BeginGetResponse(GetResponseCallback, flushAsyncResult);
			}
			catch (Exception ex)
			{
				flushAsyncResult.AsyncException = new TTransportException(ex.ToString());
				flushAsyncResult.UpdateStatusToComplete();
				flushAsyncResult.NotifyCallbackWhenAvailable();
			}
		}

		private void GetResponseCallback(IAsyncResult asynchronousResult)
		{
			FlushAsyncResult flushAsyncResult = (FlushAsyncResult)asynchronousResult.AsyncState;
			try
			{
				inputStream = flushAsyncResult.Connection.EndGetResponse(asynchronousResult).GetResponseStream();
			}
			catch (Exception ex)
			{
				flushAsyncResult.AsyncException = new TTransportException(ex.ToString());
			}
			flushAsyncResult.UpdateStatusToComplete();
			flushAsyncResult.NotifyCallbackWhenAvailable();
		}

		protected override void Dispose(bool disposing)
		{
			if (!_IsDisposed && disposing)
			{
				if (inputStream != null)
				{
					inputStream.Dispose();
				}
				if (outputStream != null)
				{
					outputStream.Dispose();
				}
			}
			_IsDisposed = true;
		}
	}
}

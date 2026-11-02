using System;
using System.IO;

namespace ThriftPortable.Extensions
{
	public interface ISocket : IDisposable
	{
		int ReceiveTimeout { get; set; }

		int SendTimeout { get; set; }

		bool IsConnected { get; }

		Stream GetStream();

		void Connect();

		void Connect(int timeout);
	}
}

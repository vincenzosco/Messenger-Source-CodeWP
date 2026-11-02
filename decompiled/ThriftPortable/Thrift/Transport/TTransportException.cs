namespace Thrift.Transport
{
	public class TTransportException : TException
	{
		public enum ExceptionType
		{
			Unknown,
			NotOpen,
			AlreadyOpen,
			TimedOut,
			EndOfFile
		}

		protected ExceptionType type;

		public ExceptionType Type => type;

		public TTransportException()
		{
		}

		public TTransportException(ExceptionType type)
			: this()
		{
			this.type = type;
		}

		public TTransportException(ExceptionType type, string message)
			: base(message)
		{
			this.type = type;
		}

		public TTransportException(string message)
			: base(message)
		{
		}
	}
}

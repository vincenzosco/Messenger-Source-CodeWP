namespace Thrift.Transport
{
	public class TTransportFactory
	{
		public virtual TTransport GetTransport(TTransport trans)
		{
			return trans;
		}
	}
}

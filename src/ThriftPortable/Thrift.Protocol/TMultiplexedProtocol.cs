namespace Thrift.Protocol
{
	public class TMultiplexedProtocol : TProtocolDecorator
	{
		public static string SEPARATOR = ":";

		private string ServiceName;

		public TMultiplexedProtocol(TProtocol protocol, string serviceName)
			: base(protocol)
		{
			ServiceName = serviceName;
		}

		public override void WriteMessageBegin(TMessage tMessage)
		{
			TMessageType type = tMessage.Type;
			if (type == TMessageType.Call || type == TMessageType.Oneway)
			{
				base.WriteMessageBegin(new TMessage(ServiceName + SEPARATOR + tMessage.Name, tMessage.Type, tMessage.SeqID));
			}
			else
			{
				base.WriteMessageBegin(tMessage);
			}
		}
	}
}

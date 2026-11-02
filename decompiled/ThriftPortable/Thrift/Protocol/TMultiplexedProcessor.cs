using System.Collections.Generic;
using System.IO;

namespace Thrift.Protocol;

public class TMultiplexedProcessor : TProcessor
{
	private class StoredMessageProtocol : TProtocolDecorator
	{
		private TMessage MsgBegin;

		public StoredMessageProtocol(TProtocol protocol, TMessage messageBegin)
			: base(protocol)
		{
			MsgBegin = messageBegin;
		}

		public override TMessage ReadMessageBegin()
		{
			return MsgBegin;
		}
	}

	private Dictionary<string, TProcessor> ServiceProcessorMap = new Dictionary<string, TProcessor>();

	public void RegisterProcessor(string serviceName, TProcessor processor)
	{
		ServiceProcessorMap.Add(serviceName, processor);
	}

	private void Fail(TProtocol oprot, TMessage message, TApplicationException.ExceptionType extype, string etxt)
	{
		TApplicationException ex = new TApplicationException(extype, etxt);
		TMessage message2 = new TMessage(message.Name, TMessageType.Exception, message.SeqID);
		oprot.WriteMessageBegin(message2);
		ex.Write(oprot);
		oprot.WriteMessageEnd();
		oprot.Transport.Flush();
	}

	public bool Process(TProtocol iprot, TProtocol oprot)
	{
		try
		{
			TMessage message = iprot.ReadMessageBegin();
			if (message.Type != TMessageType.Call && message.Type != TMessageType.Oneway)
			{
				Fail(oprot, message, TApplicationException.ExceptionType.InvalidMessageType, "Message type CALL or ONEWAY expected");
				return false;
			}
			int num = message.Name.IndexOf(TMultiplexedProtocol.SEPARATOR);
			if (num < 0)
			{
				Fail(oprot, message, TApplicationException.ExceptionType.InvalidProtocol, "Service name not found in message name: " + message.Name + ". Did you forget to use a TMultiplexProtocol in your client?");
				return false;
			}
			string text = message.Name.Substring(0, num);
			if (!ServiceProcessorMap.TryGetValue(text, out var value))
			{
				Fail(oprot, message, TApplicationException.ExceptionType.InternalError, "Service name not found: " + text + ". Did you forget to call RegisterProcessor()?");
				return false;
			}
			TMessage messageBegin = new TMessage(message.Name.Substring(text.Length + TMultiplexedProtocol.SEPARATOR.Length), message.Type, message.SeqID);
			return value.Process(new StoredMessageProtocol(iprot, messageBegin), oprot);
		}
		catch (IOException)
		{
			return false;
		}
	}
}

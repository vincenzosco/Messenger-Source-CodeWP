using Thrift.Protocol;

namespace Thrift
{
	public class TApplicationException : TException
	{
		public enum ExceptionType
		{
			Unknown,
			UnknownMethod,
			InvalidMessageType,
			WrongMethodName,
			BadSequenceID,
			MissingResult,
			InternalError,
			ProtocolError,
			InvalidTransform,
			InvalidProtocol,
			UnsupportedClientType
		}

		protected ExceptionType type;

		public TApplicationException()
		{
		}

		public TApplicationException(ExceptionType type)
		{
			this.type = type;
		}

		public TApplicationException(ExceptionType type, string message)
			: base(message)
		{
			this.type = type;
		}

		public static TApplicationException Read(TProtocol iprot)
		{
			string message = null;
			ExceptionType exceptionType = ExceptionType.Unknown;
			iprot.ReadStructBegin();
			while (true)
			{
				TField tField = iprot.ReadFieldBegin();
				if (tField.Type == TType.Stop)
				{
					break;
				}
				switch (tField.ID)
				{
				case 1:
					if (tField.Type == TType.String)
					{
						message = iprot.ReadString();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 2:
					if (tField.Type == TType.I32)
					{
						exceptionType = (ExceptionType)iprot.ReadI32();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				default:
					TProtocolUtil.Skip(iprot, tField.Type);
					break;
				}
				iprot.ReadFieldEnd();
			}
			iprot.ReadStructEnd();
			return new TApplicationException(exceptionType, message);
		}

		public void Write(TProtocol oprot)
		{
			TStruct struc = new TStruct("TApplicationException");
			TField field = default(TField);
			oprot.WriteStructBegin(struc);
			if (!string.IsNullOrEmpty(Message))
			{
				field.Name = "message";
				field.Type = TType.String;
				field.ID = 1;
				oprot.WriteFieldBegin(field);
				oprot.WriteString(Message);
				oprot.WriteFieldEnd();
			}
			field.Name = "type";
			field.Type = TType.I32;
			field.ID = 2;
			oprot.WriteFieldBegin(field);
			oprot.WriteI32((int)type);
			oprot.WriteFieldEnd();
			oprot.WriteFieldStop();
			oprot.WriteStructEnd();
		}
	}
}

using System.Text;
using Thrift.Protocol;

namespace Facebook.Models.SyncProtocol
{
	public class MqttThriftHeader : TBase, TAbstractBase
	{
		public struct Isset
		{
			public bool traceInfo;
		}

		private string _traceInfo;

		public Isset __isset;

		public string TraceInfo
		{
			get
			{
				return _traceInfo;
			}
			set
			{
				__isset.traceInfo = true;
				_traceInfo = value;
			}
		}

		public void Read(TProtocol iprot)
		{
			iprot.ReadStructBegin();
			while (true)
			{
				TField tField = iprot.ReadFieldBegin();
				if (tField.Type == TType.Stop)
				{
					break;
				}
				short iD = tField.ID;
				if (iD == 1)
				{
					if (tField.Type == TType.String)
					{
						TraceInfo = iprot.ReadString();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				iprot.ReadFieldEnd();
			}
			iprot.ReadStructEnd();
		}

		public void Write(TProtocol oprot)
		{
			TStruct struc = new TStruct("MqttThriftHeader");
			oprot.WriteStructBegin(struc);
			TField field = default(TField);
			if (TraceInfo != null && __isset.traceInfo)
			{
				field.Name = "traceInfo";
				field.Type = TType.String;
				field.ID = 1;
				oprot.WriteFieldBegin(field);
				oprot.WriteString(TraceInfo);
				oprot.WriteFieldEnd();
			}
			oprot.WriteFieldStop();
			oprot.WriteStructEnd();
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("MqttThriftHeader(");
			stringBuilder.Append("TraceInfo: ");
			stringBuilder.Append(TraceInfo);
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}
	}
}

using System.Text;
using Thrift.Protocol;

namespace Facebook.Models.SyncProtocol;

public class DeltaNoOp : TBase, TAbstractBase
{
	public struct Isset
	{
		public bool numNoOps;

		public bool irisSeqId;
	}

	private int _numNoOps;

	private long _irisSeqId;

	public Isset __isset;

	public int NumNoOps
	{
		get
		{
			return _numNoOps;
		}
		set
		{
			__isset.numNoOps = true;
			_numNoOps = value;
		}
	}

	public long IrisSeqId
	{
		get
		{
			return _irisSeqId;
		}
		set
		{
			__isset.irisSeqId = true;
			_irisSeqId = value;
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
			switch (tField.ID)
			{
			case 1:
				if (tField.Type == TType.I32)
				{
					NumNoOps = iprot.ReadI32();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 1000:
				if (tField.Type == TType.I64)
				{
					IrisSeqId = iprot.ReadI64();
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
	}

	public void Write(TProtocol oprot)
	{
		TStruct struc = new TStruct("DeltaNoOp");
		oprot.WriteStructBegin(struc);
		TField field = default;
		if (__isset.numNoOps)
		{
			field.Name = "numNoOps";
			field.Type = TType.I32;
			field.ID = 1;
			oprot.WriteFieldBegin(field);
			oprot.WriteI32(NumNoOps);
			oprot.WriteFieldEnd();
		}
		if (__isset.irisSeqId)
		{
			field.Name = "irisSeqId";
			field.Type = TType.I64;
			field.ID = 1000;
			oprot.WriteFieldBegin(field);
			oprot.WriteI64(IrisSeqId);
			oprot.WriteFieldEnd();
		}
		oprot.WriteFieldStop();
		oprot.WriteStructEnd();
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder("DeltaNoOp(");
		stringBuilder.Append("NumNoOps: ");
		stringBuilder.Append(NumNoOps);
		stringBuilder.Append(",IrisSeqId: ");
		stringBuilder.Append(IrisSeqId);
		stringBuilder.Append(")");
		return stringBuilder.ToString();
	}
}

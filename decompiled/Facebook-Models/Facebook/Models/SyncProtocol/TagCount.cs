using System.Text;
using Thrift.Protocol;

namespace Facebook.Models.SyncProtocol;

public class TagCount : TBase, TAbstractBase
{
	public struct Isset
	{
		public bool hasMore;
	}

	private bool _hasMore;

	public Isset __isset;

	public int Count { get; set; }

	public bool HasMore
	{
		get
		{
			return _hasMore;
		}
		set
		{
			__isset.hasMore = true;
			_hasMore = value;
		}
	}

	public TagCount()
	{
	}

	public TagCount(int count)
		: this()
	{
		Count = count;
	}

	public void Read(TProtocol iprot)
	{
		bool flag = false;
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
					Count = iprot.ReadI32();
					flag = true;
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 2:
				if (tField.Type == TType.Bool)
				{
					HasMore = iprot.ReadBool();
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
		if (!flag)
		{
			throw new TProtocolException(1);
		}
	}

	public void Write(TProtocol oprot)
	{
		TStruct struc = new TStruct("TagCount");
		oprot.WriteStructBegin(struc);
		TField field = new TField
		{
			Name = "count",
			Type = TType.I32,
			ID = 1
		};
		oprot.WriteFieldBegin(field);
		oprot.WriteI32(Count);
		oprot.WriteFieldEnd();
		if (__isset.hasMore)
		{
			field.Name = "hasMore";
			field.Type = TType.Bool;
			field.ID = 2;
			oprot.WriteFieldBegin(field);
			oprot.WriteBool(HasMore);
			oprot.WriteFieldEnd();
		}
		oprot.WriteFieldStop();
		oprot.WriteStructEnd();
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder("TagCount(");
		stringBuilder.Append("Count: ");
		stringBuilder.Append(Count);
		stringBuilder.Append(",HasMore: ");
		stringBuilder.Append(HasMore);
		stringBuilder.Append(")");
		return stringBuilder.ToString();
	}
}

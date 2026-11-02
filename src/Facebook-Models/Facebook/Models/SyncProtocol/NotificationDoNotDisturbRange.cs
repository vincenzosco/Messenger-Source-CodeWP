using System.Text;
using Thrift.Protocol;

namespace Facebook.Models.SyncProtocol
{
	public class NotificationDoNotDisturbRange : TBase, TAbstractBase
	{
		public sbyte Days { get; set; }

		public short StartMinutes { get; set; }

		public short DurationMinutes { get; set; }

		public NotificationDoNotDisturbRange()
		{
		}

		public NotificationDoNotDisturbRange(sbyte days, short startMinutes, short durationMinutes)
			: this()
		{
			Days = days;
			StartMinutes = startMinutes;
			DurationMinutes = durationMinutes;
		}

		public void Read(TProtocol iprot)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
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
					if (tField.Type == TType.Byte)
					{
						Days = iprot.ReadByte();
						flag = true;
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 2:
					if (tField.Type == TType.I16)
					{
						StartMinutes = iprot.ReadI16();
						flag2 = true;
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 3:
					if (tField.Type == TType.I16)
					{
						DurationMinutes = iprot.ReadI16();
						flag3 = true;
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
			if (!flag2)
			{
				throw new TProtocolException(1);
			}
			if (!flag3)
			{
				throw new TProtocolException(1);
			}
		}

		public void Write(TProtocol oprot)
		{
			TStruct struc = new TStruct("NotificationDoNotDisturbRange");
			oprot.WriteStructBegin(struc);
			TField field = new TField
			{
				Name = "days",
				Type = TType.Byte,
				ID = 1
			};
			oprot.WriteFieldBegin(field);
			oprot.WriteByte(Days);
			oprot.WriteFieldEnd();
			field.Name = "startMinutes";
			field.Type = TType.I16;
			field.ID = 2;
			oprot.WriteFieldBegin(field);
			oprot.WriteI16(StartMinutes);
			oprot.WriteFieldEnd();
			field.Name = "durationMinutes";
			field.Type = TType.I16;
			field.ID = 3;
			oprot.WriteFieldBegin(field);
			oprot.WriteI16(DurationMinutes);
			oprot.WriteFieldEnd();
			oprot.WriteFieldStop();
			oprot.WriteStructEnd();
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("NotificationDoNotDisturbRange(");
			stringBuilder.Append("Days: ");
			stringBuilder.Append(Days);
			stringBuilder.Append(",StartMinutes: ");
			stringBuilder.Append(StartMinutes);
			stringBuilder.Append(",DurationMinutes: ");
			stringBuilder.Append(DurationMinutes);
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}
	}
}

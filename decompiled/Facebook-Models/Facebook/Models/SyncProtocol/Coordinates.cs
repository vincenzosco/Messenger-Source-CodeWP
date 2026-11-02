using System.Text;
using Thrift.Protocol;

namespace Facebook.Models.SyncProtocol
{
	public class Coordinates : TBase, TAbstractBase
	{
		public struct Isset
		{
			public bool latitude;

			public bool longitude;
		}

		private int _latitude;

		private int _longitude;

		public Isset __isset;

		public int Latitude
		{
			get
			{
				return _latitude;
			}
			set
			{
				__isset.latitude = true;
				_latitude = value;
			}
		}

		public int Longitude
		{
			get
			{
				return _longitude;
			}
			set
			{
				__isset.longitude = true;
				_longitude = value;
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
						Latitude = iprot.ReadI32();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 2:
					if (tField.Type == TType.I32)
					{
						Longitude = iprot.ReadI32();
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
			TStruct struc = new TStruct("Coordinates");
			oprot.WriteStructBegin(struc);
			TField field = default(TField);
			if (__isset.latitude)
			{
				field.Name = "latitude";
				field.Type = TType.I32;
				field.ID = 1;
				oprot.WriteFieldBegin(field);
				oprot.WriteI32(Latitude);
				oprot.WriteFieldEnd();
			}
			if (__isset.longitude)
			{
				field.Name = "longitude";
				field.Type = TType.I32;
				field.ID = 2;
				oprot.WriteFieldBegin(field);
				oprot.WriteI32(Longitude);
				oprot.WriteFieldEnd();
			}
			oprot.WriteFieldStop();
			oprot.WriteStructEnd();
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("Coordinates(");
			stringBuilder.Append("Latitude: ");
			stringBuilder.Append(Latitude);
			stringBuilder.Append(",Longitude: ");
			stringBuilder.Append(Longitude);
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}
	}
}

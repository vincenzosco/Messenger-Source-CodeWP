using System.Collections.Generic;
using System.Text;
using Thrift.Protocol;

namespace Facebook.Models.SyncProtocol
{
	public class DeltaNotificationSettings : TBase, TAbstractBase
	{
		public struct Isset
		{
			public bool threadKey;

			public bool doNotDisturbRanges;

			public bool irisSeqId;
		}

		private ThreadKey _threadKey;

		private List<NotificationDoNotDisturbRange> _doNotDisturbRanges;

		private long _irisSeqId;

		public Isset __isset;

		public ThreadKey ThreadKey
		{
			get
			{
				return _threadKey;
			}
			set
			{
				__isset.threadKey = true;
				_threadKey = value;
			}
		}

		public List<NotificationDoNotDisturbRange> DoNotDisturbRanges
		{
			get
			{
				return _doNotDisturbRanges;
			}
			set
			{
				__isset.doNotDisturbRanges = true;
				_doNotDisturbRanges = value;
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
					if (tField.Type == TType.Struct)
					{
						ThreadKey = new ThreadKey();
						ThreadKey.Read(iprot);
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 2:
					if (tField.Type == TType.List)
					{
						DoNotDisturbRanges = new List<NotificationDoNotDisturbRange>();
						TList tList = iprot.ReadListBegin();
						for (int i = 0; i < tList.Count; i++)
						{
							NotificationDoNotDisturbRange notificationDoNotDisturbRange = new NotificationDoNotDisturbRange();
							notificationDoNotDisturbRange = new NotificationDoNotDisturbRange();
							notificationDoNotDisturbRange.Read(iprot);
							DoNotDisturbRanges.Add(notificationDoNotDisturbRange);
						}
						iprot.ReadListEnd();
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
			TStruct struc = new TStruct("DeltaNotificationSettings");
			oprot.WriteStructBegin(struc);
			TField field = default(TField);
			if (ThreadKey != null && __isset.threadKey)
			{
				field.Name = "threadKey";
				field.Type = TType.Struct;
				field.ID = 1;
				oprot.WriteFieldBegin(field);
				ThreadKey.Write(oprot);
				oprot.WriteFieldEnd();
			}
			if (DoNotDisturbRanges != null && __isset.doNotDisturbRanges)
			{
				field.Name = "doNotDisturbRanges";
				field.Type = TType.List;
				field.ID = 2;
				oprot.WriteFieldBegin(field);
				oprot.WriteListBegin(new TList(TType.Struct, DoNotDisturbRanges.Count));
				foreach (NotificationDoNotDisturbRange doNotDisturbRange in DoNotDisturbRanges)
				{
					doNotDisturbRange.Write(oprot);
				}
				oprot.WriteListEnd();
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
			StringBuilder stringBuilder = new StringBuilder("DeltaNotificationSettings(");
			stringBuilder.Append("ThreadKey: ");
			stringBuilder.Append((ThreadKey == null) ? "<null>" : ThreadKey.ToString());
			stringBuilder.Append(",DoNotDisturbRanges: ");
			stringBuilder.Append(DoNotDisturbRanges);
			stringBuilder.Append(",IrisSeqId: ");
			stringBuilder.Append(IrisSeqId);
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}
	}
}

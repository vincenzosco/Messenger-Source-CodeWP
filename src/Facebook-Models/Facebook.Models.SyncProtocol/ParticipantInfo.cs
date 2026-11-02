using System.Collections.Generic;
using System.Text;
using Thrift.Protocol;

namespace Facebook.Models.SyncProtocol
{
	public class ParticipantInfo : TBase, TAbstractBase
	{
		public struct Isset
		{
			public bool userFbId;

			public bool firstName;

			public bool fullName;

			public bool isMessengerUser;

			public bool profPicURIMap;

			public bool initialFolder;
		}

		private long _userFbId;

		private string _firstName;

		private string _fullName;

		private bool _isMessengerUser;

		private Dictionary<int, string> _profPicURIMap;

		private ThreadFolder _initialFolder;

		public Isset __isset;

		public long UserFbId
		{
			get
			{
				return _userFbId;
			}
			set
			{
				__isset.userFbId = true;
				_userFbId = value;
			}
		}

		public string FirstName
		{
			get
			{
				return _firstName;
			}
			set
			{
				__isset.firstName = true;
				_firstName = value;
			}
		}

		public string FullName
		{
			get
			{
				return _fullName;
			}
			set
			{
				__isset.fullName = true;
				_fullName = value;
			}
		}

		public bool IsMessengerUser
		{
			get
			{
				return _isMessengerUser;
			}
			set
			{
				__isset.isMessengerUser = true;
				_isMessengerUser = value;
			}
		}

		public Dictionary<int, string> ProfPicURIMap
		{
			get
			{
				return _profPicURIMap;
			}
			set
			{
				__isset.profPicURIMap = true;
				_profPicURIMap = value;
			}
		}

		public ThreadFolder InitialFolder
		{
			get
			{
				return _initialFolder;
			}
			set
			{
				__isset.initialFolder = true;
				_initialFolder = value;
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
					if (tField.Type == TType.I64)
					{
						UserFbId = iprot.ReadI64();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 2:
					if (tField.Type == TType.String)
					{
						FirstName = iprot.ReadString();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 3:
					if (tField.Type == TType.String)
					{
						FullName = iprot.ReadString();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 4:
					if (tField.Type == TType.Bool)
					{
						IsMessengerUser = iprot.ReadBool();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 5:
					if (tField.Type == TType.Map)
					{
						ProfPicURIMap = new Dictionary<int, string>();
						TMap tMap = iprot.ReadMapBegin();
						for (int i = 0; i < tMap.Count; i++)
						{
							int key = iprot.ReadI32();
							string value = iprot.ReadString();
							ProfPicURIMap[key] = value;
						}
						iprot.ReadMapEnd();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 1000:
					if (tField.Type == TType.I32)
					{
						InitialFolder = (ThreadFolder)iprot.ReadI32();
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
			TStruct struc = new TStruct("ParticipantInfo");
			oprot.WriteStructBegin(struc);
			TField field = default(TField);
			if (__isset.userFbId)
			{
				field.Name = "userFbId";
				field.Type = TType.I64;
				field.ID = 1;
				oprot.WriteFieldBegin(field);
				oprot.WriteI64(UserFbId);
				oprot.WriteFieldEnd();
			}
			if (FirstName != null && __isset.firstName)
			{
				field.Name = "firstName";
				field.Type = TType.String;
				field.ID = 2;
				oprot.WriteFieldBegin(field);
				oprot.WriteString(FirstName);
				oprot.WriteFieldEnd();
			}
			if (FullName != null && __isset.fullName)
			{
				field.Name = "fullName";
				field.Type = TType.String;
				field.ID = 3;
				oprot.WriteFieldBegin(field);
				oprot.WriteString(FullName);
				oprot.WriteFieldEnd();
			}
			if (__isset.isMessengerUser)
			{
				field.Name = "isMessengerUser";
				field.Type = TType.Bool;
				field.ID = 4;
				oprot.WriteFieldBegin(field);
				oprot.WriteBool(IsMessengerUser);
				oprot.WriteFieldEnd();
			}
			if (ProfPicURIMap != null && __isset.profPicURIMap)
			{
				field.Name = "profPicURIMap";
				field.Type = TType.Map;
				field.ID = 5;
				oprot.WriteFieldBegin(field);
				oprot.WriteMapBegin(new TMap(TType.I32, TType.String, ProfPicURIMap.Count));
				foreach (int key in ProfPicURIMap.Keys)
				{
					oprot.WriteI32(key);
					oprot.WriteString(ProfPicURIMap[key]);
				}
				oprot.WriteMapEnd();
				oprot.WriteFieldEnd();
			}
			if (__isset.initialFolder)
			{
				field.Name = "initialFolder";
				field.Type = TType.I32;
				field.ID = 1000;
				oprot.WriteFieldBegin(field);
				oprot.WriteI32((int)InitialFolder);
				oprot.WriteFieldEnd();
			}
			oprot.WriteFieldStop();
			oprot.WriteStructEnd();
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("ParticipantInfo(");
			stringBuilder.Append("UserFbId: ");
			stringBuilder.Append(UserFbId);
			stringBuilder.Append(",FirstName: ");
			stringBuilder.Append(FirstName);
			stringBuilder.Append(",FullName: ");
			stringBuilder.Append(FullName);
			stringBuilder.Append(",IsMessengerUser: ");
			stringBuilder.Append(IsMessengerUser);
			stringBuilder.Append(",ProfPicURIMap: ");
			stringBuilder.Append(ProfPicURIMap);
			stringBuilder.Append(",InitialFolder: ");
			stringBuilder.Append(InitialFolder);
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}
	}
}

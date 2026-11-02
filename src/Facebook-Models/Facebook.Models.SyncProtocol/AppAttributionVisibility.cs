using System.Text;
using Thrift.Protocol;

namespace Facebook.Models.SyncProtocol
{
	public class AppAttributionVisibility : TBase, TAbstractBase
	{
		public struct Isset
		{
			public bool hideAttribution;

			public bool hideInstallButton;

			public bool hideReplyButton;

			public bool disableBroadcasting;

			public bool hideAppIcon;
		}

		private bool _hideAttribution;

		private bool _hideInstallButton;

		private bool _hideReplyButton;

		private bool _disableBroadcasting;

		private bool _hideAppIcon;

		public Isset __isset;

		public bool HideAttribution
		{
			get
			{
				return _hideAttribution;
			}
			set
			{
				__isset.hideAttribution = true;
				_hideAttribution = value;
			}
		}

		public bool HideInstallButton
		{
			get
			{
				return _hideInstallButton;
			}
			set
			{
				__isset.hideInstallButton = true;
				_hideInstallButton = value;
			}
		}

		public bool HideReplyButton
		{
			get
			{
				return _hideReplyButton;
			}
			set
			{
				__isset.hideReplyButton = true;
				_hideReplyButton = value;
			}
		}

		public bool DisableBroadcasting
		{
			get
			{
				return _disableBroadcasting;
			}
			set
			{
				__isset.disableBroadcasting = true;
				_disableBroadcasting = value;
			}
		}

		public bool HideAppIcon
		{
			get
			{
				return _hideAppIcon;
			}
			set
			{
				__isset.hideAppIcon = true;
				_hideAppIcon = value;
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
					if (tField.Type == TType.Bool)
					{
						HideAttribution = iprot.ReadBool();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 2:
					if (tField.Type == TType.Bool)
					{
						HideInstallButton = iprot.ReadBool();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 3:
					if (tField.Type == TType.Bool)
					{
						HideReplyButton = iprot.ReadBool();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 4:
					if (tField.Type == TType.Bool)
					{
						DisableBroadcasting = iprot.ReadBool();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 5:
					if (tField.Type == TType.Bool)
					{
						HideAppIcon = iprot.ReadBool();
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
			TStruct struc = new TStruct("AppAttributionVisibility");
			oprot.WriteStructBegin(struc);
			TField field = default(TField);
			if (__isset.hideAttribution)
			{
				field.Name = "hideAttribution";
				field.Type = TType.Bool;
				field.ID = 1;
				oprot.WriteFieldBegin(field);
				oprot.WriteBool(HideAttribution);
				oprot.WriteFieldEnd();
			}
			if (__isset.hideInstallButton)
			{
				field.Name = "hideInstallButton";
				field.Type = TType.Bool;
				field.ID = 2;
				oprot.WriteFieldBegin(field);
				oprot.WriteBool(HideInstallButton);
				oprot.WriteFieldEnd();
			}
			if (__isset.hideReplyButton)
			{
				field.Name = "hideReplyButton";
				field.Type = TType.Bool;
				field.ID = 3;
				oprot.WriteFieldBegin(field);
				oprot.WriteBool(HideReplyButton);
				oprot.WriteFieldEnd();
			}
			if (__isset.disableBroadcasting)
			{
				field.Name = "disableBroadcasting";
				field.Type = TType.Bool;
				field.ID = 4;
				oprot.WriteFieldBegin(field);
				oprot.WriteBool(DisableBroadcasting);
				oprot.WriteFieldEnd();
			}
			if (__isset.hideAppIcon)
			{
				field.Name = "hideAppIcon";
				field.Type = TType.Bool;
				field.ID = 5;
				oprot.WriteFieldBegin(field);
				oprot.WriteBool(HideAppIcon);
				oprot.WriteFieldEnd();
			}
			oprot.WriteFieldStop();
			oprot.WriteStructEnd();
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("AppAttributionVisibility(");
			stringBuilder.Append("HideAttribution: ");
			stringBuilder.Append(HideAttribution);
			stringBuilder.Append(",HideInstallButton: ");
			stringBuilder.Append(HideInstallButton);
			stringBuilder.Append(",HideReplyButton: ");
			stringBuilder.Append(HideReplyButton);
			stringBuilder.Append(",DisableBroadcasting: ");
			stringBuilder.Append(DisableBroadcasting);
			stringBuilder.Append(",HideAppIcon: ");
			stringBuilder.Append(HideAppIcon);
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}
	}
}

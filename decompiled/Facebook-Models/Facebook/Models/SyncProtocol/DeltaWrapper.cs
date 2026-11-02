using System.Text;
using Thrift.Protocol;

namespace Facebook.Models.SyncProtocol;

public class DeltaWrapper : TBase, TAbstractBase
{
	public struct Isset
	{
		public bool deltaNoOp;

		public bool deltaNewMessage;

		public bool deltaNewGroupThread;

		public bool deltaMarkRead;

		public bool deltaMarkUnread;

		public bool deltaMessageDelete;

		public bool deltaThreadDelete;

		public bool deltaParticipantsAddedToGroupThread;

		public bool deltaParticipantLeftGroupThread;

		public bool deltaThreadName;

		public bool deltaThreadImage;

		public bool deltaThreadMuteSettings;

		public bool deltaThreadAction;

		public bool deltaThreadFolder;

		public bool deltaRTCEventLog;

		public bool deltaVideoCall;

		public bool deltaAdminTextMessage;

		public bool deltaForcedFetch;

		public bool deltaReadReceipt;

		public bool deltaBroadcastMessage;

		public bool deltaMarkFolderSeen;

		public bool deltaSentMessage;

		public bool deltaPinnedGroups;

		public bool deltaPageAdminReply;

		public bool deltaDeliveryReceipt;

		public bool deltaP2PPaymentMessage;

		public bool deltaFolderCount;

		public bool deltaPagesManagerEvent;

		public bool deltaNotificationSettings;
	}

	private DeltaNoOp _deltaNoOp;

	private DeltaNewMessage _deltaNewMessage;

	private DeltaNewGroupThread _deltaNewGroupThread;

	private DeltaMarkRead _deltaMarkRead;

	private DeltaMarkUnread _deltaMarkUnread;

	private DeltaMessageDelete _deltaMessageDelete;

	private DeltaThreadDelete _deltaThreadDelete;

	private DeltaParticipantsAddedToGroupThread _deltaParticipantsAddedToGroupThread;

	private DeltaParticipantLeftGroupThread _deltaParticipantLeftGroupThread;

	private DeltaThreadName _deltaThreadName;

	private DeltaThreadImage _deltaThreadImage;

	private DeltaThreadMuteSettings _deltaThreadMuteSettings;

	private DeltaThreadAction _deltaThreadAction;

	private DeltaThreadFolder _deltaThreadFolder;

	private DeltaRTCEventLog _deltaRTCEventLog;

	private DeltaVideoCall _deltaVideoCall;

	private DeltaAdminTextMessage _deltaAdminTextMessage;

	private DeltaForcedFetch _deltaForcedFetch;

	private DeltaReadReceipt _deltaReadReceipt;

	private DeltaBroadcastMessage _deltaBroadcastMessage;

	private DeltaMarkFolderSeen _deltaMarkFolderSeen;

	private DeltaSentMessage _deltaSentMessage;

	private DeltaPinnedGroups _deltaPinnedGroups;

	private DeltaPageAdminReply _deltaPageAdminReply;

	private DeltaDeliveryReceipt _deltaDeliveryReceipt;

	private DeltaP2PPaymentMessage _deltaP2PPaymentMessage;

	private DeltaFolderCount _deltaFolderCount;

	private DeltaPagesManagerEvent _deltaPagesManagerEvent;

	private DeltaNotificationSettings _deltaNotificationSettings;

	public Isset __isset;

	public DeltaNoOp DeltaNoOp
	{
		get
		{
			return _deltaNoOp;
		}
		set
		{
			__isset.deltaNoOp = true;
			_deltaNoOp = value;
		}
	}

	public DeltaNewMessage DeltaNewMessage
	{
		get
		{
			return _deltaNewMessage;
		}
		set
		{
			__isset.deltaNewMessage = true;
			_deltaNewMessage = value;
		}
	}

	public DeltaNewGroupThread DeltaNewGroupThread
	{
		get
		{
			return _deltaNewGroupThread;
		}
		set
		{
			__isset.deltaNewGroupThread = true;
			_deltaNewGroupThread = value;
		}
	}

	public DeltaMarkRead DeltaMarkRead
	{
		get
		{
			return _deltaMarkRead;
		}
		set
		{
			__isset.deltaMarkRead = true;
			_deltaMarkRead = value;
		}
	}

	public DeltaMarkUnread DeltaMarkUnread
	{
		get
		{
			return _deltaMarkUnread;
		}
		set
		{
			__isset.deltaMarkUnread = true;
			_deltaMarkUnread = value;
		}
	}

	public DeltaMessageDelete DeltaMessageDelete
	{
		get
		{
			return _deltaMessageDelete;
		}
		set
		{
			__isset.deltaMessageDelete = true;
			_deltaMessageDelete = value;
		}
	}

	public DeltaThreadDelete DeltaThreadDelete
	{
		get
		{
			return _deltaThreadDelete;
		}
		set
		{
			__isset.deltaThreadDelete = true;
			_deltaThreadDelete = value;
		}
	}

	public DeltaParticipantsAddedToGroupThread DeltaParticipantsAddedToGroupThread
	{
		get
		{
			return _deltaParticipantsAddedToGroupThread;
		}
		set
		{
			__isset.deltaParticipantsAddedToGroupThread = true;
			_deltaParticipantsAddedToGroupThread = value;
		}
	}

	public DeltaParticipantLeftGroupThread DeltaParticipantLeftGroupThread
	{
		get
		{
			return _deltaParticipantLeftGroupThread;
		}
		set
		{
			__isset.deltaParticipantLeftGroupThread = true;
			_deltaParticipantLeftGroupThread = value;
		}
	}

	public DeltaThreadName DeltaThreadName
	{
		get
		{
			return _deltaThreadName;
		}
		set
		{
			__isset.deltaThreadName = true;
			_deltaThreadName = value;
		}
	}

	public DeltaThreadImage DeltaThreadImage
	{
		get
		{
			return _deltaThreadImage;
		}
		set
		{
			__isset.deltaThreadImage = true;
			_deltaThreadImage = value;
		}
	}

	public DeltaThreadMuteSettings DeltaThreadMuteSettings
	{
		get
		{
			return _deltaThreadMuteSettings;
		}
		set
		{
			__isset.deltaThreadMuteSettings = true;
			_deltaThreadMuteSettings = value;
		}
	}

	public DeltaThreadAction DeltaThreadAction
	{
		get
		{
			return _deltaThreadAction;
		}
		set
		{
			__isset.deltaThreadAction = true;
			_deltaThreadAction = value;
		}
	}

	public DeltaThreadFolder DeltaThreadFolder
	{
		get
		{
			return _deltaThreadFolder;
		}
		set
		{
			__isset.deltaThreadFolder = true;
			_deltaThreadFolder = value;
		}
	}

	public DeltaRTCEventLog DeltaRTCEventLog
	{
		get
		{
			return _deltaRTCEventLog;
		}
		set
		{
			__isset.deltaRTCEventLog = true;
			_deltaRTCEventLog = value;
		}
	}

	public DeltaVideoCall DeltaVideoCall
	{
		get
		{
			return _deltaVideoCall;
		}
		set
		{
			__isset.deltaVideoCall = true;
			_deltaVideoCall = value;
		}
	}

	public DeltaAdminTextMessage DeltaAdminTextMessage
	{
		get
		{
			return _deltaAdminTextMessage;
		}
		set
		{
			__isset.deltaAdminTextMessage = true;
			_deltaAdminTextMessage = value;
		}
	}

	public DeltaForcedFetch DeltaForcedFetch
	{
		get
		{
			return _deltaForcedFetch;
		}
		set
		{
			__isset.deltaForcedFetch = true;
			_deltaForcedFetch = value;
		}
	}

	public DeltaReadReceipt DeltaReadReceipt
	{
		get
		{
			return _deltaReadReceipt;
		}
		set
		{
			__isset.deltaReadReceipt = true;
			_deltaReadReceipt = value;
		}
	}

	public DeltaBroadcastMessage DeltaBroadcastMessage
	{
		get
		{
			return _deltaBroadcastMessage;
		}
		set
		{
			__isset.deltaBroadcastMessage = true;
			_deltaBroadcastMessage = value;
		}
	}

	public DeltaMarkFolderSeen DeltaMarkFolderSeen
	{
		get
		{
			return _deltaMarkFolderSeen;
		}
		set
		{
			__isset.deltaMarkFolderSeen = true;
			_deltaMarkFolderSeen = value;
		}
	}

	public DeltaSentMessage DeltaSentMessage
	{
		get
		{
			return _deltaSentMessage;
		}
		set
		{
			__isset.deltaSentMessage = true;
			_deltaSentMessage = value;
		}
	}

	public DeltaPinnedGroups DeltaPinnedGroups
	{
		get
		{
			return _deltaPinnedGroups;
		}
		set
		{
			__isset.deltaPinnedGroups = true;
			_deltaPinnedGroups = value;
		}
	}

	public DeltaPageAdminReply DeltaPageAdminReply
	{
		get
		{
			return _deltaPageAdminReply;
		}
		set
		{
			__isset.deltaPageAdminReply = true;
			_deltaPageAdminReply = value;
		}
	}

	public DeltaDeliveryReceipt DeltaDeliveryReceipt
	{
		get
		{
			return _deltaDeliveryReceipt;
		}
		set
		{
			__isset.deltaDeliveryReceipt = true;
			_deltaDeliveryReceipt = value;
		}
	}

	public DeltaP2PPaymentMessage DeltaP2PPaymentMessage
	{
		get
		{
			return _deltaP2PPaymentMessage;
		}
		set
		{
			__isset.deltaP2PPaymentMessage = true;
			_deltaP2PPaymentMessage = value;
		}
	}

	public DeltaFolderCount DeltaFolderCount
	{
		get
		{
			return _deltaFolderCount;
		}
		set
		{
			__isset.deltaFolderCount = true;
			_deltaFolderCount = value;
		}
	}

	public DeltaPagesManagerEvent DeltaPagesManagerEvent
	{
		get
		{
			return _deltaPagesManagerEvent;
		}
		set
		{
			__isset.deltaPagesManagerEvent = true;
			_deltaPagesManagerEvent = value;
		}
	}

	public DeltaNotificationSettings DeltaNotificationSettings
	{
		get
		{
			return _deltaNotificationSettings;
		}
		set
		{
			__isset.deltaNotificationSettings = true;
			_deltaNotificationSettings = value;
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
					DeltaNoOp = new DeltaNoOp();
					DeltaNoOp.Read(iprot);
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 2:
				if (tField.Type == TType.Struct)
				{
					DeltaNewMessage = new DeltaNewMessage();
					DeltaNewMessage.Read(iprot);
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 3:
				if (tField.Type == TType.Struct)
				{
					DeltaNewGroupThread = new DeltaNewGroupThread();
					DeltaNewGroupThread.Read(iprot);
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 4:
				if (tField.Type == TType.Struct)
				{
					DeltaMarkRead = new DeltaMarkRead();
					DeltaMarkRead.Read(iprot);
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 5:
				if (tField.Type == TType.Struct)
				{
					DeltaMarkUnread = new DeltaMarkUnread();
					DeltaMarkUnread.Read(iprot);
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 6:
				if (tField.Type == TType.Struct)
				{
					DeltaMessageDelete = new DeltaMessageDelete();
					DeltaMessageDelete.Read(iprot);
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 7:
				if (tField.Type == TType.Struct)
				{
					DeltaThreadDelete = new DeltaThreadDelete();
					DeltaThreadDelete.Read(iprot);
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 8:
				if (tField.Type == TType.Struct)
				{
					DeltaParticipantsAddedToGroupThread = new DeltaParticipantsAddedToGroupThread();
					DeltaParticipantsAddedToGroupThread.Read(iprot);
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 9:
				if (tField.Type == TType.Struct)
				{
					DeltaParticipantLeftGroupThread = new DeltaParticipantLeftGroupThread();
					DeltaParticipantLeftGroupThread.Read(iprot);
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 10:
				if (tField.Type == TType.Struct)
				{
					DeltaThreadName = new DeltaThreadName();
					DeltaThreadName.Read(iprot);
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 11:
				if (tField.Type == TType.Struct)
				{
					DeltaThreadImage = new DeltaThreadImage();
					DeltaThreadImage.Read(iprot);
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 12:
				if (tField.Type == TType.Struct)
				{
					DeltaThreadMuteSettings = new DeltaThreadMuteSettings();
					DeltaThreadMuteSettings.Read(iprot);
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 13:
				if (tField.Type == TType.Struct)
				{
					DeltaThreadAction = new DeltaThreadAction();
					DeltaThreadAction.Read(iprot);
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 14:
				if (tField.Type == TType.Struct)
				{
					DeltaThreadFolder = new DeltaThreadFolder();
					DeltaThreadFolder.Read(iprot);
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 15:
				if (tField.Type == TType.Struct)
				{
					DeltaRTCEventLog = new DeltaRTCEventLog();
					DeltaRTCEventLog.Read(iprot);
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 16:
				if (tField.Type == TType.Struct)
				{
					DeltaVideoCall = new DeltaVideoCall();
					DeltaVideoCall.Read(iprot);
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 17:
				if (tField.Type == TType.Struct)
				{
					DeltaAdminTextMessage = new DeltaAdminTextMessage();
					DeltaAdminTextMessage.Read(iprot);
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 18:
				if (tField.Type == TType.Struct)
				{
					DeltaForcedFetch = new DeltaForcedFetch();
					DeltaForcedFetch.Read(iprot);
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 19:
				if (tField.Type == TType.Struct)
				{
					DeltaReadReceipt = new DeltaReadReceipt();
					DeltaReadReceipt.Read(iprot);
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 20:
				if (tField.Type == TType.Struct)
				{
					DeltaBroadcastMessage = new DeltaBroadcastMessage();
					DeltaBroadcastMessage.Read(iprot);
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 21:
				if (tField.Type == TType.Struct)
				{
					DeltaMarkFolderSeen = new DeltaMarkFolderSeen();
					DeltaMarkFolderSeen.Read(iprot);
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 22:
				if (tField.Type == TType.Struct)
				{
					DeltaSentMessage = new DeltaSentMessage();
					DeltaSentMessage.Read(iprot);
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 23:
				if (tField.Type == TType.Struct)
				{
					DeltaPinnedGroups = new DeltaPinnedGroups();
					DeltaPinnedGroups.Read(iprot);
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 24:
				if (tField.Type == TType.Struct)
				{
					DeltaPageAdminReply = new DeltaPageAdminReply();
					DeltaPageAdminReply.Read(iprot);
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 25:
				if (tField.Type == TType.Struct)
				{
					DeltaDeliveryReceipt = new DeltaDeliveryReceipt();
					DeltaDeliveryReceipt.Read(iprot);
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 26:
				if (tField.Type == TType.Struct)
				{
					DeltaP2PPaymentMessage = new DeltaP2PPaymentMessage();
					DeltaP2PPaymentMessage.Read(iprot);
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 27:
				if (tField.Type == TType.Struct)
				{
					DeltaFolderCount = new DeltaFolderCount();
					DeltaFolderCount.Read(iprot);
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 28:
				if (tField.Type == TType.Struct)
				{
					DeltaPagesManagerEvent = new DeltaPagesManagerEvent();
					DeltaPagesManagerEvent.Read(iprot);
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 29:
				if (tField.Type == TType.Struct)
				{
					DeltaNotificationSettings = new DeltaNotificationSettings();
					DeltaNotificationSettings.Read(iprot);
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
		TStruct struc = new TStruct("DeltaWrapper");
		oprot.WriteStructBegin(struc);
		TField field = default;
		if (DeltaNoOp != null && __isset.deltaNoOp)
		{
			field.Name = "deltaNoOp";
			field.Type = TType.Struct;
			field.ID = 1;
			oprot.WriteFieldBegin(field);
			DeltaNoOp.Write(oprot);
			oprot.WriteFieldEnd();
		}
		if (DeltaNewMessage != null && __isset.deltaNewMessage)
		{
			field.Name = "deltaNewMessage";
			field.Type = TType.Struct;
			field.ID = 2;
			oprot.WriteFieldBegin(field);
			DeltaNewMessage.Write(oprot);
			oprot.WriteFieldEnd();
		}
		if (DeltaNewGroupThread != null && __isset.deltaNewGroupThread)
		{
			field.Name = "deltaNewGroupThread";
			field.Type = TType.Struct;
			field.ID = 3;
			oprot.WriteFieldBegin(field);
			DeltaNewGroupThread.Write(oprot);
			oprot.WriteFieldEnd();
		}
		if (DeltaMarkRead != null && __isset.deltaMarkRead)
		{
			field.Name = "deltaMarkRead";
			field.Type = TType.Struct;
			field.ID = 4;
			oprot.WriteFieldBegin(field);
			DeltaMarkRead.Write(oprot);
			oprot.WriteFieldEnd();
		}
		if (DeltaMarkUnread != null && __isset.deltaMarkUnread)
		{
			field.Name = "deltaMarkUnread";
			field.Type = TType.Struct;
			field.ID = 5;
			oprot.WriteFieldBegin(field);
			DeltaMarkUnread.Write(oprot);
			oprot.WriteFieldEnd();
		}
		if (DeltaMessageDelete != null && __isset.deltaMessageDelete)
		{
			field.Name = "deltaMessageDelete";
			field.Type = TType.Struct;
			field.ID = 6;
			oprot.WriteFieldBegin(field);
			DeltaMessageDelete.Write(oprot);
			oprot.WriteFieldEnd();
		}
		if (DeltaThreadDelete != null && __isset.deltaThreadDelete)
		{
			field.Name = "deltaThreadDelete";
			field.Type = TType.Struct;
			field.ID = 7;
			oprot.WriteFieldBegin(field);
			DeltaThreadDelete.Write(oprot);
			oprot.WriteFieldEnd();
		}
		if (DeltaParticipantsAddedToGroupThread != null && __isset.deltaParticipantsAddedToGroupThread)
		{
			field.Name = "deltaParticipantsAddedToGroupThread";
			field.Type = TType.Struct;
			field.ID = 8;
			oprot.WriteFieldBegin(field);
			DeltaParticipantsAddedToGroupThread.Write(oprot);
			oprot.WriteFieldEnd();
		}
		if (DeltaParticipantLeftGroupThread != null && __isset.deltaParticipantLeftGroupThread)
		{
			field.Name = "deltaParticipantLeftGroupThread";
			field.Type = TType.Struct;
			field.ID = 9;
			oprot.WriteFieldBegin(field);
			DeltaParticipantLeftGroupThread.Write(oprot);
			oprot.WriteFieldEnd();
		}
		if (DeltaThreadName != null && __isset.deltaThreadName)
		{
			field.Name = "deltaThreadName";
			field.Type = TType.Struct;
			field.ID = 10;
			oprot.WriteFieldBegin(field);
			DeltaThreadName.Write(oprot);
			oprot.WriteFieldEnd();
		}
		if (DeltaThreadImage != null && __isset.deltaThreadImage)
		{
			field.Name = "deltaThreadImage";
			field.Type = TType.Struct;
			field.ID = 11;
			oprot.WriteFieldBegin(field);
			DeltaThreadImage.Write(oprot);
			oprot.WriteFieldEnd();
		}
		if (DeltaThreadMuteSettings != null && __isset.deltaThreadMuteSettings)
		{
			field.Name = "deltaThreadMuteSettings";
			field.Type = TType.Struct;
			field.ID = 12;
			oprot.WriteFieldBegin(field);
			DeltaThreadMuteSettings.Write(oprot);
			oprot.WriteFieldEnd();
		}
		if (DeltaThreadAction != null && __isset.deltaThreadAction)
		{
			field.Name = "deltaThreadAction";
			field.Type = TType.Struct;
			field.ID = 13;
			oprot.WriteFieldBegin(field);
			DeltaThreadAction.Write(oprot);
			oprot.WriteFieldEnd();
		}
		if (DeltaThreadFolder != null && __isset.deltaThreadFolder)
		{
			field.Name = "deltaThreadFolder";
			field.Type = TType.Struct;
			field.ID = 14;
			oprot.WriteFieldBegin(field);
			DeltaThreadFolder.Write(oprot);
			oprot.WriteFieldEnd();
		}
		if (DeltaRTCEventLog != null && __isset.deltaRTCEventLog)
		{
			field.Name = "deltaRTCEventLog";
			field.Type = TType.Struct;
			field.ID = 15;
			oprot.WriteFieldBegin(field);
			DeltaRTCEventLog.Write(oprot);
			oprot.WriteFieldEnd();
		}
		if (DeltaVideoCall != null && __isset.deltaVideoCall)
		{
			field.Name = "deltaVideoCall";
			field.Type = TType.Struct;
			field.ID = 16;
			oprot.WriteFieldBegin(field);
			DeltaVideoCall.Write(oprot);
			oprot.WriteFieldEnd();
		}
		if (DeltaAdminTextMessage != null && __isset.deltaAdminTextMessage)
		{
			field.Name = "deltaAdminTextMessage";
			field.Type = TType.Struct;
			field.ID = 17;
			oprot.WriteFieldBegin(field);
			DeltaAdminTextMessage.Write(oprot);
			oprot.WriteFieldEnd();
		}
		if (DeltaForcedFetch != null && __isset.deltaForcedFetch)
		{
			field.Name = "deltaForcedFetch";
			field.Type = TType.Struct;
			field.ID = 18;
			oprot.WriteFieldBegin(field);
			DeltaForcedFetch.Write(oprot);
			oprot.WriteFieldEnd();
		}
		if (DeltaReadReceipt != null && __isset.deltaReadReceipt)
		{
			field.Name = "deltaReadReceipt";
			field.Type = TType.Struct;
			field.ID = 19;
			oprot.WriteFieldBegin(field);
			DeltaReadReceipt.Write(oprot);
			oprot.WriteFieldEnd();
		}
		if (DeltaBroadcastMessage != null && __isset.deltaBroadcastMessage)
		{
			field.Name = "deltaBroadcastMessage";
			field.Type = TType.Struct;
			field.ID = 20;
			oprot.WriteFieldBegin(field);
			DeltaBroadcastMessage.Write(oprot);
			oprot.WriteFieldEnd();
		}
		if (DeltaMarkFolderSeen != null && __isset.deltaMarkFolderSeen)
		{
			field.Name = "deltaMarkFolderSeen";
			field.Type = TType.Struct;
			field.ID = 21;
			oprot.WriteFieldBegin(field);
			DeltaMarkFolderSeen.Write(oprot);
			oprot.WriteFieldEnd();
		}
		if (DeltaSentMessage != null && __isset.deltaSentMessage)
		{
			field.Name = "deltaSentMessage";
			field.Type = TType.Struct;
			field.ID = 22;
			oprot.WriteFieldBegin(field);
			DeltaSentMessage.Write(oprot);
			oprot.WriteFieldEnd();
		}
		if (DeltaPinnedGroups != null && __isset.deltaPinnedGroups)
		{
			field.Name = "deltaPinnedGroups";
			field.Type = TType.Struct;
			field.ID = 23;
			oprot.WriteFieldBegin(field);
			DeltaPinnedGroups.Write(oprot);
			oprot.WriteFieldEnd();
		}
		if (DeltaPageAdminReply != null && __isset.deltaPageAdminReply)
		{
			field.Name = "deltaPageAdminReply";
			field.Type = TType.Struct;
			field.ID = 24;
			oprot.WriteFieldBegin(field);
			DeltaPageAdminReply.Write(oprot);
			oprot.WriteFieldEnd();
		}
		if (DeltaDeliveryReceipt != null && __isset.deltaDeliveryReceipt)
		{
			field.Name = "deltaDeliveryReceipt";
			field.Type = TType.Struct;
			field.ID = 25;
			oprot.WriteFieldBegin(field);
			DeltaDeliveryReceipt.Write(oprot);
			oprot.WriteFieldEnd();
		}
		if (DeltaP2PPaymentMessage != null && __isset.deltaP2PPaymentMessage)
		{
			field.Name = "deltaP2PPaymentMessage";
			field.Type = TType.Struct;
			field.ID = 26;
			oprot.WriteFieldBegin(field);
			DeltaP2PPaymentMessage.Write(oprot);
			oprot.WriteFieldEnd();
		}
		if (DeltaFolderCount != null && __isset.deltaFolderCount)
		{
			field.Name = "deltaFolderCount";
			field.Type = TType.Struct;
			field.ID = 27;
			oprot.WriteFieldBegin(field);
			DeltaFolderCount.Write(oprot);
			oprot.WriteFieldEnd();
		}
		if (DeltaPagesManagerEvent != null && __isset.deltaPagesManagerEvent)
		{
			field.Name = "deltaPagesManagerEvent";
			field.Type = TType.Struct;
			field.ID = 28;
			oprot.WriteFieldBegin(field);
			DeltaPagesManagerEvent.Write(oprot);
			oprot.WriteFieldEnd();
		}
		if (DeltaNotificationSettings != null && __isset.deltaNotificationSettings)
		{
			field.Name = "deltaNotificationSettings";
			field.Type = TType.Struct;
			field.ID = 29;
			oprot.WriteFieldBegin(field);
			DeltaNotificationSettings.Write(oprot);
			oprot.WriteFieldEnd();
		}
		oprot.WriteFieldStop();
		oprot.WriteStructEnd();
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder("DeltaWrapper(");
		stringBuilder.Append("DeltaNoOp: ");
		stringBuilder.Append((DeltaNoOp == null) ? "<null>" : DeltaNoOp.ToString());
		stringBuilder.Append(",DeltaNewMessage: ");
		stringBuilder.Append((DeltaNewMessage == null) ? "<null>" : DeltaNewMessage.ToString());
		stringBuilder.Append(",DeltaNewGroupThread: ");
		stringBuilder.Append((DeltaNewGroupThread == null) ? "<null>" : DeltaNewGroupThread.ToString());
		stringBuilder.Append(",DeltaMarkRead: ");
		stringBuilder.Append((DeltaMarkRead == null) ? "<null>" : DeltaMarkRead.ToString());
		stringBuilder.Append(",DeltaMarkUnread: ");
		stringBuilder.Append((DeltaMarkUnread == null) ? "<null>" : DeltaMarkUnread.ToString());
		stringBuilder.Append(",DeltaMessageDelete: ");
		stringBuilder.Append((DeltaMessageDelete == null) ? "<null>" : DeltaMessageDelete.ToString());
		stringBuilder.Append(",DeltaThreadDelete: ");
		stringBuilder.Append((DeltaThreadDelete == null) ? "<null>" : DeltaThreadDelete.ToString());
		stringBuilder.Append(",DeltaParticipantsAddedToGroupThread: ");
		stringBuilder.Append((DeltaParticipantsAddedToGroupThread == null) ? "<null>" : DeltaParticipantsAddedToGroupThread.ToString());
		stringBuilder.Append(",DeltaParticipantLeftGroupThread: ");
		stringBuilder.Append((DeltaParticipantLeftGroupThread == null) ? "<null>" : DeltaParticipantLeftGroupThread.ToString());
		stringBuilder.Append(",DeltaThreadName: ");
		stringBuilder.Append((DeltaThreadName == null) ? "<null>" : DeltaThreadName.ToString());
		stringBuilder.Append(",DeltaThreadImage: ");
		stringBuilder.Append((DeltaThreadImage == null) ? "<null>" : DeltaThreadImage.ToString());
		stringBuilder.Append(",DeltaThreadMuteSettings: ");
		stringBuilder.Append((DeltaThreadMuteSettings == null) ? "<null>" : DeltaThreadMuteSettings.ToString());
		stringBuilder.Append(",DeltaThreadAction: ");
		stringBuilder.Append((DeltaThreadAction == null) ? "<null>" : DeltaThreadAction.ToString());
		stringBuilder.Append(",DeltaThreadFolder: ");
		stringBuilder.Append((DeltaThreadFolder == null) ? "<null>" : DeltaThreadFolder.ToString());
		stringBuilder.Append(",DeltaRTCEventLog: ");
		stringBuilder.Append((DeltaRTCEventLog == null) ? "<null>" : DeltaRTCEventLog.ToString());
		stringBuilder.Append(",DeltaVideoCall: ");
		stringBuilder.Append((DeltaVideoCall == null) ? "<null>" : DeltaVideoCall.ToString());
		stringBuilder.Append(",DeltaAdminTextMessage: ");
		stringBuilder.Append((DeltaAdminTextMessage == null) ? "<null>" : DeltaAdminTextMessage.ToString());
		stringBuilder.Append(",DeltaForcedFetch: ");
		stringBuilder.Append((DeltaForcedFetch == null) ? "<null>" : DeltaForcedFetch.ToString());
		stringBuilder.Append(",DeltaReadReceipt: ");
		stringBuilder.Append((DeltaReadReceipt == null) ? "<null>" : DeltaReadReceipt.ToString());
		stringBuilder.Append(",DeltaBroadcastMessage: ");
		stringBuilder.Append((DeltaBroadcastMessage == null) ? "<null>" : DeltaBroadcastMessage.ToString());
		stringBuilder.Append(",DeltaMarkFolderSeen: ");
		stringBuilder.Append((DeltaMarkFolderSeen == null) ? "<null>" : DeltaMarkFolderSeen.ToString());
		stringBuilder.Append(",DeltaSentMessage: ");
		stringBuilder.Append((DeltaSentMessage == null) ? "<null>" : DeltaSentMessage.ToString());
		stringBuilder.Append(",DeltaPinnedGroups: ");
		stringBuilder.Append((DeltaPinnedGroups == null) ? "<null>" : DeltaPinnedGroups.ToString());
		stringBuilder.Append(",DeltaPageAdminReply: ");
		stringBuilder.Append((DeltaPageAdminReply == null) ? "<null>" : DeltaPageAdminReply.ToString());
		stringBuilder.Append(",DeltaDeliveryReceipt: ");
		stringBuilder.Append((DeltaDeliveryReceipt == null) ? "<null>" : DeltaDeliveryReceipt.ToString());
		stringBuilder.Append(",DeltaP2PPaymentMessage: ");
		stringBuilder.Append((DeltaP2PPaymentMessage == null) ? "<null>" : DeltaP2PPaymentMessage.ToString());
		stringBuilder.Append(",DeltaFolderCount: ");
		stringBuilder.Append((DeltaFolderCount == null) ? "<null>" : DeltaFolderCount.ToString());
		stringBuilder.Append(",DeltaPagesManagerEvent: ");
		stringBuilder.Append((DeltaPagesManagerEvent == null) ? "<null>" : DeltaPagesManagerEvent.ToString());
		stringBuilder.Append(",DeltaNotificationSettings: ");
		stringBuilder.Append((DeltaNotificationSettings == null) ? "<null>" : DeltaNotificationSettings.ToString());
		stringBuilder.Append(")");
		return stringBuilder.ToString();
	}
}

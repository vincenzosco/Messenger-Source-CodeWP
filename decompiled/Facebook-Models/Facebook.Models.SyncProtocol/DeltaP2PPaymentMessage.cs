using System.Text;
using Thrift.Protocol;

namespace Facebook.Models.SyncProtocol
{
	public class DeltaP2PPaymentMessage : TBase, TAbstractBase
	{
		public struct Isset
		{
			public bool transferId;

			public bool messageType;

			public bool irisSeqId;

			public bool subtype;

			public bool amount;

			public bool currency;

			public bool creationTime;

			public bool completedTime;

			public bool senderName;

			public bool senderId;

			public bool senderCredentialId;

			public bool receiverName;

			public bool receiverId;

			public bool receiverCredentialId;
		}

		private long _transferId;

		private PaymentLogMessageType _messageType;

		private long _irisSeqId;

		private string _subtype;

		private string _amount;

		private string _currency;

		private long _creationTime;

		private long _completedTime;

		private string _senderName;

		private long _senderId;

		private long _senderCredentialId;

		private string _receiverName;

		private long _receiverId;

		private long _receiverCredentialId;

		public Isset __isset;

		public MessageMetadata MessageMetadata { get; set; }

		public long TransferId
		{
			get
			{
				return _transferId;
			}
			set
			{
				__isset.transferId = true;
				_transferId = value;
			}
		}

		public PaymentLogMessageType MessageType
		{
			get
			{
				return _messageType;
			}
			set
			{
				__isset.messageType = true;
				_messageType = value;
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

		public string Subtype
		{
			get
			{
				return _subtype;
			}
			set
			{
				__isset.subtype = true;
				_subtype = value;
			}
		}

		public string Amount
		{
			get
			{
				return _amount;
			}
			set
			{
				__isset.amount = true;
				_amount = value;
			}
		}

		public string Currency
		{
			get
			{
				return _currency;
			}
			set
			{
				__isset.currency = true;
				_currency = value;
			}
		}

		public long CreationTime
		{
			get
			{
				return _creationTime;
			}
			set
			{
				__isset.creationTime = true;
				_creationTime = value;
			}
		}

		public long CompletedTime
		{
			get
			{
				return _completedTime;
			}
			set
			{
				__isset.completedTime = true;
				_completedTime = value;
			}
		}

		public string SenderName
		{
			get
			{
				return _senderName;
			}
			set
			{
				__isset.senderName = true;
				_senderName = value;
			}
		}

		public long SenderId
		{
			get
			{
				return _senderId;
			}
			set
			{
				__isset.senderId = true;
				_senderId = value;
			}
		}

		public long SenderCredentialId
		{
			get
			{
				return _senderCredentialId;
			}
			set
			{
				__isset.senderCredentialId = true;
				_senderCredentialId = value;
			}
		}

		public string ReceiverName
		{
			get
			{
				return _receiverName;
			}
			set
			{
				__isset.receiverName = true;
				_receiverName = value;
			}
		}

		public long ReceiverId
		{
			get
			{
				return _receiverId;
			}
			set
			{
				__isset.receiverId = true;
				_receiverId = value;
			}
		}

		public long ReceiverCredentialId
		{
			get
			{
				return _receiverCredentialId;
			}
			set
			{
				__isset.receiverCredentialId = true;
				_receiverCredentialId = value;
			}
		}

		public DeltaP2PPaymentMessage()
		{
		}

		public DeltaP2PPaymentMessage(MessageMetadata messageMetadata)
			: this()
		{
			MessageMetadata = messageMetadata;
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
					if (tField.Type == TType.Struct)
					{
						MessageMetadata = new MessageMetadata();
						MessageMetadata.Read(iprot);
						flag = true;
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 2:
					if (tField.Type == TType.I64)
					{
						TransferId = iprot.ReadI64();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 3:
					if (tField.Type == TType.I32)
					{
						MessageType = (PaymentLogMessageType)iprot.ReadI32();
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
				case 1001:
					if (tField.Type == TType.String)
					{
						Subtype = iprot.ReadString();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 1002:
					if (tField.Type == TType.String)
					{
						Amount = iprot.ReadString();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 1003:
					if (tField.Type == TType.String)
					{
						Currency = iprot.ReadString();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 1004:
					if (tField.Type == TType.I64)
					{
						CreationTime = iprot.ReadI64();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 1005:
					if (tField.Type == TType.I64)
					{
						CompletedTime = iprot.ReadI64();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 1006:
					if (tField.Type == TType.String)
					{
						SenderName = iprot.ReadString();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 1007:
					if (tField.Type == TType.I64)
					{
						SenderId = iprot.ReadI64();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 1008:
					if (tField.Type == TType.I64)
					{
						SenderCredentialId = iprot.ReadI64();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 1009:
					if (tField.Type == TType.String)
					{
						ReceiverName = iprot.ReadString();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 1010:
					if (tField.Type == TType.I64)
					{
						ReceiverId = iprot.ReadI64();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 1011:
					if (tField.Type == TType.I64)
					{
						ReceiverCredentialId = iprot.ReadI64();
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
			TStruct struc = new TStruct("DeltaP2PPaymentMessage");
			oprot.WriteStructBegin(struc);
			TField field = new TField
			{
				Name = "messageMetadata",
				Type = TType.Struct,
				ID = 1
			};
			oprot.WriteFieldBegin(field);
			MessageMetadata.Write(oprot);
			oprot.WriteFieldEnd();
			if (__isset.transferId)
			{
				field.Name = "transferId";
				field.Type = TType.I64;
				field.ID = 2;
				oprot.WriteFieldBegin(field);
				oprot.WriteI64(TransferId);
				oprot.WriteFieldEnd();
			}
			if (__isset.messageType)
			{
				field.Name = "messageType";
				field.Type = TType.I32;
				field.ID = 3;
				oprot.WriteFieldBegin(field);
				oprot.WriteI32((int)MessageType);
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
			if (Subtype != null && __isset.subtype)
			{
				field.Name = "subtype";
				field.Type = TType.String;
				field.ID = 1001;
				oprot.WriteFieldBegin(field);
				oprot.WriteString(Subtype);
				oprot.WriteFieldEnd();
			}
			if (Amount != null && __isset.amount)
			{
				field.Name = "amount";
				field.Type = TType.String;
				field.ID = 1002;
				oprot.WriteFieldBegin(field);
				oprot.WriteString(Amount);
				oprot.WriteFieldEnd();
			}
			if (Currency != null && __isset.currency)
			{
				field.Name = "currency";
				field.Type = TType.String;
				field.ID = 1003;
				oprot.WriteFieldBegin(field);
				oprot.WriteString(Currency);
				oprot.WriteFieldEnd();
			}
			if (__isset.creationTime)
			{
				field.Name = "creationTime";
				field.Type = TType.I64;
				field.ID = 1004;
				oprot.WriteFieldBegin(field);
				oprot.WriteI64(CreationTime);
				oprot.WriteFieldEnd();
			}
			if (__isset.completedTime)
			{
				field.Name = "completedTime";
				field.Type = TType.I64;
				field.ID = 1005;
				oprot.WriteFieldBegin(field);
				oprot.WriteI64(CompletedTime);
				oprot.WriteFieldEnd();
			}
			if (SenderName != null && __isset.senderName)
			{
				field.Name = "senderName";
				field.Type = TType.String;
				field.ID = 1006;
				oprot.WriteFieldBegin(field);
				oprot.WriteString(SenderName);
				oprot.WriteFieldEnd();
			}
			if (__isset.senderId)
			{
				field.Name = "senderId";
				field.Type = TType.I64;
				field.ID = 1007;
				oprot.WriteFieldBegin(field);
				oprot.WriteI64(SenderId);
				oprot.WriteFieldEnd();
			}
			if (__isset.senderCredentialId)
			{
				field.Name = "senderCredentialId";
				field.Type = TType.I64;
				field.ID = 1008;
				oprot.WriteFieldBegin(field);
				oprot.WriteI64(SenderCredentialId);
				oprot.WriteFieldEnd();
			}
			if (ReceiverName != null && __isset.receiverName)
			{
				field.Name = "receiverName";
				field.Type = TType.String;
				field.ID = 1009;
				oprot.WriteFieldBegin(field);
				oprot.WriteString(ReceiverName);
				oprot.WriteFieldEnd();
			}
			if (__isset.receiverId)
			{
				field.Name = "receiverId";
				field.Type = TType.I64;
				field.ID = 1010;
				oprot.WriteFieldBegin(field);
				oprot.WriteI64(ReceiverId);
				oprot.WriteFieldEnd();
			}
			if (__isset.receiverCredentialId)
			{
				field.Name = "receiverCredentialId";
				field.Type = TType.I64;
				field.ID = 1011;
				oprot.WriteFieldBegin(field);
				oprot.WriteI64(ReceiverCredentialId);
				oprot.WriteFieldEnd();
			}
			oprot.WriteFieldStop();
			oprot.WriteStructEnd();
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("DeltaP2PPaymentMessage(");
			stringBuilder.Append("MessageMetadata: ");
			stringBuilder.Append((MessageMetadata == null) ? "<null>" : MessageMetadata.ToString());
			stringBuilder.Append(",TransferId: ");
			stringBuilder.Append(TransferId);
			stringBuilder.Append(",MessageType: ");
			stringBuilder.Append(MessageType);
			stringBuilder.Append(",IrisSeqId: ");
			stringBuilder.Append(IrisSeqId);
			stringBuilder.Append(",Subtype: ");
			stringBuilder.Append(Subtype);
			stringBuilder.Append(",Amount: ");
			stringBuilder.Append(Amount);
			stringBuilder.Append(",Currency: ");
			stringBuilder.Append(Currency);
			stringBuilder.Append(",CreationTime: ");
			stringBuilder.Append(CreationTime);
			stringBuilder.Append(",CompletedTime: ");
			stringBuilder.Append(CompletedTime);
			stringBuilder.Append(",SenderName: ");
			stringBuilder.Append(SenderName);
			stringBuilder.Append(",SenderId: ");
			stringBuilder.Append(SenderId);
			stringBuilder.Append(",SenderCredentialId: ");
			stringBuilder.Append(SenderCredentialId);
			stringBuilder.Append(",ReceiverName: ");
			stringBuilder.Append(ReceiverName);
			stringBuilder.Append(",ReceiverId: ");
			stringBuilder.Append(ReceiverId);
			stringBuilder.Append(",ReceiverCredentialId: ");
			stringBuilder.Append(ReceiverCredentialId);
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}
	}
}

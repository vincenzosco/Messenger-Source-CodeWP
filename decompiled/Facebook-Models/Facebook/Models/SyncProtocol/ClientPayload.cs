using System.Collections.Generic;
using System.Text;
using Thrift.Protocol;

namespace Facebook.Models.SyncProtocol
{
	public class ClientPayload : TBase, TAbstractBase
	{
		public struct Isset
		{
			public bool deltas;

			public bool firstDeltaSeqId;

			public bool lastIssuedSeqId;

			public bool queueEntityId;

			public bool failedSend;

			public bool syncToken;

			public bool errorCode;
		}

		private List<DeltaWrapper> _deltas;

		private long _firstDeltaSeqId;

		private long _lastIssuedSeqId;

		private long _queueEntityId;

		private FailedSend _failedSend;

		private string _syncToken;

		private string _errorCode;

		public Isset __isset;

		public List<DeltaWrapper> Deltas
		{
			get
			{
				return _deltas;
			}
			set
			{
				__isset.deltas = true;
				_deltas = value;
			}
		}

		public long FirstDeltaSeqId
		{
			get
			{
				return _firstDeltaSeqId;
			}
			set
			{
				__isset.firstDeltaSeqId = true;
				_firstDeltaSeqId = value;
			}
		}

		public long LastIssuedSeqId
		{
			get
			{
				return _lastIssuedSeqId;
			}
			set
			{
				__isset.lastIssuedSeqId = true;
				_lastIssuedSeqId = value;
			}
		}

		public long QueueEntityId
		{
			get
			{
				return _queueEntityId;
			}
			set
			{
				__isset.queueEntityId = true;
				_queueEntityId = value;
			}
		}

		public FailedSend FailedSend
		{
			get
			{
				return _failedSend;
			}
			set
			{
				__isset.failedSend = true;
				_failedSend = value;
			}
		}

		public string SyncToken
		{
			get
			{
				return _syncToken;
			}
			set
			{
				__isset.syncToken = true;
				_syncToken = value;
			}
		}

		public string ErrorCode
		{
			get
			{
				return _errorCode;
			}
			set
			{
				__isset.errorCode = true;
				_errorCode = value;
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
					if (tField.Type == TType.List)
					{
						Deltas = new List<DeltaWrapper>();
						TList tList = iprot.ReadListBegin();
						for (int i = 0; i < tList.Count; i++)
						{
							DeltaWrapper deltaWrapper = new DeltaWrapper();
							deltaWrapper = new DeltaWrapper();
							deltaWrapper.Read(iprot);
							Deltas.Add(deltaWrapper);
						}
						iprot.ReadListEnd();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 2:
					if (tField.Type == TType.I64)
					{
						FirstDeltaSeqId = iprot.ReadI64();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 3:
					if (tField.Type == TType.I64)
					{
						LastIssuedSeqId = iprot.ReadI64();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 4:
					if (tField.Type == TType.I64)
					{
						QueueEntityId = iprot.ReadI64();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 10:
					if (tField.Type == TType.Struct)
					{
						FailedSend = new FailedSend();
						FailedSend.Read(iprot);
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 11:
					if (tField.Type == TType.String)
					{
						SyncToken = iprot.ReadString();
					}
					else
					{
						TProtocolUtil.Skip(iprot, tField.Type);
					}
					break;
				case 12:
					if (tField.Type == TType.String)
					{
						ErrorCode = iprot.ReadString();
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
			TStruct struc = new TStruct("ClientPayload");
			oprot.WriteStructBegin(struc);
			TField field = default(TField);
			if (Deltas != null && __isset.deltas)
			{
				field.Name = "deltas";
				field.Type = TType.List;
				field.ID = 1;
				oprot.WriteFieldBegin(field);
				oprot.WriteListBegin(new TList(TType.Struct, Deltas.Count));
				foreach (DeltaWrapper delta in Deltas)
				{
					delta.Write(oprot);
				}
				oprot.WriteListEnd();
				oprot.WriteFieldEnd();
			}
			if (__isset.firstDeltaSeqId)
			{
				field.Name = "firstDeltaSeqId";
				field.Type = TType.I64;
				field.ID = 2;
				oprot.WriteFieldBegin(field);
				oprot.WriteI64(FirstDeltaSeqId);
				oprot.WriteFieldEnd();
			}
			if (__isset.lastIssuedSeqId)
			{
				field.Name = "lastIssuedSeqId";
				field.Type = TType.I64;
				field.ID = 3;
				oprot.WriteFieldBegin(field);
				oprot.WriteI64(LastIssuedSeqId);
				oprot.WriteFieldEnd();
			}
			if (__isset.queueEntityId)
			{
				field.Name = "queueEntityId";
				field.Type = TType.I64;
				field.ID = 4;
				oprot.WriteFieldBegin(field);
				oprot.WriteI64(QueueEntityId);
				oprot.WriteFieldEnd();
			}
			if (FailedSend != null && __isset.failedSend)
			{
				field.Name = "failedSend";
				field.Type = TType.Struct;
				field.ID = 10;
				oprot.WriteFieldBegin(field);
				FailedSend.Write(oprot);
				oprot.WriteFieldEnd();
			}
			if (SyncToken != null && __isset.syncToken)
			{
				field.Name = "syncToken";
				field.Type = TType.String;
				field.ID = 11;
				oprot.WriteFieldBegin(field);
				oprot.WriteString(SyncToken);
				oprot.WriteFieldEnd();
			}
			if (ErrorCode != null && __isset.errorCode)
			{
				field.Name = "errorCode";
				field.Type = TType.String;
				field.ID = 12;
				oprot.WriteFieldBegin(field);
				oprot.WriteString(ErrorCode);
				oprot.WriteFieldEnd();
			}
			oprot.WriteFieldStop();
			oprot.WriteStructEnd();
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("ClientPayload(");
			stringBuilder.Append("Deltas: ");
			stringBuilder.Append(Deltas);
			stringBuilder.Append(",FirstDeltaSeqId: ");
			stringBuilder.Append(FirstDeltaSeqId);
			stringBuilder.Append(",LastIssuedSeqId: ");
			stringBuilder.Append(LastIssuedSeqId);
			stringBuilder.Append(",QueueEntityId: ");
			stringBuilder.Append(QueueEntityId);
			stringBuilder.Append(",FailedSend: ");
			stringBuilder.Append((FailedSend == null) ? "<null>" : FailedSend.ToString());
			stringBuilder.Append(",SyncToken: ");
			stringBuilder.Append(SyncToken);
			stringBuilder.Append(",ErrorCode: ");
			stringBuilder.Append(ErrorCode);
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}
	}
}

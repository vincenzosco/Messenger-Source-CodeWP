using System.Text;
using Thrift.Protocol;

namespace Facebook.Models.SyncProtocol;

public class VideoMetadata : TBase, TAbstractBase
{
	public struct Isset
	{
		public bool width;

		public bool height;

		public bool durationMs;

		public bool thumbnailUri;

		public bool videoUri;

		public bool source;

		public bool rotation;
	}

	private int _width;

	private int _height;

	private int _durationMs;

	private string _thumbnailUri;

	private string _videoUri;

	private VideoSource _source;

	private int _rotation;

	public Isset __isset;

	public int Width
	{
		get
		{
			return _width;
		}
		set
		{
			__isset.width = true;
			_width = value;
		}
	}

	public int Height
	{
		get
		{
			return _height;
		}
		set
		{
			__isset.height = true;
			_height = value;
		}
	}

	public int DurationMs
	{
		get
		{
			return _durationMs;
		}
		set
		{
			__isset.durationMs = true;
			_durationMs = value;
		}
	}

	public string ThumbnailUri
	{
		get
		{
			return _thumbnailUri;
		}
		set
		{
			__isset.thumbnailUri = true;
			_thumbnailUri = value;
		}
	}

	public string VideoUri
	{
		get
		{
			return _videoUri;
		}
		set
		{
			__isset.videoUri = true;
			_videoUri = value;
		}
	}

	public VideoSource Source
	{
		get
		{
			return _source;
		}
		set
		{
			__isset.source = true;
			_source = value;
		}
	}

	public int Rotation
	{
		get
		{
			return _rotation;
		}
		set
		{
			__isset.rotation = true;
			_rotation = value;
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
					Width = iprot.ReadI32();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 2:
				if (tField.Type == TType.I32)
				{
					Height = iprot.ReadI32();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 3:
				if (tField.Type == TType.I32)
				{
					DurationMs = iprot.ReadI32();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 4:
				if (tField.Type == TType.String)
				{
					ThumbnailUri = iprot.ReadString();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 5:
				if (tField.Type == TType.String)
				{
					VideoUri = iprot.ReadString();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 6:
				if (tField.Type == TType.I32)
				{
					Source = (VideoSource)iprot.ReadI32();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 7:
				if (tField.Type == TType.I32)
				{
					Rotation = iprot.ReadI32();
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
		TStruct struc = new TStruct("VideoMetadata");
		oprot.WriteStructBegin(struc);
		TField field = default;
		if (__isset.width)
		{
			field.Name = "width";
			field.Type = TType.I32;
			field.ID = 1;
			oprot.WriteFieldBegin(field);
			oprot.WriteI32(Width);
			oprot.WriteFieldEnd();
		}
		if (__isset.height)
		{
			field.Name = "height";
			field.Type = TType.I32;
			field.ID = 2;
			oprot.WriteFieldBegin(field);
			oprot.WriteI32(Height);
			oprot.WriteFieldEnd();
		}
		if (__isset.durationMs)
		{
			field.Name = "durationMs";
			field.Type = TType.I32;
			field.ID = 3;
			oprot.WriteFieldBegin(field);
			oprot.WriteI32(DurationMs);
			oprot.WriteFieldEnd();
		}
		if (ThumbnailUri != null && __isset.thumbnailUri)
		{
			field.Name = "thumbnailUri";
			field.Type = TType.String;
			field.ID = 4;
			oprot.WriteFieldBegin(field);
			oprot.WriteString(ThumbnailUri);
			oprot.WriteFieldEnd();
		}
		if (VideoUri != null && __isset.videoUri)
		{
			field.Name = "videoUri";
			field.Type = TType.String;
			field.ID = 5;
			oprot.WriteFieldBegin(field);
			oprot.WriteString(VideoUri);
			oprot.WriteFieldEnd();
		}
		if (__isset.source)
		{
			field.Name = "source";
			field.Type = TType.I32;
			field.ID = 6;
			oprot.WriteFieldBegin(field);
			oprot.WriteI32((int)Source);
			oprot.WriteFieldEnd();
		}
		if (__isset.rotation)
		{
			field.Name = "rotation";
			field.Type = TType.I32;
			field.ID = 7;
			oprot.WriteFieldBegin(field);
			oprot.WriteI32(Rotation);
			oprot.WriteFieldEnd();
		}
		oprot.WriteFieldStop();
		oprot.WriteStructEnd();
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder("VideoMetadata(");
		stringBuilder.Append("Width: ");
		stringBuilder.Append(Width);
		stringBuilder.Append(",Height: ");
		stringBuilder.Append(Height);
		stringBuilder.Append(",DurationMs: ");
		stringBuilder.Append(DurationMs);
		stringBuilder.Append(",ThumbnailUri: ");
		stringBuilder.Append(ThumbnailUri);
		stringBuilder.Append(",VideoUri: ");
		stringBuilder.Append(VideoUri);
		stringBuilder.Append(",Source: ");
		stringBuilder.Append(Source);
		stringBuilder.Append(",Rotation: ");
		stringBuilder.Append(Rotation);
		stringBuilder.Append(")");
		return stringBuilder.ToString();
	}
}

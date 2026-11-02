using System.Collections.Generic;
using System.Text;
using Thrift.Protocol;

namespace Facebook.Models.SyncProtocol;

public class ImageMetadata : TBase, TAbstractBase
{
	public struct Isset
	{
		public bool width;

		public bool height;

		public bool imageURIMap;

		public bool imageSource;

		public bool rawImageURI;

		public bool rawImageURIFormat;

		public bool animatedImageURIMap;

		public bool imageURIMapFormat;

		public bool animatedImageURIMapFormat;

		public bool renderAsSticker;
	}

	private int _width;

	private int _height;

	private Dictionary<int, string> _imageURIMap;

	private ImageSource _imageSource;

	private string _rawImageURI;

	private string _rawImageURIFormat;

	private Dictionary<int, string> _animatedImageURIMap;

	private string _imageURIMapFormat;

	private string _animatedImageURIMapFormat;

	private bool _renderAsSticker;

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

	public Dictionary<int, string> ImageURIMap
	{
		get
		{
			return _imageURIMap;
		}
		set
		{
			__isset.imageURIMap = true;
			_imageURIMap = value;
		}
	}

	public ImageSource ImageSource
	{
		get
		{
			return _imageSource;
		}
		set
		{
			__isset.imageSource = true;
			_imageSource = value;
		}
	}

	public string RawImageURI
	{
		get
		{
			return _rawImageURI;
		}
		set
		{
			__isset.rawImageURI = true;
			_rawImageURI = value;
		}
	}

	public string RawImageURIFormat
	{
		get
		{
			return _rawImageURIFormat;
		}
		set
		{
			__isset.rawImageURIFormat = true;
			_rawImageURIFormat = value;
		}
	}

	public Dictionary<int, string> AnimatedImageURIMap
	{
		get
		{
			return _animatedImageURIMap;
		}
		set
		{
			__isset.animatedImageURIMap = true;
			_animatedImageURIMap = value;
		}
	}

	public string ImageURIMapFormat
	{
		get
		{
			return _imageURIMapFormat;
		}
		set
		{
			__isset.imageURIMapFormat = true;
			_imageURIMapFormat = value;
		}
	}

	public string AnimatedImageURIMapFormat
	{
		get
		{
			return _animatedImageURIMapFormat;
		}
		set
		{
			__isset.animatedImageURIMapFormat = true;
			_animatedImageURIMapFormat = value;
		}
	}

	public bool RenderAsSticker
	{
		get
		{
			return _renderAsSticker;
		}
		set
		{
			__isset.renderAsSticker = true;
			_renderAsSticker = value;
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
				if (tField.Type == TType.Map)
				{
					ImageURIMap = new Dictionary<int, string>();
					TMap tMap = iprot.ReadMapBegin();
					for (int i = 0; i < tMap.Count; i++)
					{
						int key = iprot.ReadI32();
						string value = iprot.ReadString();
						ImageURIMap[key] = value;
					}
					iprot.ReadMapEnd();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 4:
				if (tField.Type == TType.I32)
				{
					ImageSource = (ImageSource)iprot.ReadI32();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 5:
				if (tField.Type == TType.String)
				{
					RawImageURI = iprot.ReadString();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 6:
				if (tField.Type == TType.String)
				{
					RawImageURIFormat = iprot.ReadString();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 7:
				if (tField.Type == TType.Map)
				{
					AnimatedImageURIMap = new Dictionary<int, string>();
					TMap tMap2 = iprot.ReadMapBegin();
					for (int j = 0; j < tMap2.Count; j++)
					{
						int key2 = iprot.ReadI32();
						string value2 = iprot.ReadString();
						AnimatedImageURIMap[key2] = value2;
					}
					iprot.ReadMapEnd();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 8:
				if (tField.Type == TType.String)
				{
					ImageURIMapFormat = iprot.ReadString();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 9:
				if (tField.Type == TType.String)
				{
					AnimatedImageURIMapFormat = iprot.ReadString();
				}
				else
				{
					TProtocolUtil.Skip(iprot, tField.Type);
				}
				break;
			case 10:
				if (tField.Type == TType.Bool)
				{
					RenderAsSticker = iprot.ReadBool();
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
		TStruct struc = new TStruct("ImageMetadata");
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
		if (ImageURIMap != null && __isset.imageURIMap)
		{
			field.Name = "imageURIMap";
			field.Type = TType.Map;
			field.ID = 3;
			oprot.WriteFieldBegin(field);
			oprot.WriteMapBegin(new TMap(TType.I32, TType.String, ImageURIMap.Count));
			foreach (int key in ImageURIMap.Keys)
			{
				oprot.WriteI32(key);
				oprot.WriteString(ImageURIMap[key]);
			}
			oprot.WriteMapEnd();
			oprot.WriteFieldEnd();
		}
		if (__isset.imageSource)
		{
			field.Name = "imageSource";
			field.Type = TType.I32;
			field.ID = 4;
			oprot.WriteFieldBegin(field);
			oprot.WriteI32((int)ImageSource);
			oprot.WriteFieldEnd();
		}
		if (RawImageURI != null && __isset.rawImageURI)
		{
			field.Name = "rawImageURI";
			field.Type = TType.String;
			field.ID = 5;
			oprot.WriteFieldBegin(field);
			oprot.WriteString(RawImageURI);
			oprot.WriteFieldEnd();
		}
		if (RawImageURIFormat != null && __isset.rawImageURIFormat)
		{
			field.Name = "rawImageURIFormat";
			field.Type = TType.String;
			field.ID = 6;
			oprot.WriteFieldBegin(field);
			oprot.WriteString(RawImageURIFormat);
			oprot.WriteFieldEnd();
		}
		if (AnimatedImageURIMap != null && __isset.animatedImageURIMap)
		{
			field.Name = "animatedImageURIMap";
			field.Type = TType.Map;
			field.ID = 7;
			oprot.WriteFieldBegin(field);
			oprot.WriteMapBegin(new TMap(TType.I32, TType.String, AnimatedImageURIMap.Count));
			foreach (int key2 in AnimatedImageURIMap.Keys)
			{
				oprot.WriteI32(key2);
				oprot.WriteString(AnimatedImageURIMap[key2]);
			}
			oprot.WriteMapEnd();
			oprot.WriteFieldEnd();
		}
		if (ImageURIMapFormat != null && __isset.imageURIMapFormat)
		{
			field.Name = "imageURIMapFormat";
			field.Type = TType.String;
			field.ID = 8;
			oprot.WriteFieldBegin(field);
			oprot.WriteString(ImageURIMapFormat);
			oprot.WriteFieldEnd();
		}
		if (AnimatedImageURIMapFormat != null && __isset.animatedImageURIMapFormat)
		{
			field.Name = "animatedImageURIMapFormat";
			field.Type = TType.String;
			field.ID = 9;
			oprot.WriteFieldBegin(field);
			oprot.WriteString(AnimatedImageURIMapFormat);
			oprot.WriteFieldEnd();
		}
		if (__isset.renderAsSticker)
		{
			field.Name = "renderAsSticker";
			field.Type = TType.Bool;
			field.ID = 10;
			oprot.WriteFieldBegin(field);
			oprot.WriteBool(RenderAsSticker);
			oprot.WriteFieldEnd();
		}
		oprot.WriteFieldStop();
		oprot.WriteStructEnd();
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder("ImageMetadata(");
		stringBuilder.Append("Width: ");
		stringBuilder.Append(Width);
		stringBuilder.Append(",Height: ");
		stringBuilder.Append(Height);
		stringBuilder.Append(",ImageURIMap: ");
		stringBuilder.Append(ImageURIMap);
		stringBuilder.Append(",ImageSource: ");
		stringBuilder.Append(ImageSource);
		stringBuilder.Append(",RawImageURI: ");
		stringBuilder.Append(RawImageURI);
		stringBuilder.Append(",RawImageURIFormat: ");
		stringBuilder.Append(RawImageURIFormat);
		stringBuilder.Append(",AnimatedImageURIMap: ");
		stringBuilder.Append(AnimatedImageURIMap);
		stringBuilder.Append(",ImageURIMapFormat: ");
		stringBuilder.Append(ImageURIMapFormat);
		stringBuilder.Append(",AnimatedImageURIMapFormat: ");
		stringBuilder.Append(AnimatedImageURIMapFormat);
		stringBuilder.Append(",RenderAsSticker: ");
		stringBuilder.Append(RenderAsSticker);
		stringBuilder.Append(")");
		return stringBuilder.ToString();
	}
}

namespace Thrift.Protocol;

public struct TMap
{
	private TType keyType;

	private TType valueType;

	private int count;

	public TType KeyType
	{
		get
		{
			return keyType;
		}
		set
		{
			keyType = value;
		}
	}

	public TType ValueType
	{
		get
		{
			return valueType;
		}
		set
		{
			valueType = value;
		}
	}

	public int Count
	{
		get
		{
			return count;
		}
		set
		{
			count = value;
		}
	}

	public TMap(TType keyType, TType valueType, int count)
	{
		this = default;
		this.keyType = keyType;
		this.valueType = valueType;
		this.count = count;
	}
}

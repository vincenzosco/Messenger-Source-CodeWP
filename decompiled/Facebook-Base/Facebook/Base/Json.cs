using Newtonsoft.Json;

namespace Facebook.Base;

public static class Json
{
	public static JsonTextWriter WriteProperty(this JsonTextWriter writer, string name, string value)
	{
		writer.WritePropertyName(name);
		writer.WriteValue(value);
		return writer;
	}

	public static JsonTextWriter WriteProperty(this JsonTextWriter writer, string name, double value)
	{
		writer.WritePropertyName(name);
		writer.WriteValue(value);
		return writer;
	}

	public static JsonTextWriter WriteProperty(this JsonTextWriter writer, string name, long value)
	{
		writer.WritePropertyName(name);
		writer.WriteValue(value);
		return writer;
	}

	public static JsonTextWriter WriteProperty(this JsonTextWriter writer, string name, int value)
	{
		writer.WritePropertyName(name);
		writer.WriteValue(value);
		return writer;
	}
}

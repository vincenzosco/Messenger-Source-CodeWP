using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace Facebook.Base;

public static class Serialization
{
	public static object DeserializeObject(string value, Type type)
	{
		object obj = Activator.CreateInstance(type);
		Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(value);
		IEnumerable<PropertyInfo> declaredProperties = obj.GetType().GetTypeInfo().DeclaredProperties;
		foreach (PropertyInfo item in declaredProperties)
		{
			if (dictionary.ContainsKey(item.Name))
			{
				item.SetValue(obj, Convert(dictionary[item.Name], item.PropertyType));
			}
		}
		return obj;
	}

	private static object Convert(object o, Type type)
	{
		type = Nullable.GetUnderlyingType(type) ?? type;
		object result = null;
		if ((object)type == typeof(string))
		{
			result = System.Convert.ToString(o);
		}
		else if ((object)type == typeof(short))
		{
			result = System.Convert.ToInt16(o);
		}
		else if ((object)type == typeof(int))
		{
			result = System.Convert.ToInt32(o);
		}
		else if ((object)type == typeof(long))
		{
			result = System.Convert.ToInt64(o);
		}
		else if ((object)type == typeof(double))
		{
			result = System.Convert.ToDouble(o);
		}
		else if ((object)type == typeof(decimal))
		{
			result = System.Convert.ToDecimal(o);
		}
		else if ((object)type == typeof(ushort))
		{
			result = System.Convert.ToUInt16(o);
		}
		else if ((object)type == typeof(uint))
		{
			result = System.Convert.ToUInt32(o);
		}
		else if ((object)type == typeof(ulong))
		{
			result = System.Convert.ToUInt64(o);
		}
		else if ((object)type == typeof(DateTime))
		{
			result = System.Convert.ToDateTime(o);
		}
		else if ((object)type == typeof(bool))
		{
			result = System.Convert.ToBoolean(o);
		}
		return result;
	}

	private static T Convert<T>(object o)
	{
		if (o is T)
		{
			return (T)o;
		}
		Type typeFromHandle = typeof(T);
		return (T)Convert(o, typeFromHandle);
	}

	public static T Get<T>(object o)
	{
		if (o != null)
		{
			return Convert<T>(o);
		}
		return default;
	}

	public static T Get<T>(this Dictionary<string, object> result, string name)
	{
		if (!result.ContainsKey(name))
		{
			return default;
		}
		return Get<T>(result[name]);
	}

	public static T Get<T>(this Dictionary<string, string> result, string name)
	{
		if (!result.ContainsKey(name))
		{
			return default;
		}
		return Get<T>(result[name]);
	}
}

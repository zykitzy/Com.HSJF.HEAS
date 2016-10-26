using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Com.HSJF.Infrastructure.ExtendTools
{
	public static class StringExtend
	{
		/// <summary>
		/// string.IsNullOrWhiteSpace简化写法
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static bool IsBlank(this string s)
		{
			return string.IsNullOrWhiteSpace(s);
		}

		/// <summary>
		/// string.Format简化写法
		/// </summary>
		/// <param name="s"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public static string DoFormat(this string format, params object[] args)
		{
			return string.Format(format, args);
		}

		/// <summary>
		/// 将字符串转换为指定枚举，如转换失败则返回null
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="s"></param>
		/// <returns></returns>
		public static T? ToEnum<T>(this string s) where T : struct
		{
			T t;
			return Enum.TryParse(s, out t) ? t : (T?)null;
		}

		/// <summary>
		/// 将字符串转换为指定枚举，如转换失败则返回默认值
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="s"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static T ToEnum<T>(this string s, T defaultValue) where T : struct
		{
			var v = s.ToEnum<T>();
			return v != null ? (T)v : defaultValue;
		}

		/// <summary>
		/// 将字符串转换为指定枚举，如转换失败则抛出InvalidCastException
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="s"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static T TryEnum<T>(this string s) where T : struct
		{
			var v = s.ToEnum<T>();
			if (v != null) return (T)v;
			throw new InvalidCastException();
		}

		/// <summary>
		/// 将字符串转换为byte，如失败则返回null
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static byte? ToByte(this string s)
		{
			byte i;
			return byte.TryParse(s, out i) ? i : (byte?)null;
		}

		/// <summary>
		/// 将字符串转换为byte，如失败则返回默认值
		/// </summary>
		/// <param name="s"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static byte ToByte(this string s, byte defaultValue)
		{
			var v = s.ToByte();
			return v != null ? (byte)v : defaultValue;
		}

		/// <summary>
		/// 将字符串转换为byte，如转换失败则抛出InvalidCastException
		/// </summary>
		/// <param name="s"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static byte TryByte(this string s)
		{
			var v = s.ToByte();
			if (v != null) return (byte)v;
			throw new InvalidCastException();
		}

		/// <summary>
		/// 将字符串转换为short，如失败则返回null
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static short? ToShort(this string s)
		{
			short i;
			return short.TryParse(s, out i) ? i : (short?)null;
		}

		/// <summary>
		/// 将字符串转换为short，如失败则返回默认值
		/// </summary>
		/// <param name="s"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static short ToShort(this string s, short defaultValue)
		{
			var v = s.ToShort();
			return v != null ? (short)v : defaultValue;
		}

		/// <summary>
		/// 将字符串转换为short，如转换失败则抛出InvalidCastException
		/// </summary>
		/// <param name="s"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static short TryShort(this string s)
		{
			var v = s.ToShort();
			if (v != null) return (short)v;
			throw new InvalidCastException();
		}

		/// <summary>
		/// 将字符串转换为int，如失败则返回null
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static int? ToInt(this string s)
		{
			int i;
			return int.TryParse(s, out i) ? i : (int?)null;
		}

		/// <summary>
		/// 将字符串转换为int，如失败则返回默认值
		/// </summary>
		/// <param name="s"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static int ToInt(this string s, int defaultValue)
		{
			var v = s.ToInt();
			return v != null ? (int)v : defaultValue;
		}

		/// <summary>
		/// 将字符串转换为int，如转换失败则抛出InvalidCastException
		/// </summary>
		/// <param name="s"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static int TryInt(this string s)
		{
			var v = s.ToInt();
			if (v != null) return (int)v;
			throw new InvalidCastException();
		}

		/// <summary>
		/// 将字符串转换为long，如失败则返回null
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static long? ToLong(this string s)
		{
			long i;
			return long.TryParse(s, out i) ? i : (long?)null;
		}

		/// <summary>
		/// 将字符串转换为long，如失败则返回默认值
		/// </summary>
		/// <param name="s"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static long ToLong(this string s, long defaultValue)
		{
			var v = s.ToLong();
			return v != null ? (long)v : defaultValue;
		}

		/// <summary>
		/// 将字符串转换为long，如转换失败则抛出InvalidCastException
		/// </summary>
		/// <param name="s"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static long TryLong(this string s)
		{
			var v = s.ToLong();
			if (v != null) return (long)v;
			throw new InvalidCastException();
		}

		/// <summary>
		/// 将字符串转换为decimal，如失败则返回null
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static decimal? ToDecimal(this string s)
		{
			decimal i;
			return decimal.TryParse(s, out i) ? i : (decimal?)null;
		}

		/// <summary>
		/// 将字符串转换为decimal，如失败则返回默认值
		/// </summary>
		/// <param name="s"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static decimal ToDecimal(this string s, decimal defaultValue)
		{
			var v = s.ToDecimal();
			return v != null ? (decimal)v : defaultValue;
		}

		/// <summary>
		/// 将字符串转换为decimal，如转换失败则抛出InvalidCastException
		/// </summary>
		/// <param name="s"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static decimal TryDecimal(this string s)
		{
			var v = s.ToDecimal();
			if (v != null) return (decimal)v;
			throw new InvalidCastException();
		}

		/// <summary>
		/// 将字符串转换为float，如失败则返回null
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static float? ToFloat(this string s)
		{
			float i;
			return float.TryParse(s, out i) ? i : (float?)null;
		}

		/// <summary>
		/// 将字符串转换为float，如失败则返回默认值
		/// </summary>
		/// <param name="s"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static float ToFloat(this string s, float defaultValue)
		{
			var v = s.ToFloat();
			return v != null ? (float)v : defaultValue;
		}

		/// <summary>
		/// 将字符串转换为float，如转换失败则抛出InvalidCastException
		/// </summary>
		/// <param name="s"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static float TryFloat(this string s)
		{
			var v = s.ToFloat();
			if (v != null) return (float)v;
			throw new InvalidCastException();
		}

		/// <summary>
		/// 将字符串转换为double，如失败则返回null
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static double? ToDouble(this string s)
		{
			double v;
			return double.TryParse(s, out v) ? v : (double?)null;
		}

		/// <summary>
		/// 将字符串转换为double，如失败则返回默认值
		/// </summary>
		/// <param name="s"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static double ToDouble(this string s, double defaultValue)
		{
			var v = s.ToDouble();
			return v != null ? (double)v : defaultValue;
		}

		/// <summary>
		/// 将字符串转换为double，如转换失败则抛出InvalidCastException
		/// </summary>
		/// <param name="s"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static double TryDouble(this string s)
		{
			var v = s.ToDouble();
			if (v != null) return (double)v;
			throw new InvalidCastException();
		}

		/// <summary>
		/// 将字符串转换为boolean，如失败则返回null
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static bool? ToBool(this string s)
		{
			bool v;
			if (bool.TryParse(s, out v)) return v;
			s = s.ToLower();
			if (s == "1" || s == "yes" || s == "on" || s == "true") return true;
			if (s == "0" || s == "no" || s == "off" || s == "false") return false;
			return null;
		}

		/// <summary>
		/// 将字符串转换为boolean，如失败则返回null
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static bool? ToBool(this string s, bool defaultValue)
		{
			var v = s.ToBool();
			return v != null ? (bool)v : defaultValue;
		}

		/// <summary>
		/// 将字符串转换为bool，如转换失败则抛出InvalidCastException
		/// </summary>
		/// <param name="s"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static bool TryBool(this string s)
		{
			var v = s.ToBool();
			if (v != null) return (bool)v;
			throw new InvalidCastException();
		}

		/// <summary>
		/// 将字符串转换为DateTime，如失败则返回null
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static DateTime? ToDateTime(this string s)
		{
			DateTime v;
			return DateTime.TryParse(s, out v) ? v : (DateTime?)null;
		}


		/// <summary>
		/// 将字符串转换为DateTime，如失败则返回默认值
		/// </summary>
		/// <param name="s"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static DateTime ToDateTime(this string s, DateTime defaultValue)
		{
			var v = s.ToDateTime();
			return v != null ? (DateTime)v : defaultValue;
		}

		/// <summary>
		/// 将字符串转换为DateTime，如转换失败则抛出InvalidCastException
		/// </summary>
		/// <param name="s"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static DateTime TryDateTime(this string s)
		{
			var v = s.ToDateTime();
			if (v != null) return (DateTime)v;
			throw new InvalidCastException();
		}
	}
}

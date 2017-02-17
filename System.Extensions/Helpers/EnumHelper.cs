// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Collections.Generic;

	public static class EnumHelper
	{
		/// <summary>
		/// Parse a value to an enum.
		/// </summary>
		/// 
		/// <typeparam name="T">
		/// Type of enum.
		/// </typeparam>
		/// 
		/// <param name="value">
		/// Value to parse.
		/// </param>
		/// 
		/// <returns>
		/// The enum.
		/// </returns>
		public static Nullable<T> ParseByValue<T>(int? value)
		   where T : struct
		{
			if (value == null)
				return null;

			if (!Enum.IsDefined(typeof(T), value))
			{
				ThrowException.ThrowInvalidOperationException(string.Format(
					   "The '{0}' enum does not define '{1}' value",
					   typeof(T).ToString(), value));
			}

			return (T)Enum.ToObject(typeof(T), value);
		}

		/// <summary>
		/// Try to parse a value to an enum.
		/// </summary>
		/// 
		/// <typeparam name="T">
		/// Type of enum.
		/// </typeparam>
		/// 
		/// <param name="value">
		/// Value to parse.
		/// </param>
		/// 
		/// <param name="returnEnum">
		/// The returned enum.
		/// </param>
		/// 
		/// <returns>
		/// True if the value has been parsed; otherwise, false.
		/// </returns>
		public static bool TryParseByValue<T>(int value, out T returnEnum)
		   where T : struct
		{
			returnEnum = default(T);

			if (!Enum.IsDefined(typeof(T), value))
				return false;

			returnEnum = (T)Enum.ToObject(typeof(T), value);

			return true;
		}

		/// <summary>
		/// Parse a value to a enum.
		/// </summary>
		/// 
		/// <typeparam name="T">
		/// Type of enum.
		/// </typeparam>
		/// 
		/// <param name="value">
		/// Value to parse.
		/// </param>
		/// 
		/// <param name="defaultValue">
		/// Default value if the value is null.
		/// </param>
		/// 
		/// <returns>
		/// The enum.
		/// </returns>
		public static T ParseByValue<T>(int? value, T defaultValue)
		   where T : struct
		{
			if (value == null)
			{
				return defaultValue;
			}

			if (!Enum.IsDefined(typeof(T), value))
			{
				ThrowException.ThrowInvalidOperationException(string.Format(
					"The '{0}' enum does not define '{1}' value",
					typeof(T).ToString(), value));
			}

			return (T)Enum.ToObject(typeof(T), value);
		}

		/// <summary>
		/// Parse a value to a enum.
		/// </summary>
		/// 
		/// <typeparam name="T">
		/// Type of enum.
		/// </typeparam>
		/// 
		/// <param name="value">
		/// Value to parse.
		/// </param>
		/// 
		/// <param name="defaultValue">
		/// Default value if the value is null or invalid.
		/// </param>
		/// 
		/// <returns>
		/// The enum.
		/// </returns>
		public static T Parse<T>(object value, T defaultValue)
		   where T : struct
		{
			if (value == null)
			{
				return defaultValue;
			}

			try
			{
				return (T)Enum.Parse(typeof(T), value.ToString());
			}
			catch
			{
				return defaultValue;
			}
		}

		/// <summary>
		/// Determines whether a value can be converted to an enum.
		/// </summary>
		/// 
		/// <typeparam name="T">
		/// Type of enum.
		/// </typeparam>
		/// 
		/// <param name="value">
		/// Value to test.
		/// </param>
		/// 
		/// <returns>
		/// True if the value can be converted to an enum; otherwise, false.
		/// </returns>
		public static bool IsDefined<T>(object value)
			where T : struct
		{
			return value != null && Enum.IsDefined(typeof(T), value);
		}

		/// <summary>
		/// Converts an enum to a list.
		/// </summary>
		/// 
		/// <typeparam name="T">
		/// Type of the enum.
		/// </typeparam>
		/// 
		/// <returns>
		/// A list of KeyValuePair items.
		/// </returns>
		public static IList<KeyValuePair<int, string>> ToList<T>()
		{
			IList<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>();

			foreach (T enumItem in Enum.GetValues(typeof(T)))
			{
				list.Add(new KeyValuePair<int, string>(
					Convert.ToInt32(enumItem),
					Enum.GetName(typeof(T), enumItem)));
			}

			return list;
		}
	}
}
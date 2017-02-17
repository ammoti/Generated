// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.ComponentModel;
	using System.Data;

	/// <summary>
	/// Static class for type conversion.
	/// </summary>
	public static class TypeHelper
	{
		/// <summary>
		/// Converts an object to the given type.
		/// </summary>
		/// 
		/// <typeparam name="T">
		/// Convertion type.
		/// </typeparam>
		/// 
		/// <param name="value">
		/// Object to convert.
		/// </param>
		/// 
		/// <returns>
		/// Converted object.
		/// </returns>
		public static T To<T>(object value)
		{
			if (value == DBNull.Value || value == null)
			{
				return default(T);
			}

			var conversionType = typeof(T);
			if (conversionType == null)
			{
				ThrowException.ThrowArgumentNullException("conversionType");
			}

			if (TypeHelper.IsNullable(conversionType))
			{
				var nullableConverter = new NullableConverter(conversionType);
				conversionType = nullableConverter.UnderlyingType;
			}

			if (typeof(T).Equals(typeof(Guid)))
			{
				return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(value.ToString());
			}

			return (T)Convert.ChangeType(value, conversionType);
		}

		/// <summary>
		/// Converts a DataReader item to given type.
		/// </summary>
		/// 
		/// <typeparam name="T">
		/// Convertion type.
		/// </typeparam>
		/// 
		/// <param name="dbReader">
		/// DataReader object.
		/// </param>
		/// 
		/// <param name="columnName">
		/// Column name.
		/// </param>
		/// 
		/// <returns>
		/// Converted object.
		/// </returns>
		public static T To<T>(IDataReader dbReader, string columnName)
		{
			return TypeHelper.To<T>(dbReader[columnName]);
		}

		/// <summary>
		/// Converts a DataRow item to given type.
		/// </summary>
		/// 
		/// <typeparam name="T">
		/// Convertion type.
		/// </typeparam>
		/// 
		/// <param name="row">
		/// DataRow object.
		/// </param>
		/// 
		/// <param name="columnName">
		/// Column name.
		/// </param>
		/// 
		/// <returns>
		/// Converted object.
		/// </returns>
		public static T To<T>(DataRow row, string columnName)
		{
			return TypeHelper.To<T>(row[columnName]);
		}

		/// <summary>
		/// Indicates whether the specified type is nullable.
		/// </summary>
		/// 
		/// <param name="type">
		/// Type to test.
		/// </param>
		/// 
		/// <returns>
		/// True if the type is nullable; otherwise, false.
		/// </returns>
		public static bool IsNullable(Type type)
		{
			return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
		}
	}
}

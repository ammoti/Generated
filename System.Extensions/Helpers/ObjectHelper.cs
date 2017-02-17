// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.IO;
	using System.Runtime.Serialization.Formatters.Binary;

	public static class ObjectHelper
	{
		public static byte[] ToByteArray(object obj)
		{
			if (obj == null)
			{
				return null;
			}

			using (var mStream = new MemoryStream())
			{
				var bFormatter = new BinaryFormatter();
				bFormatter.Serialize(mStream, obj);

				return mStream.ToArray();
			}
		}

		public static object ToObject(byte[] bytes)
		{
			return ObjectHelper.ToObject<object>(bytes);
		}

		public static T ToObject<T>(byte[] bytes)
		{
			if (bytes == null)
			{
				return default(T);
			}

			using (var mStream = new MemoryStream())
			{
				var bFormatter = new BinaryFormatter();

				mStream.Write(bytes, 0, bytes.Length);
				mStream.Seek(0, SeekOrigin.Begin);

				return (T)bFormatter.Deserialize(mStream);
			}
		}

		/// <summary>
		/// Indicates whether the value can be null ( bool bIsNullable = theVariable.IsNullable() ).
		/// </summary>
		/// <typeparam name="T">Type of the object</typeparam>
		/// <param name="obj">Object.</param>
		/// <returns>True if the value can be null; otherwise, false.</returns>
		public static bool IsNullable(object obj)
		{
			if (obj == null)
			{
				return true;
			}

			Type type = obj.GetType();

			if (!type.IsValueType)
			{
				return true;
			}

			if (Nullable.GetUnderlyingType(type) != null)
			{
				return true;
			}

			return false;
		}
	}
}

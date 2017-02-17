// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	public static class TypeExtensions
	{
		/// <summary>
		/// Determine whether a type is simple (string, decimal, DateTime, etc).
		/// </summary>
		/// 
		/// <param name="type">
		/// The type.
		/// </param>
		/// 
		/// <returns>
		/// True if the type is simple; otherwise, false.
		/// </returns>
		public static bool IsSimpleType(this Type type)
		{
			return type.IsValueType || type.IsPrimitive || type == typeof(string) || Convert.GetTypeCode(type) != TypeCode.Object;
		}
	}
}
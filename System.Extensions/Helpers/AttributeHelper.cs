// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	/// <summary>
	/// Attribute helper class.
	/// </summary>
	public static class AttributeHelper
	{
		/// <summary>
		/// Retrieves the specified attributes of a property.
		/// </summary>
		/// 
		/// <typeparam name="T">
		/// The attribute type to retrieve.
		/// </typeparam>
		/// 
		/// <param name="property">
		/// Target property.
		/// </param>
		/// 
		/// <param name="inherit">
		/// True to search this member's inheritance chain to find the attributes; otherwise, false.
		/// </param>
		/// 
		/// <returns>
		/// The list of the attributes.
		/// </returns>
		public static IList<T> GetAttributes<T>(PropertyInfo property, bool inherit = false) where T : Attribute
		{
			return ((T[])property.GetCustomAttributes(typeof(T), inherit)).ToList();
		}

		/// <summary>
		/// Retrieves the specified attributes of a method.
		/// </summary>
		/// 
		/// <typeparam name="T">
		/// The attribute type to retrieve.
		/// </typeparam>
		/// 
		/// <param name="method">
		/// Target method.
		/// </param>
		/// 
		/// <param name="inherit">
		/// True to search this member's inheritance chain to find the attributes; otherwise, false.
		/// </param>
		/// 
		/// <returns>
		/// The list of the attributes.
		/// </returns>
		public static IEnumerable<T> GetAttributes<T>(MethodInfo method, bool inherit = false) where T : Attribute
		{
			return (T[])method.GetCustomAttributes(typeof(T), inherit);
		}
	}
}

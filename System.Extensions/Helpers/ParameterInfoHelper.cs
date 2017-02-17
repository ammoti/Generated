// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System.Reflection
{
	/// <summary>
	/// ParameterInfo helper class.
	/// </summary>
	public class ParameterInfoHelper
	{
		/// <summary>
		/// Gets the real parameter type.
		/// </summary>
		/// 
		/// <param name="parameter">
		/// Parameter.
		/// </param>
		/// 
		/// <returns>
		/// The real type of the parameter.
		/// </returns>
		public static Type GetParameterType(ParameterInfo parameter)
		{
			Type type = parameter.ParameterType;
			return type.IsByRef ? type.GetElementType() : type;
		}

		/// <summary>
		/// Gets the value indicating whether the parameter is passed by reference (ref, out).
		/// </summary>
		/// 
		/// <param name="parameter">
		/// Parameter.
		/// </param>
		/// 
		/// <returns>
		/// True if the parameter is passed by reference; otherwise, false.
		/// </returns>
		public static bool IsRef(ParameterInfo parameter)
		{
			Type type = parameter.ParameterType;
			return type.IsByRef;
		}
	}
}

// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	public static class Int16Extensions
	{
		/// <summary>
		/// Gets the value indicating whether the input value is null or 0.
		/// </summary>
		/// 
		/// <param name="value">
		/// The value to check.
		/// </param>
		/// 
		/// <returns>
		/// True if the input value is null or 0; otherwise, false.
		/// </returns>
		public static bool IsNullOrZero(this short value)
		{
			return value == 0;
		}

		/// <summary>
		/// Gets the value indicating whether the input value is null or 0.
		/// </summary>
		/// 
		/// <param name="value">
		/// The value to check.
		/// </param>
		/// 
		/// <returns>
		/// True if the input value is null or 0; otherwise, false.
		/// </returns>
		public static bool IsNullOrZero(this short? value)
		{
			return value == null || value == 0;
		}
	}
}

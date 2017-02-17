// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.Core
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public static partial class Cultures
	{
		/// <summary>
		/// Gets the default culture (EN).
		/// </summary>
		public static readonly string Default = "EN";

		/// <summary>
		/// Determines whether the culture is supported.
		/// </summary>
		/// 
		/// <param name="culture">
		/// Culture to check.
		/// </param>
		/// 
		/// <param name="ignoreCase">
		/// Value indicating whether the case is ignored.
		/// </param>
		/// 
		/// <returns>
		/// True if the culture is supported; otherwise, false.
		/// </returns>
		public static bool IsSupported(string culture, bool ignoreCase = true)
		{
			return (culture != null)
				? CultureList.Any(i => string.Compare(i, culture, ignoreCase) == 0)
				: false;
		}

		/// <summary>
		/// Determines whether the culture is the default one.
		/// </summary>
		/// 
		/// <param name="culture">
		/// Culture to check.
		/// </param>
		/// 
		/// <param name="ignoreCase">
		/// Value indicating whether the case is ignored.
		/// </param>
		/// 
		/// <returns>
		/// True if the culture is the default one; otherwise, false.
		/// </returns>
		public static bool IsDefault(string culture, bool ignoreCase = true)
		{
			return (culture != null)
				? string.Compare(culture, Default, ignoreCase) == 0
				: false;
		}
	}
}
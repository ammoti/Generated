// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Globalization;
	using System.Linq;
	using System.Web;

	public static class CultureHelper
	{
		/// <summary>
		/// Gets all the cultures supported by the user's browser.
		/// </summary>
		/// 
		/// <returns>
		/// All the cultures supported by the user's browser.
		/// </returns>
		public static CultureInfo[] GetBowserCultures()
		{
			var cultures = new CultureInfo[0];

			if (HttpContext.Current.Request != null &&
				HttpContext.Current.Request.UserLanguages != null &&
				HttpContext.Current.Request.UserLanguages.Length != 0)
			{
				cultures = HttpContext.Current.Request.UserLanguages
					.Select(i => i.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)[0])
					.Select(i => new CultureInfo(i)).ToArray();
			}

			return cultures;
		}

		/// <summary>
		/// Gets the culture installed with the operating system.
		/// </summary>
		/// 
		/// <returns>
		/// The culture installed with the operating system.
		/// </returns>
		public static CultureInfo GetOperatingSystemCulture()
		{
			return CultureInfo.InstalledUICulture;
		}
	}
}

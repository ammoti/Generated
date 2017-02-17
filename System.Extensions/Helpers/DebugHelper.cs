// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Globalization;
	using System.Threading;

	public static class DebugHelper
	{
		public static void SetEnglishExceptionMessages()
		{
			SetSpecificLanguageExceptionMessages("en-US");
		}

		public static void SetSpecificLanguageExceptionMessages(string culture)
		{
			Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(culture);
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
		}
	}
}

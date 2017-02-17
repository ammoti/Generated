// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.UI
{
	using System;
	using System.Reflection;
	using System.Windows;

	public static class AppHelper
	{
		public static string AppName
		{
			get
			{
				return (Application.ResourceAssembly != null) ?
					Application.ResourceAssembly.GetName().Name.Replace('.', ' ') :
					"N.C.";
			}
		}

		public static string AppVersion
		{
			get
			{
				Version version = Assembly.GetExecutingAssembly().GetName().Version;
				return version.ToString().Substring(0, 5);
			}
		}
	}
}

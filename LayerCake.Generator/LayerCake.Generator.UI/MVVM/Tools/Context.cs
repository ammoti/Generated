// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.UI.Tools
{
	using System.Windows;

	public static class Context
	{
		#region [ IsRequired ]

		public static readonly DependencyProperty IsRequiredProperty = DependencyProperty.RegisterAttached(
			"IsRequired",
			typeof(bool),
			typeof(Context),
			new FrameworkPropertyMetadata(false));

		public static bool GetIsRequired(DependencyObject dependencyObject)
		{
			return (bool)dependencyObject.GetValue(IsRequiredProperty);
		}

		public static void SetIsRequired(DependencyObject dependencyObject, bool value)
		{
			dependencyObject.SetValue(IsRequiredProperty, value);
		}

		#endregion
	}
}

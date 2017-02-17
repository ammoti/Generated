// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.UI.Converters
{
	using System;
	using System.Globalization;
	using System.Windows.Data;
	using System.Windows.Shell;

	public class BoolToTaskbarItemProgressState : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((bool)value == true)
			{
				return TaskbarItemProgressState.Error;
			}

			return TaskbarItemProgressState.Normal;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}

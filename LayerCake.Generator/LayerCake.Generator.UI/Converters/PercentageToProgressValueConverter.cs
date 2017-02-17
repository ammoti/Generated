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

	public class PercentageToProgressValueConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			double res = 0.0d;
			if (value is int)
			{
				int intVal = (int)value;
				res = intVal / 100.0d;
				if (res < 0.0d)
					res = 0.0d;
				else if (res > 100.0d)
					res = 100.0d;
			}
			return res;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}

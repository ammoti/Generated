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

	public class NotConverter : IValueConverter
	{
		#region [ IValueConverter Implementation ]

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool)
			{
				return !((bool)value);
			}
			else if (value is bool?)
			{
				return !((bool?)value).Value;
			}

			return Binding.DoNothing;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}

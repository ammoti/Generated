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
	using System.Windows;
	using System.Windows.Data;

	public class BoolToVisibilityConverter : IValueConverter
	{
		public bool Not
		{
			get;
			set;
		}

		#region [ IValueConverter Implementation ]

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool && true == (bool)value ^ this.Not)
			{
				return Visibility.Visible;
			}

			return (parameter is string && string.Compare(parameter.ToString(), "Hidden", ignoreCase: true) == 0) ? Visibility.Hidden : Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}

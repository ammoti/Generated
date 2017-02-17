// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.UI.Converters
{
	using System;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using System.Windows.Controls;
	using System.Windows.Data;

	public class ValidationErrorConverter : IValueConverter
	{
		#region [ IValueConverter Implementation ]

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var errors = value as ReadOnlyObservableCollection<ValidationError>;
			if (errors != null)
			{
				if (errors.Count != 0)
				{
					return errors[0].ErrorContent;
				}
			}

			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}

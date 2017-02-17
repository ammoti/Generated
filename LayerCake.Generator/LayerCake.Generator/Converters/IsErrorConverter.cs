
namespace LayerCake.Generator.Converters
{
	using LayerCake.Generator.Commons;

	using System;
	using System.Globalization;
	using System.Windows.Data;

	public class IsErrorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is ProcessMessageType)
			{
				return (ProcessMessageType)value == ProcessMessageType.Error;
			}

			return Binding.DoNothing;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

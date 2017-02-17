
namespace LayerCake.Generator.Converters
{
	using LayerCake.Generator.Commons;

	using System;
	using System.Globalization;
	using System.Windows.Data;

	public class ProcessMessageConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			ProcessMessageEventArgs e = value as ProcessMessageEventArgs;
			if (e != null)
			{
				if (e.Type == ProcessMessageType.Error)
				{
					return string.Format("* {0}", e.Message);
				}
			}

			return e.Message;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

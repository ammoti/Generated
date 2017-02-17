// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.UI
{
	using System.Windows;
	using System.Windows.Threading;

	public static class MessageBoxHelper
	{
		public static MessageBoxResult Show(string message, string caption, MessageBoxButton button, MessageBoxImage image)
		{
			MessageBoxResult result = MessageBoxResult.None;

			if (Application.Current != null)
			{
				Application.Current.Dispatcher.InvokeAction(() =>
				{
					result = MessageBox.Show(message, caption, button, image);
				},
				DispatcherPriority.Normal);
			}
			else
			{
				result = MessageBox.Show(message, caption, button, image);
			}

			return result;
		}
	}
}

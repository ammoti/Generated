// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.UI
{
	using System;
	using System.Windows.Threading;

	public static class DispatcherExtension
	{
		public static void InvokeAction(this Dispatcher dispatcher, Action action, DispatcherPriority priority)
		{
			if (dispatcher.CheckAccess())
			{
				action.Invoke();
			}
			else
			{
				dispatcher.Invoke(action, priority);
			}
		}

		public static void BeginInvokeAction(this Dispatcher dispatcher, Action action, DispatcherPriority priority)
		{
			if (dispatcher.CheckAccess())
			{
				action.Invoke();
			}
			else
			{
				dispatcher.BeginInvoke(action, priority);
			}
		}
	}
}

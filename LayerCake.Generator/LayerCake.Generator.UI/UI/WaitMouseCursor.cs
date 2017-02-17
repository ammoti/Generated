// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.UI
{
	using System;
	using System.Windows;
	using System.Windows.Input;
	using System.Windows.Threading;

	public class WaitMouseCursor : IDisposable
	{
		public WaitMouseCursor(Cursor waitCursor = null)
		{
			Application.Current.Dispatcher.InvokeAction(() =>
			{
				Mouse.OverrideCursor = (waitCursor != null) ? waitCursor : Cursors.Wait;
			},
			DispatcherPriority.Normal);
		}

		~WaitMouseCursor()
		{
			this.Dispose(false);
		}

		public static void SetMouseCursor(bool isLoading)
		{
			Application.Current.Dispatcher.InvokeAction(() =>
			{
				Mouse.OverrideCursor = (isLoading) ? Cursors.Wait : null;
			},
			DispatcherPriority.Normal);
		}

		#region [ IDisposable Implementation ]

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			Application.Current.Dispatcher.InvokeAction(() =>
			{
				Mouse.OverrideCursor = null;
			},
			DispatcherPriority.Normal);
		}

		#endregion
	}
}

// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.UI
{
	using System.Windows;

	public static class WindowHelper
	{
		public static void Activate(Window window)
		{
			if (window != null)
			{
				if (!window.IsVisible)
				{
					window.Show();
				}

				if (window.WindowState == WindowState.Minimized)
				{
					window.WindowState = WindowState.Normal;
				}

				window.Activate();

				window.Topmost = true;
				window.Topmost = false;

				window.Focus();
			}
		}
	}
}

// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.Views
{
	using LayerCake.Generator.UI;
	using LayerCake.Generator.UI.Windows;

	using System;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;

	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			this.Closing += OnClosing;

			this.SizeChanged += OnSizeChanged;
			this.LocationChanged += OnLocationChanged;

			this.SetPreferences();

			this.SetWindowTitle();
			this.SetWindowSize();
			this.SetWindowPosition();

			WindowHelper.Activate(this);
		}

		private void OnClosing(object sender, CancelEventArgs e)
		{
			if (this.CancelButton.Visibility == Visibility.Visible) // Process running -> cannot quit.
			{
				e.Cancel = true;
			}
		}

		private void OnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (this.IsLoaded)
			{
				PreferenceManager.SizeHeight = (int)Application.Current.MainWindow.Height;
				PreferenceManager.SizeWidth = (int)Application.Current.MainWindow.Width;
			}
		}

		private void OnLocationChanged(object sender, EventArgs e)
		{
			if (this.IsLoaded)
			{
				PreferenceManager.LocationX = (int)Application.Current.MainWindow.Left;
				PreferenceManager.LocationY = (int)Application.Current.MainWindow.Top;
			}
		}

		private void SetPreferences()
		{
			this.CheckAutomaticUpdatesMenuItem.IsChecked = PreferenceManager.WithCheckAutomaticUpdates;
			this.SendProcessReportsMenuItem.IsChecked = PreferenceManager.WithSendProcessReports;
			this.SendErrorReportsMenuItem.IsChecked = PreferenceManager.WithSendErrorReports;

			PreferenceManager.ExecutionCount++;
			PreferenceManager.LastExecutionTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		}

		private void SetWindowTitle()
		{
			this.Title = string.Format("{0} v{1}", AppHelper.AppName, AppHelper.AppVersion);
		}

		private void SetWindowPosition()
		{
			if (PreferenceManager.LocationX >= 0 && PreferenceManager.LocationY >= 0)
			{
				this.WindowStartupLocation = WindowStartupLocation.Manual;

				Application.Current.MainWindow.Left = PreferenceManager.LocationX;
				Application.Current.MainWindow.Top = PreferenceManager.LocationY;
			}
			else
			{
				this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			}
		}

		private void SetWindowSize()
		{
			const int minHeight = 640;
			const int minWidth = 720;

			if (PreferenceManager.LocationX == 0 && PreferenceManager.LocationY == 0)
			{
				this.WindowState = WindowState.Maximized;
			}
			else if (PreferenceManager.SizeHeight < minHeight && PreferenceManager.SizeWidth < minWidth)
			{
				Application.Current.MainWindow.Height = minHeight;
				Application.Current.MainWindow.Width = minWidth;
			}
			else
			{
				Application.Current.MainWindow.Height = PreferenceManager.SizeHeight;
				Application.Current.MainWindow.Width = PreferenceManager.SizeWidth;
			}
		}

		private void OnCheckAutomaticUpdatesMenuItemClick(object sender, RoutedEventArgs e)
		{
			PreferenceManager.WithCheckAutomaticUpdates = ((MenuItem)sender).IsChecked;
		}

		private void OnSendProcessReportsMenuItemClick(object sender, RoutedEventArgs e)
		{
			PreferenceManager.WithSendProcessReports = ((MenuItem)sender).IsChecked;
		}

		private void OnSendErrorReportsMenuItemClick(object sender, RoutedEventArgs e)
		{
			PreferenceManager.WithSendErrorReports = ((MenuItem)sender).IsChecked;
		}

		private void OnLicenseMenuItemClick(object sender, RoutedEventArgs e)
		{
			Window window = new LicenseWindow();

			window.Owner = this;
			window.ShowDialog();
		}

		private void OnAboutMenuItemClick(object sender, RoutedEventArgs e)
		{
			Window window = new AboutWindow();

			window.Owner = this;
			window.ShowDialog();
		}

		private void OnUpdateAvailableButtonClick(object sender, RoutedEventArgs e)
		{
			Process.Start("http://www.layercake-generator.net/");
		}

		private void OnLogoImageClick(object sender, MouseButtonEventArgs e)
		{
			Process.Start("http://www.layercake-generator.net/");
		}
	}
}

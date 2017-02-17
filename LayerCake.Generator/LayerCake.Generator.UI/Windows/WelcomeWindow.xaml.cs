// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.UI.Windows
{
	using System;
	using System.Diagnostics;
	using System.IO;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Navigation;

	public partial class WelcomeWindow : Window
	{
		public WelcomeWindow()
		{
			InitializeComponent();

			this.SetWindowTexts();
		}

		private void SetWindowTexts()
		{
			this.TitleTextBlock.Text = string.Format(this.TitleTextBlock.Text, AppHelper.AppName);
			this.ThankTextBlock.Text = string.Format(this.ThankTextBlock.Text, AppHelper.AppName, AppHelper.AppVersion);
		}

		private void OnWebSiteRequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
			e.Handled = true;
		}

		private void OnNeverShowPopupChecked(object sender, RoutedEventArgs e)
		{
			PreferenceManager.IsFirstRun = !((CheckBox)sender).IsChecked.Value;
		}

		private void OnContinueClick(object sender, RoutedEventArgs e)
		{
			base.Close();
		}

		private void OnDocumentationClick(object sender, RoutedEventArgs e)
		{
			string htmlLocalPath = @"Documentation\Index.html";

			string indexPagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, htmlLocalPath);
			if (File.Exists(indexPagePath))
			{
				Process.Start(indexPagePath);
			}
			else
			{
				MessageBox.Show("Damn! I can't find the local documentation :(", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}

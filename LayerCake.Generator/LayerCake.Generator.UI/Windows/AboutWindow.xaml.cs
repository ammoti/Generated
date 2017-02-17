// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.UI.Windows
{
	using System.Diagnostics;
	using System.Windows;
	using System.Windows.Navigation;

	public partial class AboutWindow : Window
	{
		public AboutWindow()
		{
			InitializeComponent();

			this.SetTitle();
		}

		private void SetTitle()
		{
			this.AppNameTextBox.Text = string.Format("{0} v{1}", AppHelper.AppName, AppHelper.AppVersion);
		}

		private void OnWebSiteRequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
			e.Handled = true;
		}

		private void OnCloseClick(object sender, RoutedEventArgs e)
		{
			base.Close();
		}
	}
}

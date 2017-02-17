// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator
{
	using LayerCake.Generator.UI;
	using LayerCake.Generator.UI.Windows;

	using System;
	using System.Globalization;
	using System.IO;
	using System.Linq;
	using System.Threading;
	using System.Windows;
	using System.Windows.Forms;

	public partial class App : System.Windows.Application
	{
		static App()
		{
			AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
			Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

			#region [ #BUG-001 ]

			// The type initializer for 'LayerCake.Generator.App' threw an exception.
			// The calling thread must be STA, because many UI components require this.

			// When using VS, sometimes (after full recompilation) the process is run twice, breakpoints seem to be KO and the TaskManager shows only 1 process.
			// There is a ghost process that fails with PreferenceManager.IsFirstRun (returns True instead of False)...

			// VS issue?

			//if (PreferenceManager.IsFirstRun)
			//{
			//	System.Windows.MessageBox.Show(string.Format(
			//	   "The App is starting -> {0} and exit (PreferenceManager.IsFirstRun = {1})",
			//	   System.Diagnostics.Process.GetCurrentProcess().Id,
			//	   PreferenceManager.IsFirstRun),
			//	   "");
			//}

			//System.Windows.MessageBox.Show(string.Format(
			//	"The App is starting -> {0} (PreferenceManager.IsFirstRun = {1})",
			//	System.Diagnostics.Process.GetCurrentProcess().Id,
			//	PreferenceManager.IsFirstRun),
			//	"");

			#endregion

			if (PreferenceManager.IsFirstRun)
			{
				Window window = new WelcomeWindow();

				window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
				window.ShowDialog();
			}

			if (!App.SelectConfigFile())
			{
				Environment.Exit(-1);
			}

			if (App.IsWrongConfigFile)
			{
				MessageBoxResult result = MessageBoxHelper.Show(
					"Wrong file!\r\n\r\nIt seems that you haven't read the documentation of the product.\r\nDo you want to report to the documentation?",
					"Wrong file!",
					MessageBoxButton.YesNo,
					MessageBoxImage.Stop);

				if (result == MessageBoxResult.Yes)
				{
					System.Diagnostics.Process.Start("http://www.layercake-generator.net/Documentation/Index.html");
				}

				Environment.Exit(-3);
			}
		}

		private static bool SelectConfigFile()
		{
			bool bContinue = false;

			using (var fileDialog = new OpenFileDialog())
			{
				fileDialog.Title = "Select a LayerCake Generator configuration file...";
				fileDialog.Filter = "Configuration files (*.json) |*.json";

				string dirPath = null;
				string lastFilePath = PreferenceManager.LastConfigFilePath;

				if (lastFilePath != null)
				{
					dirPath = Path.GetDirectoryName(lastFilePath);

					if (!Directory.Exists(dirPath))
					{
						dirPath = Path.Combine(dirPath, "..");
					}
				}

				if (dirPath == null || !Directory.Exists(dirPath))
				{
					dirPath = Path.GetPathRoot(Environment.SystemDirectory);
				}

				fileDialog.InitialDirectory = Path.GetFullPath(dirPath);

				DialogResult result = fileDialog.ShowDialog();
				if (result == DialogResult.OK)
				{
					App.ConfigFilePath = fileDialog.FileName;
					bContinue = true;
				}
			}

			return bContinue;
		}

		private static bool IsWrongConfigFile
		{
			get
			{
				string fileName = Path.GetFileName(App.ConfigFilePath);

				string[] wrongFiles = new string[3]
				{
					"LayerCake.Generator.exe.config",
					"LayerCake.Generator.Cmd.exe.config",
					"LayerCake.Cooker.exe.config"
				};

				return wrongFiles.Contains(fileName);
			}
		}

		public static string ConfigFilePath
		{
			get;
			private set;
		}

		private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			if (e.ExceptionObject != null)
			{
				Exception x = e.ExceptionObject as Exception;
				if (x != null)
				{
					ServiceManager.SubmitExceptionAsync(x);

					MessageBoxHelper.Show(
						string.Format("An exception occurred: {0}", x.GetFullMessage(true, "\r\n\r\n")),
						"Exception",
						MessageBoxButton.OK,
						MessageBoxImage.Error);
				}
			}
		}
	}
}

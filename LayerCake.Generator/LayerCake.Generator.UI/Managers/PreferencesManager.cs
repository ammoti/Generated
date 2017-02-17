// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.UI
{
	using System;

	public static class PreferenceManager
	{
		#region [ Constants ]

		private static readonly string _rootKeyName = @"HKEY_CURRENT_USER\Software\{0}\v{1}";

		#endregion

		#region [ Constructor ]

		static PreferenceManager()
		{
			_rootKeyName = string.Format(_rootKeyName, AppHelper.AppName, AppHelper.AppVersion);

			RegistryHelper.SetValue(_rootKeyName, null /* default entry */, AppHelper.AppVersion); // creates the entry if not exists
		}

		#endregion

		#region [ Methods ]

		public static void SetLcgLastConfigFilePath(string value) // Hack method
		{
			string keyName = string.Format(@"HKEY_CURRENT_USER\Software\LayerCake Generator\v{0}", AppHelper.AppVersion);

			if (!RegistryHelper.HasValue(keyName, DataLastConfigFilePath))
			{
				RegistryHelper.SetValue(keyName, DataLastConfigFilePath, value);
			}
		}

		#endregion

		#region [ Properties ]

		public static bool IsFirstRun
		{
			get
			{
				return RegistryHelper.GetValue<int>(_rootKeyName, DataIsFirstRun, 1) == 1;
			}
			set
			{
				RegistryHelper.SetValue(_rootKeyName, DataIsFirstRun, value ? 1 : 0);
			}
		}

		public static string LastConfigFilePath
		{
			get
			{
				return RegistryHelper.GetValue<string>(_rootKeyName, DataLastConfigFilePath);
			}
			set
			{
				RegistryHelper.SetValue(_rootKeyName, DataLastConfigFilePath, value);
			}
		}

		public static int ExecutionCount
		{
			get
			{
				int value = RegistryHelper.GetValue<int>(_rootKeyName, StatsExecutionCount);

				if (value == -1 || value == int.MaxValue)
					value = 1;

				return value;
			}
			set
			{
				RegistryHelper.SetValue(_rootKeyName, StatsExecutionCount, value);
			}
		}

		public static string LastExecutionTime
		{
			get
			{
				return RegistryHelper.GetValue<string>(_rootKeyName, StatsLastExecutionTime);
			}
			set
			{
				RegistryHelper.SetValue(_rootKeyName, StatsLastExecutionTime, value);
			}
		}

		public static int ProcessCount
		{
			get
			{
				int value = RegistryHelper.GetValue<int>(_rootKeyName, StatsProcessCount);

				if (value == int.MaxValue)
					value = 1;

				return value;
			}
			set
			{
				RegistryHelper.SetValue(_rootKeyName, StatsProcessCount, value);
			}
		}

		public static bool WithCheckAutomaticUpdates
		{
			get
			{
				return RegistryHelper.GetValue<int>(_rootKeyName, OptionsWithCheckAutomaticUpdates, 1) == 1;
			}
			set
			{
				RegistryHelper.SetValue(_rootKeyName, OptionsWithCheckAutomaticUpdates, value ? 1 : 0);
			}
		}

		public static bool WithSendProcessReports
		{
			get
			{
				return RegistryHelper.GetValue<int>(_rootKeyName, OptionsWithSendProcessReports, 1) == 1;
			}
			set
			{
				RegistryHelper.SetValue(_rootKeyName, OptionsWithSendProcessReports, value ? 1 : 0);
			}
		}

		public static bool WithSendErrorReports
		{
			get
			{
				return RegistryHelper.GetValue<int>(_rootKeyName, OptionsWithSendErrorReports, 1) == 1;
			}
			set
			{
				RegistryHelper.SetValue(_rootKeyName, OptionsWithSendErrorReports, value ? 1 : 0);
			}
		}

		public static int LocationX
		{
			get
			{
				return RegistryHelper.GetValue<int>(_rootKeyName, UILocationX, -1);
			}
			set
			{
				if (value < 0) value = 0;
				RegistryHelper.SetValue(_rootKeyName, UILocationX, value);
			}
		}

		public static int LocationY
		{
			get
			{
				return RegistryHelper.GetValue<int>(_rootKeyName, UILocationY, -1);
			}
			set
			{
				if (value < 0) value = 0;
				RegistryHelper.SetValue(_rootKeyName, UILocationY, value);
			}
		}

		public static int SizeHeight
		{
			get
			{
				return RegistryHelper.GetValue<int>(_rootKeyName, UISizeHeight);
			}
			set
			{
				RegistryHelper.SetValue(_rootKeyName, UISizeHeight, value);
			}
		}

		public static int SizeWidth
		{
			get
			{
				return RegistryHelper.GetValue<int>(_rootKeyName, UISizeWidth);
			}
			set
			{
				RegistryHelper.SetValue(_rootKeyName, UISizeWidth, value);
			}
		}

		#endregion

		#region [ Constants ]

		private static readonly string DataIsFirstRun = "Data.IsFirstRun";
		private static readonly string DataLastConfigFilePath = "Data.LastConfigFilePath";

		private static readonly string StatsExecutionCount = "Stats.ExecutionCount";
		private static readonly string StatsLastExecutionTime = "Stats.LastExecutionTime";
		private static readonly string StatsProcessCount = "Stats.ProcessCount";

		private static readonly string OptionsWithCheckAutomaticUpdates = "Options.WithCheckAutomaticUpdates";
		private static readonly string OptionsWithSendProcessReports = "Options.WithSendProcessReports";
		private static readonly string OptionsWithSendErrorReports = "Options.WithSendErrorReports";

		private static readonly string UILocationX = "UI.Location.X";
		private static readonly string UILocationY = "UI.Location.Y";

		private static readonly string UISizeHeight = "UI.Size.Height";
		private static readonly string UISizeWidth = "UI.Size.Width";

		#endregion
	}
}

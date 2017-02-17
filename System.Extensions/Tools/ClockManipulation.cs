// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Diagnostics;
	using System.IO;

	public static class ClockManipulation
	{
		/// <summary>
		/// Determines whether the clock has been manipulated in past.
		/// </summary>
		/// 
		/// <param name="thresholdTime">
		/// The date and time to validate (default value is DateTime.Now).
		/// </param>
		/// 
		/// <returns>
		/// True if the clock has been manipulated in past; otherwise, false.
		/// </returns>
		public static bool HasBeenManipulated(DateTime? thresholdTime = null)
		{
			if (thresholdTime == null)
			{
				thresholdTime = DateTime.Now;
			}

			if (DetectClockManipulationFromEventLog(thresholdTime.Value))
			{
				return true;
			}

			if (DetectClockManipulationFromFiles(thresholdTime.Value))
			{
				return true;
			}

			return false;
		}

		private static bool DetectClockManipulationFromEventLog(DateTime thresholdTime)
		{
			try
			{
				DateTime adjustedThresholdTime = thresholdTime.Date.AddDays(1).AddSeconds(-1);
				EventLog eventLog = new EventLog("system");

				foreach (EventLogEntry entry in eventLog.Entries)
				{
					if (entry.TimeWritten > adjustedThresholdTime)
					{
						return true;
					}
				}
			}
			catch
			{
				if (Debugger.IsAttached)
					Debugger.Break();
			}

			return false;
		}

		private static bool DetectClockManipulationFromFiles(DateTime thresholdTime)
		{
			try
			{
				DateTime adjustedThresholdTime = thresholdTime.Date.AddDays(1).AddSeconds(-1);
				DirectoryInfo dirInfo = new DirectoryInfo(Path.GetTempPath());

				foreach (var dir in dirInfo.GetDirectories())
				{
					if (dir.CreationTime > adjustedThresholdTime)
					{
						return true;
					}
				}

				foreach (var file in dirInfo.GetFiles())
				{
					if (file.CreationTime > adjustedThresholdTime)
					{
						return true;
					}
				}
			}
			catch
			{
				if (Debugger.IsAttached)
					Debugger.Break();
			}

			return false;
		}
	}
}

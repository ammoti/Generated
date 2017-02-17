// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	public static class DateTimeHelper
	{
		public static DateTime AddDaysWithoutWeekEndDays(DateTime dateTime, int value)
		{
			int p = 0;
			int daysToAdd = (value < 0) ? -1 : 1;

			DateTime newDateTime = dateTime;

			while (p < value)
			{
				newDateTime = newDateTime.AddDays(daysToAdd);

				if (newDateTime.DayOfWeek != DayOfWeek.Saturday && newDateTime.DayOfWeek != DayOfWeek.Sunday)
				{
					p += 1;
				}
			}

			return newDateTime;
		}

		/// <summary>
		/// Gets the DateTime of the first day of the week.
		/// </summary>
		/// 
		/// <param name="source">
		/// The DateTime source.
		/// </param>
		/// 
		/// <returns>
		/// The DateTime of the first day of the week.
		/// </returns>
		public static DateTime GetFirstDayOfWeek(this DateTime source)
		{
			int delta = DayOfWeek.Monday - source.DayOfWeek;
			if (delta > 0) delta -= 7;

			return source.AddDays(delta).Date;
		}

		/// <summary>
		/// Gets the DateTime of the last day of the week.
		/// </summary>
		/// 
		/// <param name="source">
		/// The DateTime source.
		/// </param>
		/// 
		/// <returns>
		/// The DateTime of the last day of the week.
		/// </returns>
		public static DateTime GetLastDayOfWeek(this DateTime source)
		{
			return source.GetFirstDayOfWeek().AddDays(6).AddHours(23).AddMinutes(59).AddSeconds(59);
		}
	}
}

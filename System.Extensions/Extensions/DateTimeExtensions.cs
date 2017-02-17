// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Collections.Generic;
	using System.Linq;

	public static class DateTimeExtensions
	{
		/// <summary>
		/// Indicates whether the value is included in the [start, end] interval.
		/// </summary>
		/// 
		/// <param name="value">
		/// Value.
		/// </param>
		/// 
		/// <param name="start">
		/// Begin DateTime value.
		/// </param>
		/// 
		/// <param name="end">
		/// End DateTime value.
		/// </param>
		/// 
		/// <returns>
		/// True if the value is included in the [start, end] interval; otherwise, false.
		/// </returns>
		public static bool IsIncluded(this DateTime value, DateTime? start, DateTime? end)
		{
			if (start == null && end == null)
				return true;

			if (start == null)
				return value <= end;

			if (end == null)
				return value >= start;

			var range = new DateTimeRange(start, end);

			return range.Contains(value);
		}

		/// <summary>
		/// Indicates whether the value is included in the [start, end] interval.
		/// </summary>
		/// 
		/// <param name="value">
		/// Value.
		/// </param>
		/// 
		/// <param name="start">
		/// Begin DateTime value.
		/// </param>
		/// 
		/// <param name="end">
		/// End DateTime value.
		/// </param>
		/// 
		/// <returns>
		/// True if the value is included in the [start, end] interval; otherwise, false.
		/// </returns>
		public static bool IsIncluded(this DateTime? value, DateTime? start, DateTime? end)
		{
			if (value == null)
				return true;

			return value.Value.IsIncluded(start, end);
		}

		/// <summary>
		/// Indicates whether the value is included in one of the [start, end] intervals from the object.
		/// </summary>
		/// 
		/// <typeparam name="T">
		/// Type of the sequence.
		/// </typeparam>
		/// 
		/// <param name="value">
		/// Value.
		/// </param>
		/// 
		/// <param name="sequence">
		/// Sequence that contains the intervals.
		/// </param>
		/// 
		/// <param name="beginDateSelector">
		/// Begin DateTime selector.
		/// </param>
		/// 
		/// <param name="endDateSelector">
		/// End DateTime selector.
		/// </param>
		/// 
		/// <returns>
		/// True if the value is included in one of the [start, end] intervals from the object.
		/// </returns>
		public static bool IsIncluded<T>(this DateTime value, IEnumerable<T> sequence, Func<T, DateTime?> startDateSelector, Func<T, DateTime?> endDateSelector)
		{
			if (sequence.IsNullOrEmpty())
				return false;

			return sequence.Any(i => value.IsIncluded(startDateSelector(i), endDateSelector(i)));
		}

		/// <summary>
		/// Indicates whether the value is included in one of the [start, end] intervals from the object.
		/// </summary>
		/// 
		/// <typeparam name="T">
		/// Type of the object.
		/// </typeparam>
		/// 
		/// <param name="value">
		/// Value.
		/// </param>
		/// 
		/// <param name="sequence">
		/// Sequence that contains the intervals.
		/// </param>
		/// 
		/// <param name="beginDateSelector">
		/// Begin DateTime selector.
		/// </param>
		/// 
		/// <param name="endDateSelector">
		/// End DateTime selector.
		/// </param>
		/// 
		/// <returns>
		/// True if the value is included in one of the [start, end] intervals from the object.
		/// </returns>
		public static bool IsIncluded<T>(this DateTime? value, IEnumerable<T> sequence, Func<T, DateTime?> startDateSelector, Func<T, DateTime?> endDateSelector)
		{
			if (sequence.IsNullOrEmpty())
				return false;

			if (value == null) // at least 1 item in the sequence
				return true;

			return sequence.Any(i => value.IsIncluded(startDateSelector(i), endDateSelector(i)));
		}
	}
}

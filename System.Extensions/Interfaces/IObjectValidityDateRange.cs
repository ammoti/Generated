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

	/// <summary>
	/// Objets that implements the IObjectValidityDateRange interface has a notion of validity date range.
	/// </summary>
	public interface IObjectValidityDateRange
	{
		/// <summary>
		/// Begin date of the interval.
		/// </summary>
		DateTime? ValidityBeginDate
		{
			get;
		}

		/// <summary>
		/// End date of the interval.
		/// </summary>
		DateTime? ValidityEndDate
		{
			get;
		}
	}

	public static class IObjectDateValidationRangeExtensions
	{
		public static bool IsValidityDateRangeValid(this IObjectValidityDateRange source, DateTime? dateTime = null)
		{
			return dateTime.IsIncluded(source.ValidityBeginDate, source.ValidityEndDate);
		}

		public static bool IsValidityDateRangeValid(this IObjectValidityDateRange source, DateTime? beginDate, DateTime? endDate)
		{
			IList<DateTimeRange> sequence = new List<DateTimeRange>();
			sequence.Add(new DateTimeRange(beginDate, endDate));

			return source.IsValidityDateRangeValid<DateTimeRange>(sequence, f => f.Minimum, f => f.Maximum);
		}

		public static bool IsValidityDateRangeValid(this IObjectValidityDateRange source, IEnumerable<IObjectValidityDateRange> sequence)
		{
			return source.IsValidityDateRangeValid<IObjectValidityDateRange>(sequence, f => f.ValidityBeginDate, f => f.ValidityEndDate);
		}

		public static bool IsValidityDateRangeValid<T>(this IObjectValidityDateRange source, IEnumerable<T> sequence, Func<T, DateTime?> beginDateSelector, Func<T, DateTime?> endDateSelector)
		{
			if (source == null)
			{
				ThrowException.ThrowArgumentNullException("source");
			}

			if (!sequence.Any())
				return false;

			if (source.ValidityBeginDate == null && source.ValidityEndDate == null)
				return true;

			DateTimeRange range = new DateTimeRange(source.ValidityBeginDate, source.ValidityEndDate);

			if (sequence.Any(i => new DateTimeRange(beginDateSelector(i), endDateSelector(i)).Overlaps(range)))
				return true;

			return false;
		}
	}
}

// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.Core
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public static class FilterListExtensions
	{
		/// <summary>
		/// Add a filter.
		/// </summary>
		/// <param name="filters">Filter collection.</param>
		/// <param name="filter">Filter to add.</param>
		public static void Add(this IList<Filter> filters, Filter filter)
		{
			if (filter != null)
			{
				filters.Add(filter);
			}
		}

		/// <summary>
		/// Add a filter.
		/// </summary>
		/// <param name="filters">Filter collection.</param>
		/// <param name="orGroup">Or group (filters with the same group value will be linked with AND operand. OR operand is used between filters having different group value).</param>
		/// <param name="columnName">Column name.</param>
		/// <param name="filterOperator">Filter operator.</param>
		/// <param name="value">Value to add.</param>
		/// <param name="accentSensitivity">Accent sensitivity enumeration.</param>
		public static void Add(this IList<Filter> filters, int orGroup, string columnName, FilterOperator filterOperator, object value, AccentSensitivity accentSensitivity = AccentSensitivity.Without)
		{
			if (!string.IsNullOrEmpty(columnName))
			{
				filters.Add(new Filter(orGroup, columnName, filterOperator, value, accentSensitivity));
			}
		}

		/// <summary>
		/// Add a filter.
		/// </summary>
		/// <param name="filters">Filter collection.</param>
		/// <param name="columnName">Column name.</param>
		/// <param name="filterOperator">Filter operator.</param>
		/// <param name="value">Value to add.</param>
		/// <param name="accentSensitivity">Accent sensitivity enumeration.</param>
		public static void Add(this IList<Filter> filters, string columnName, FilterOperator filterOperator, object value, AccentSensitivity accentSensitivity = AccentSensitivity.Without)
		{
			filters.Add(0, columnName, filterOperator, value, accentSensitivity);
		}

		/// <summary>
		/// Gets the formatted filters.
		/// </summary>
		/// <param name="filters">Filter collection.</param>
		/// <returns>The formatted filters.</returns>
		public static string ToSql(this IList<Filter> filters)
		{
			const string orOp = " OR ";
			const string andOp = " AND ";

			var sbPattern = new StringBuilder(128);
			var groupingFilters = filters.GroupBy(f => f.OrGroup);

			int i = 0;
			foreach (IGrouping<int, Filter> filterGroup in groupingFilters)
			{
				sbPattern.Append("("); // New group id

				int j = 0;
				foreach (Filter filter in filterGroup)
				{
					sbPattern.Append(filter.ToString());

					if (j < filterGroup.Count() - 1)
					{
						sbPattern.Append(andOp);
					}
					j++;
				}

				sbPattern.Append(")");

				if (i < groupingFilters.Count() - 1)
				{
					sbPattern.Append(orOp);
				}
				i++;
			}

			return sbPattern.ToString();
		}
	}
}

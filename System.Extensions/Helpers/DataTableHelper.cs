// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System.Data
{
	using System.Collections.Generic;
	using System.Text;

	public static class DataTableHelper
	{
		/// <summary>
		/// Filters a DataTable given an expression.
		/// </summary>
		/// 
		/// <param name="table">
		/// DataTable to filter.
		/// </param>
		/// 
		/// <param name="expression">
		/// Filter expression (COLUMNNAME = '{0}').
		/// </param>
		/// 
		/// <param name="parameters">
		/// Optional parameters.
		/// </param>
		/// 
		/// <returns>
		/// The filtered DataTable.
		/// </returns>
		public static DataTable Filter(DataTable table, string expression, params object[] parameters)
		{
			if (table == null)
			{
				ThrowException.ThrowArgumentNullException("table");
			}

			if (string.IsNullOrEmpty(expression))
			{
				ThrowException.ThrowArgumentNullException("expression");
			}

			if (table.Rows.Count == 0)
			{
				return table;
			}

			DataTable tmpDt = new DataTable();
			DataRow[] rows = table.Select(!parameters.IsNullOrEmpty() ? string.Format(expression, parameters) : expression);

			if (rows != null && rows.Length != 0)
			{
				foreach (DataRow row in rows)
				{
					tmpDt.ImportRow(row);
				}
			}

			return tmpDt;
		}

		public static void Sort(ref DataTable table, string columnName, string direction = "ASC")
		{
			if (table == null)
			{
				ThrowException.ThrowArgumentNullException("table");
			}

			if (string.IsNullOrEmpty(columnName))
			{
				ThrowException.ThrowArgumentNullException("columnName");
			}

			table.DefaultView.Sort = string.Format("{0} {1}", columnName, direction);
		}

		/// <summary>
		/// Remove duplicated rows given one of many column names.
		/// Usefull when merging 2 DataTables.
		/// </summary>
		/// 
		/// <param name="table">
		/// DataTable.
		/// </param>
		/// 
		/// <param name="columnNames">
		/// Column names.
		/// </param>
		public static void RemoveDuplicates(this DataTable table, params string[] columnNames)
		{
			if (table == null)
			{
				ThrowException.ThrowArgumentNullException("table");
			}

			if (columnNames.IsNullOrEmpty())
			{
				ThrowException.ThrowArgumentException("columnNames", "The 'columnNames' parameter must have at least 1 column name");
			}

			var taggedRowList = new List<string>(table.Rows.Count);
			var idToRemoveList = new List<int>();

			int idx = 0;

			foreach (DataRow row in table.Rows)
			{
				StringBuilder sbRowTag = new StringBuilder();

				foreach (string columnName in columnNames)
				{
					sbRowTag.Append((row[columnName] != DBNull.Value ? row[columnName].ToString() : string.Empty));
				}

				if (taggedRowList.Contains(sbRowTag.ToString()))
				{
					idToRemoveList.Add(idx);
				}
				else
				{
					taggedRowList.Add(sbRowTag.ToString());
				}

				idx++;
			}

			idToRemoveList.Reverse();

			foreach (var i in idToRemoveList)
			{
				table.Rows.RemoveAt(i);
			}
		}
	}
}

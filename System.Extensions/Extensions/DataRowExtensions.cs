// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System.Data
{
	public static class DataRowExtensions
	{
		public static bool HasColumn(this DataRow row, string columnName)
		{
			for (int i = 0; i < row.Table.Columns.Count; i++)
			{
				if (row.Table.Columns[i].ColumnName.Equals(columnName))
				{
					return true;
				}
			}

			return false;
		}
	}
}
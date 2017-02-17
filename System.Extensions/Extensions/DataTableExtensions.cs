// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System.Data
{
	public static class DataTableExtensions
	{
		public static bool HasColumn(this DataTable dt, string columnName)
		{
			return dt.Columns.Contains(columnName);
		}
	}
}
// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System.Data
{
	public static class IDataReaderExtensions
	{
		public static bool HasColumn(this IDataReader dr, string columnName)
		{
			for (int i = 0; i < dr.FieldCount; i++)
			{
				if (dr.GetName(i).Equals(columnName))
				{
					return true;
				}
			}

			return false;
		}
	}
}
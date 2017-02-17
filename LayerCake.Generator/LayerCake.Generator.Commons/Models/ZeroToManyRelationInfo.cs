// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.Commons
{
	public class ZeroToManyRelationInfo
	{
		public string TableName { get; set; }

		public string ColumnName { get; set; }

		public string ReferencedTableName { get; set; }

		public string ReferencedColumnName { get; set; }

		public bool IsLoop
		{
			get
			{
				return string.Compare(this.TableName, this.ReferencedTableName, true) == 0;
			}
		}
	}
}

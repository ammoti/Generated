// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.Commons
{
	using Microsoft.SqlServer.Management.Smo;

	public class SmoForeignKeyColumnInfo
	{
		public Table ReferencedTable { get; set; }

		public Column ReferencedColumn { get; set; }
	}
}

// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator
{
	class TableInfo
	{
		public TableInfo()
		{
		}

		public TableInfo(string name)
		{
			this.Name = name;
		}

		public string Name
		{
			get;
			set;
		}

		public override string ToString()
		{
			return this.Name;
		}
	}
}

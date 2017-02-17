// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.Commons
{
	using System.Collections.Generic;
	using System.Collections.Specialized;

	public class LanguageColumnInfo
	{
		public LanguageColumnInfo()
		{
			this.IsLanguageColumn = false;

			this.LanguageColumnNames = new List<string>();
			this.DefaultValues = new StringDictionary();
		}

		public bool IsLanguageColumn { get; set; }

		public IList<string> LanguageColumnNames { get; set; }

		public string ColumnNameWithoutCulture { get; set; }

		public bool IsNullable { get; set; }

		public StringDictionary DefaultValues { get; set; }
	}
}

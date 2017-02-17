// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	sealed public class CriteriaMemberAttribute : Attribute
	{
		/// <summary>
		/// Name of the Sql parameter.
		/// </summary>
		public string SqlParameterName
		{
			get;
			set;
		}

		/// <summary>
		/// Value indicating whether the property is used in Full-Text Search.
		/// </summary>
		public bool IsFullTextSearch
		{
			get;
			set;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// 
		/// <param name="sqlParameterName">
		/// Name of the Sql parameter.
		/// </param>
		public CriteriaMemberAttribute(string sqlParameterName, bool isFullTextSearch = false)
		{
			if (string.IsNullOrWhiteSpace(sqlParameterName))
			{
				ThrowException.ThrowArgumentNullException("sqlParameterName");
			}

			if (!sqlParameterName.StartsWith("@"))
			{
				ThrowException.ThrowFormatException("The sqlParameterName value must start with the '@' character");
			}

			this.SqlParameterName = sqlParameterName;
			this.IsFullTextSearch = isFullTextSearch;
		}
	}
}

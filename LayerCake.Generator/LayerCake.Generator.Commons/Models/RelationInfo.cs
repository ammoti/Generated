// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.Commons
{
	public class RelationInfo
	{
		public bool IsLoop
		{
			get
			{
				return string.Compare(this.TableName, this.ReferencedTableName, true) == 0;
			}
		}

		public override string ToString()
		{
			return string.Format("TableName = {0}, ColumnName = {1}, PropertyName = {2}, CollectionName = {3}, ReferencedTableName = {4}, ReferencedColumnName = {5}, ReferencedColumnNameWithoutTableName = {6}",
				this.TableName, this.ColumnName, this.PropertyName, this.CollectionName, this.ReferencedTableName, this.ReferencedColumnName, this.ReferencedColumnNameWithoutTableName);
		}

		public string TableName { get; set; }

		public string ColumnName { get; set; }

		public string PropertyName { get; set; }

		public string CollectionName { get; set; }

		public string ReferencedTableName { get; set; }

		public string ReferencedColumnName { get; set; }

		public string ReferencedColumnNameWithoutTableName { get; set; }
	}

	public enum RelationType
	{
		/// <summary>
		/// (0, 1) relation.
		/// </summary>
		ZeroToOne,

		/// <summary>
		/// (0, n) relation.
		/// </summary>
		ZeroToMany
	}
}

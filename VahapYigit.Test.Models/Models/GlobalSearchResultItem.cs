// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.Models
{
	using System;
	using System.Diagnostics;
	using System.Runtime.Serialization;

	using VahapYigit.Test.Core;

	[Serializable]
	[DataContract(Namespace = Globals.Namespace, IsReference = true)]
	[DebuggerDisplay("Table = {TableName}, Column = {ColumnName}, Record ID = {Id}, Value Found = {Value}")]
	public class GlobalSearchResultItem // : ModelBase <- not needed for this class
	{
		#region [ Properties ]

		[DataMember]
		public string TableName { get; set; }

		[DataMember]
		public string ColumnName { get; set; }

		[DataMember]
		public long Id { get; set; }

		[DataMember]
		public string Value { get; set; }

		#endregion
	}
}
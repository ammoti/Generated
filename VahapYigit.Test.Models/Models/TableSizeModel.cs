// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.Models
{
	using System;
	using System.Runtime.Serialization;

	using VahapYigit.Test.Core;

	[Serializable]
	[DataContract(Namespace = Globals.Namespace, IsReference = true)]
	public class TableSizeModel // : ModelBase <- not needed for this class
	{
		#region [ Properties ]

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public int RowCounts { get; set; }

		[DataMember]
		public double TableSizeInMB { get; set; }

		[DataMember]
		public double IndexSizeInMB { get; set; }

		[DataMember]
		public double TotalSizeInMB
		{
			get { return this.TableSizeInMB + this.IndexSizeInMB; }
		}

		#endregion
	}
}
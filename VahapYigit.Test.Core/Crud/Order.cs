// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.Core
{
	using System;
	using System.Runtime.Serialization;

	/// <summary>
	/// Order class.
	/// </summary>
	[Serializable]
	[DataContract(Namespace = Globals.Namespace, Name = "VahapYigit.Test.Core.Order", IsReference = true)]
	public class Order
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		private Order()
		{
			this.Operator = OrderOperator.Asc;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="columnName">Column name.</param>
		/// <param name="orderOperator">Order operator.</param>
		public Order(string columnName, OrderOperator orderOperator = OrderOperator.Asc)
			: this()
		{
			this.ColumnName = columnName;
			this.Operator = orderOperator;
		}

		/// <summary>
		/// Gets the formatted Order operator.
		/// </summary>
		/// <returns>The formatted Order operator.</returns>
		public override string ToString()
		{
			if (this.Operator == OrderOperator.Desc)
			{
				return string.Format("[{0}] DESC", this.ColumnName);
			}
			else
			{
				return string.Format("[{0}] ASC", this.ColumnName);
			}
		}

		#region [ Properties ]

		/// <summary>
		/// Gets or sets the column name.
		/// </summary>
		[DataMember]
		public string ColumnName
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the Order operator.
		/// </summary>
		[DataMember]
		public OrderOperator Operator
		{
			get;
			set;
		}

		#endregion
	}
}

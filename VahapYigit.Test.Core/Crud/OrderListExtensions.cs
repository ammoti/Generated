// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.Core
{
	using System.Collections.Generic;
	using System.Text;

	public static class OrderListExtensions
	{
		/// <summary>
		/// Add an Order.
		/// </summary>
		/// <param name="orders">Order collection.</param>
		/// <param name="order">Order to add.</param>
		public static void Add(this IList<Order> orders, Order order)
		{
			if (order != null)
			{
				orders.Add(order);
			}
		}

		/// <summary>
		/// Add an Order.
		/// </summary>
		/// <param name="orders">Order collection.</param>
		/// <param name="columnName">Column name.</param>
		/// <param name="orderOperator">Order operator.</param>
		public static void Add(this IList<Order> orders, string columnName, OrderOperator orderOperator = OrderOperator.Asc)
		{
			if (!string.IsNullOrEmpty(columnName))
			{
				orders.Add(new Order(columnName, orderOperator));
			}
		}

		/// <summary>
		/// Gets the formatted Orders.
		/// </summary>
		/// <param name="orders">Order collection.</param>
		/// <returns>The formatted Orders.</returns>
		public static string ToSql(this IList<Order> orders)
		{
			var sbPattern = new StringBuilder(128);

			for (int i = 0; i < orders.Count; i++)
			{
				sbPattern.Append(orders[i].ToString());
				if (i < orders.Count - 1) { sbPattern.Append(", "); }
			}

			return sbPattern.ToString();
		}
	}
}

// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.Core
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;

	/// <summary>
	/// Search options class. Used by Crud Search methods.
	/// </summary>
	[Serializable]
	[DataContract(Namespace = Globals.Namespace, Name = "VahapYigit.Test.Core.SearchOptions", IsReference = true)]
	public class SearchOptions
	{
		/// <summary>
		/// Value == int.MaxValue
		/// </summary>
		private static readonly int MAX_RECORDS = int.MaxValue;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public SearchOptions()
		{
			this.SetDefaultValues();
		}

		/// <summary>
		/// Reinitialize the instance with default values (without filter, without orderby, without max records limit and without paging).
		/// </summary>
		public void Clear()
		{
			this.SetDefaultValues();
		}

		/// <summary>
		/// Set default properties values.
		/// </summary>
		private void SetDefaultValues()
		{
			this.Filters.Clear();
			this.Orders.Clear();

			this.MaxRecords = MAX_RECORDS;

			this.WithPaging = false;
			this.PagingOptions = new PagingOptions { RecordsPerPage = 50 };
		}

		#region [ Properties ]

		private List<Filter> _filters;
		/// <summary>
		/// Gets the filters.
		/// </summary>
		[DataMember]
		public List<Filter> Filters
		{
			get
			{
				if (_filters == null)
				{
					_filters = new List<Filter>();
				}

				return _filters;
			}
			set { _filters = value; }
		}

		private List<Order> _orders;
		/// <summary>
		/// Gets or sets the Orders .
		/// </summary>
		[DataMember]
		public List<Order> Orders
		{
			get
			{
				if (_orders == null)
				{
					_orders = new List<Order>();
				}

				return _orders;
			}
			set { _orders = value; }
		}

		/// <summary>
		/// Set paging option values.
		/// </summary>
		/// <param name="pagingPage">Value indicating the current page.</param>
		/// <param name="pagingItemCount">Value indicating the number of items to get.</param>
		public void UsePaging(int currentPage, int recordsPerPage)
		{
			this.WithPaging = true;
			this.PagingOptions = new PagingOptions { CurrentPage = currentPage, RecordsPerPage = recordsPerPage };
		}

		private int _maxRecords;
		/// <summary>
		/// Gets or sets the max records returned. If value equals to 0 then no limit is set.
		/// </summary>
		[DataMember]
		public int MaxRecords
		{
			get { return _maxRecords; }
			set
			{
				if (value < 0)
				{
					_maxRecords = 0;
				}
				else if (value == 0)
				{
					_maxRecords = MAX_RECORDS;
				}
				else
				{
					_maxRecords = value;
				}
			}
		}

		private bool _withPaging;
		/// <summary>
		/// Gets or sets the WithPaging value. Default value is false.
		/// </summary>
		[DataMember]
		public bool WithPaging
		{
			get { return _withPaging && _pagingOptions != null && _pagingOptions.RecordsPerPage > 0; }
			set { _withPaging = value; }
		}

		private PagingOptions _pagingOptions;
		/// <summary>
		/// Gets or sets the PagingOptions instance.
		/// </summary>
		[DataMember]
		public PagingOptions PagingOptions
		{
			get { return _pagingOptions; }
			set { _pagingOptions = value; }
		}

		#endregion
	}
}

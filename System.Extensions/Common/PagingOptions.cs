// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Runtime.Serialization;

	[Serializable]
	[DataContract(IsReference = true)]
	public class PagingOptions
	{
		#region [ Constructor ]

		public PagingOptions()
		{
			this.CurrentPage = 1;
			this.RecordsPerPage = 50;
		}

		#endregion

		#region [ Properties ]

		private int _currentPage;
		/// <summary>
		/// Gets or sets the current page value. Default value is 1.
		/// </summary>
		[DataMember]
		public int CurrentPage
		{
			get { return _currentPage; }
			set { _currentPage = (value < 1) ? 1 : value; }
		}

		private int _recordsPerPage;
		/// <summary>
		/// Gets or sets the number of records per page value. Default value is 50.
		/// </summary>
		[DataMember]
		public int RecordsPerPage
		{
			get { return _recordsPerPage; }
			set { _recordsPerPage = (value < 1) ? 50 : value; }
		}

		private int _totalRecords;
		/// <summary>
		/// Gets the total of records returned by a Search operation when the result is not paged.
		/// </summary>
		[DataMember]
		public int TotalRecords
		{
			get { return _totalRecords; }
			set { _totalRecords = value; }
		}

		#endregion
	}
}

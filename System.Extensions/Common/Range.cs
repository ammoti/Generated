// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	public class Range<T> where T : IComparable<T>
	{
		#region [ Properties ]

		/// <summary>
		/// Minimum value of the range.
		/// </summary>
		public T Minimum { get; set; }

		/// <summary>
		/// Maximum value of the range.
		/// </summary>
		public T Maximum { get; set; }

		#endregion

		#region [ Constructors ]

		/// <summary>
		/// Constructor.
		/// </summary>
		public Range()
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// 
		/// <param name="minimun">
		/// Minimum value of the range.
		/// </param>
		/// 
		/// <param name="maximum">
		/// Maximum value of the range.
		/// </param>
		public Range(T minimun, T maximum)
		{
			this.Minimum = minimun;
			this.Maximum = maximum;
		}

		#endregion

		#region [ Methods ]

		/// <summary>
		/// Determines whether the range is valid.
		/// </summary>
		/// 
		/// <returns>
		/// True if range is valid; otherwise, false.
		/// </returns>
		public bool IsValid()
		{
			return this.Minimum.CompareTo(this.Maximum) <= 0;
		}

		/// <summary>
		/// Determines whether the provided value is inside the range.
		/// </summary>
		/// 
		/// <param name="value">
		/// The value to test.
		/// </param>
		/// 
		/// <returns>
		/// True if the value is inside Range; otherwise, false.
		/// </returns>
		public bool Contains(T value)
		{
			return (this.Minimum.CompareTo(value) <= 0) && (value.CompareTo(this.Maximum) <= 0);
		}

		/// <summary>
		/// Determines whether this range is inside the bounds of another range.
		/// </summary>
		/// 
		/// <param name="range">
		/// The parent range to test on.
		/// </param>
		/// 
		/// <returns>
		/// True if range is inclusive; otherwise, false.
		/// </returns>
		public bool IsInsideRange(Range<T> range)
		{
			return this.IsValid() && range.IsValid() && range.Contains(this.Minimum) && range.Contains(this.Maximum);
		}

		/// <summary>
		/// Determines whether another range is inside the bounds of this range.
		/// </summary>
		/// 
		/// <param name="range">
		/// The child range to test.
		/// </param>
		/// 
		/// <returns>
		/// True if range is inside; otherwise, false.
		/// </returns>
		public bool ContainsRange(Range<T> range)
		{
			return this.IsValid() && range.IsValid() && this.Contains(range.Minimum) && this.Contains(range.Maximum);
		}

		/// <summary>
		/// Determines whether ranges overlaps.
		/// </summary>
		/// 
		/// <param name="range">
		/// The child range to test.
		/// </param>
		/// 
		/// <returns>
		/// True if the ranges overlap; otherwise, false.
		/// </returns>
		public bool Overlaps(Range<T> range)
		{
			return this.ContainsRange(range) || this.IsInsideRange(range) || this.Contains(range.Minimum) || this.Contains(range.Maximum);
		}

		/// <summary>
		/// Returns a string that represents the current range.
		/// </summary>
		/// 
		/// <returns>
		/// A string that represents the current range.
		/// </returns>
		public override string ToString()
		{
			return string.Format("[{0} - {1}]", this.Minimum, this.Maximum);
		}

		#endregion
	}
}

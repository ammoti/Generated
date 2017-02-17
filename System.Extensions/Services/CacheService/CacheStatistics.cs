// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Threading;

	[DebuggerDisplay("ItemCount = {ItemCount}, RequestCount = {RequestCount}, HitCount = {HitCount}, MissCount = {MissCount}")]
	public class CacheStatistics : ICacheStatistics
	{
		#region [ Members ]

		//private readonly ICollection _items = null;

		#endregion

		#region [ Constructor ]

		public CacheStatistics()
		{
		}

		#endregion

		#region [ ICacheStatistics Implementation ]

		/// <summary>
		/// Gets the value indicating the number of items in the cache.
		/// </summary>
		public long ItemCount { get; set; }
			
		private long _requestCount = 0;
		/// <summary>
		/// Gets or sets the value indicating the number of requests.
		/// </summary>
		public long RequestCount
		{
			get { return _requestCount; }
		}

		private long _hitCount = 0;
		/// <summary>
		/// Gets or sets the value indicating the number of requests that returned cached object.
		/// </summary>
		public long HitCount
		{
			get { return _hitCount; }
		}

		/// <summary>
		/// Gets the value indicating the % of requests that returned cached object.
		/// </summary>
		public double HitCountPercentage
		{
			get
			{
				if (this.RequestCount == 0) return 0;
				return Math.Round(this.HitCount * 100 / (double)this.RequestCount, 2);
			}
		}

		private long _missCount = 0;
		/// <summary>
		/// Gets or sets the value indicating the number of requests that didn't return cached object.
		/// </summary>
		public long MissCount
		{
			get { return _missCount; }
		}

		/// <summary>
		/// Gets the value indicating the % of requests that didn't return cached object.
		/// </summary>
		public double MissCountPercentage
		{
			get
			{
				if (this.RequestCount == 0) return 0;
				return Math.Round(this.MissCount * 100 / (double)this.RequestCount, 2);
			}
		}

		public IDictionary<string, CacheItemDisplay> Info { get; set; }

		#endregion

		#region [ Public Methods ]

		/// <summary>
		/// Increments the RequestCount value.
		/// </summary>
		public void IncrementRequestCount()
		{
			Interlocked.Increment(ref _requestCount);
		}

		/// <summary>
		/// Increments the HitCount value.
		/// </summary>
		public void IncrementHitCount()
		{
			Interlocked.Increment(ref _hitCount);
		}

		/// <summary>
		/// Increments the MissCount value.
		/// </summary>
		public void IncrementMissCount()
		{
			Interlocked.Increment(ref _missCount);
		}

		#endregion
	}
}

// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Collections.Generic;

	public interface ICacheStatistics
	{
		/// <summary>
		/// Gets the value indicating the number of items in the cache.
		/// </summary>
		long ItemCount { get; }

		/// <summary>
		/// Gets the value indicating the number of requests.
		/// </summary>
		long RequestCount { get; }

		/// <summary>
		/// Gets the value indicating the number of requests that returned cached object.
		/// </summary>
		long HitCount { get; }

		/// <summary>
		/// Gets the value indicating the % of requests that returned cached object.
		/// </summary>
		double HitCountPercentage { get; }

		/// <summary>
		/// Gets the value indicating the number of requests that didn't return cached object.
		/// </summary>
		long MissCount { get; }

		/// <summary>
		/// Gets the value indicating the % of requests that didn't return cached object.
		/// </summary>
		double MissCountPercentage { get; }

		IDictionary<string, CacheItemDisplay> Info { get; }
	}
}

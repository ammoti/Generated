// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Diagnostics;

	[DebuggerDisplay("{Key} (isExpired = {IsExpired})")]
	public class CacheItemDisplay
	{
		#region [ Constructor ]

		public CacheItemDisplay()
		{
		}

		public CacheItemDisplay(CacheItem cacheItem)
		{
			if (cacheItem != null)
			{
				this.Key = cacheItem.Key;
				this.Expiration = cacheItem.Expiration;
				this.IsExpired = cacheItem.IsExpired;
				this.Description = cacheItem.Description;
			}
		}

		#endregion

		#region [ Properties ]

		public string Key { get; set; }

		public string Description { get; set; }

		public DateTime Expiration { get; set; }

		public bool IsExpired { get; set; }

		#endregion
	}
}

// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	/// <summary>
	/// Provides custom TimeSpan expirations.
	/// </summary>
	public class CacheDuration
	{
		/// <summary>
		/// 5 minutes.
		/// </summary>
		public static readonly TimeSpan Short = TimeSpan.FromMinutes(5);

		/// <summary>
		/// 1 hour.
		/// </summary>
		public static readonly TimeSpan Medium = TimeSpan.FromHours(1);

		/// <summary>
		/// 5 hours.
		/// </summary>
		public static readonly TimeSpan Long = TimeSpan.FromHours(5);

		/// <summary>
		/// 1 day.
		/// </summary>
		public static readonly TimeSpan OneDay = TimeSpan.FromDays(1);

		/// <summary>
		/// 1 month.
		/// </summary>
		public static readonly TimeSpan OneMonth = TimeSpan.FromDays(31);
	}
}

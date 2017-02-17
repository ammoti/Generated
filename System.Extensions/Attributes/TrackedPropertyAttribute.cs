// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	/// <summary>
	/// TrackedProperty attribute.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	sealed public class TrackedPropertyAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new instance of the TrackedPropertyAttribute class.
		/// </summary>
		public TrackedPropertyAttribute()
		{
		}
	}
}

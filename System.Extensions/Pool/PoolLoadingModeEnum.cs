// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.ComponentModel;

	[DefaultValue(PoolLoadingModeEnum.Lazy)]
	public enum PoolLoadingModeEnum
	{
		/// <summary>
		/// All the instances are created when the pool is instanciated.
		/// </summary>
		Eager,

		/// <summary>
		/// Instances are created when required.
		/// </summary>
		Lazy,

		/// <summary>
		/// Instances are created when required, since the pool is full.
		/// </summary>
		LazyExpanding
	}
}

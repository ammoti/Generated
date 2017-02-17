// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.ComponentModel;

	/// <summary>
	/// Instancing mode enumeration.
	/// </summary>
	[DefaultValue(InstancingMode.NewInstance)]
	public enum InstancingMode
	{
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		NewInstance,

		/// <summary>
		/// Uses existing instance or creates new one and registers it for next uses.
		/// </summary>
		Singleton
	}
}

// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.ComponentModel;

	[DefaultValue(PoolAccessModeEnum.FIFO)]
	public enum PoolAccessModeEnum
	{
		/// <summary>
		/// First In First Out.
		/// </summary>
		FIFO,

		/// <summary>
		/// Last In First Out.
		/// </summary>
		LIFO,

		/// <summary>
		/// Circular.
		/// </summary>
		Circular
	};
}

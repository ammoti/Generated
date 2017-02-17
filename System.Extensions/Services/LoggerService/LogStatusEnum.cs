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
	/// LogStatus enumeration.
	/// </summary>
	[DefaultValue(LogStatusEnum.Info)]
	public enum LogStatusEnum
	{
		Info = 0,
		Debug = 1,
		Warning = 2,
		Error = 3
	}
}

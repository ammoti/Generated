// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace LayerCake.Generator.Commons
{
	using System.ComponentModel;

	[DefaultValue(ProcessStateEnum.Unknown)]
	public enum ProcessStateEnum
	{
		Unknown,
		Processed,
		NotProcessed,
		Failed
	}
}

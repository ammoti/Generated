// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Diagnostics;

	public class DebugExecutionTracerService : ActionExecutionTracerService
	{
		public DebugExecutionTracerService(string description)
			: base((val, desc) => Debug.WriteLine("DebugExecutionTracerService[{0}] -> {1}ms", desc, val))
		{
			_description = description;
		}
	}
}

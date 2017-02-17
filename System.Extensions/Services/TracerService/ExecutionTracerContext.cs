// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	public static class ExecutionTracerContext
	{
		static ExecutionTracerContext()
		{
			TraceCrudMethods = true;
			TraceBusinessMethods = true;
			TraceServiceMethods = true;
		}

		public static bool TraceCrudMethods { get; set; }
		public static bool TraceBusinessMethods { get; set; }
		public static bool TraceServiceMethods { get; set; }
	}
}

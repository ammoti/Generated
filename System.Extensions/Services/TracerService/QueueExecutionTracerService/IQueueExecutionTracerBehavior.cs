// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Collections.Generic;

	public interface IQueueExecutionTracerBehavior<T>
	{
		T CreateItem(string module, string className, string methodName, string tag, long duration);

		void TraceItems(IEnumerable<T> traces);
	}
}

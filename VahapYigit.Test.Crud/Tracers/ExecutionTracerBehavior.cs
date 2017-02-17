// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.Crud
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using VahapYigit.Test.Core;
	using VahapYigit.Test.Models;

	class ExecutionTracerBehavior : IQueueExecutionTracerBehavior<ExecutionTraceModel>
	{
		#region [ IQueueExecutionTracerBehavior Implementation ]

		public ExecutionTraceModel CreateItem(string module, string className, string methodName, string tag, long duration)
		{
			if (tag != null)
			{
				if (string.Compare(tag, "ExecutionTrace_Custom_Trace", true) == 0) // #hack - To avoid loop (Crud.ExecuteNonQuery())
					return null;

				if (tag.StartsWith("Translation_")) // Do not track Translation operations
					return null;
			}

			return new ExecutionTraceModel(module, className, methodName, tag, duration);
		}

		public void TraceItems(IEnumerable<ExecutionTraceModel> traces)
		{
			if (ExecutionTracerContext.TraceCrudMethods)
			{
				using (var db = new ExecutionTraceCrud(ClientContext.Server))
				{
					db.Trace(traces.ToArray());
				}
			}
		}

		#endregion
	}
}

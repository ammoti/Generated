// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using VahapYigit.Test.Contracts;
	using VahapYigit.Test.Core;
	using VahapYigit.Test.Models;

	class ExecutionTracerBehavior : IQueueExecutionTracerBehavior<ExecutionTraceModel>
	{
		#region [ IQueueExecutionTracerBehavior Implementation ]

		public ExecutionTraceModel CreateItem(string module, string className, string methodName, string tag, long duration)
		{
			return new ExecutionTraceModel(module, className, methodName, tag, duration);
		}

		public void TraceItems(IEnumerable<ExecutionTraceModel> traces)
		{
			if (ExecutionTracerContext.TraceServiceMethods)
			{
				IExecutionTraceService service = ServiceLocator.Current.Resolve<IExecutionTraceService>();
				if (service != null)
				{
					service.Trace(ClientContext.Server, traces.ToArray());
				}
			}
		}

		#endregion
	}
}

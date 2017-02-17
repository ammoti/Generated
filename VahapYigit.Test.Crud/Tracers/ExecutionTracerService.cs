// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.Crud
{
	using System;

	using VahapYigit.Test.Models;

	class ExecutionTracerService : QueueExecutionTracerService<ExecutionTraceModel>
	{
		public ExecutionTracerService(string tag = null)
			: base(tag)
		{
			_behavior = new VahapYigit.Test.Crud.ExecutionTracerBehavior();
		}
	}
}
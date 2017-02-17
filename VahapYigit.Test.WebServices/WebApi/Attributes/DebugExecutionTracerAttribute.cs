// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace VahapYigit.Test.WebServices.WebApi
{
	using System;
	using System.Web.Http.Controllers;
	using System.Web.Http.Filters;

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
	public class DebugExecutionTracerAttribute : ActionFilterAttribute, IActionFilter, IDisposable
	{
		#region [ Members ]

		private ITracerService _tracerService = null;

		#endregion

		#region [ Constructor ]

		public DebugExecutionTracerAttribute()
		{
		}

		~DebugExecutionTracerAttribute()
		{
			this.Dispose(false);
		}

		#endregion

		#region [ ActionFilterAttribute Members ]

		public override void OnActionExecuting(HttpActionContext context)
		{
			string controllerName = context.ControllerContext.ControllerDescriptor.ControllerName;
			string controllerActionName = context.ActionDescriptor.ActionName;

			_tracerService = new DebugExecutionTracerService(string.Format("{0}::{1}()", controllerName, controllerActionName));
		}

		public override void OnActionExecuted(HttpActionExecutedContext context)
		{
			_tracerService.SafeDispose(); // work is performed in the inner Disposed() method or the _tracerService instance
			_tracerService = null;
		}

		#endregion

		#region [ IDisposable Implementation ]

		// for Code Analysis...

		private bool _isDisposed = false;

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_isDisposed)
			{
				if (_tracerService != null) // should never occurred -> OnActionExecuted()
				{
					_tracerService.Dispose();
					_tracerService = null;
				}

				_isDisposed = true;
			}
		}

		#endregion
	}
}
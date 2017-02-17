// -----------------------------------------------
// This file is part of the LayerCake Generator.
// 
// Copyright (c) 2012, 2015 LayerCake Generator.
// http://www.layercake-generator.net
// -----------------------------------------------

namespace System
{
	using System.Diagnostics;

	public class ActionExecutionTracerService : ITracerService
	{
		#region [ Members ]

		private readonly Stopwatch _timeWatcher = null;

		private readonly Action<long, string> _action = null;

		protected string _description = null;

		#endregion

		#region [ Constructors ]

		public ActionExecutionTracerService(Action<long, string> action, string description = null)
		{
			_timeWatcher = new Stopwatch();
			_timeWatcher.Start();

			_action = action;
			_description = description;
		}

		~ActionExecutionTracerService()
		{
			this.Dispose(false);
		}

		#endregion

		#region [ ITracerService Implementation ]

		private bool _isDisposed;

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (!_isDisposed)
				{
					_timeWatcher.Stop();

					if (_action != null)
					{
						_action(_timeWatcher.ElapsedMilliseconds, _description);
					}

					_isDisposed = true;
				}
			}
		}

		#endregion
	}
}
